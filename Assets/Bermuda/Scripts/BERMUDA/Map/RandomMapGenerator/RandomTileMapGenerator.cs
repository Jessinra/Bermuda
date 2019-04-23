using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

// using System.Diagnostics;

public class RandomTileMapGenerator : MonoBehaviour
{
    [SerializeField] private MazeDiggerConfig mazeDiggerConfig = null;
    [SerializeField] private TileMapDrawerConfig tileMapDrawerConfig = null;

    [SerializeField] private String activePlayerReference = null;
    [SerializeField] private Vector2Int areaOfView = new Vector2Int(10, 6);
    [SerializeField] private float updateMapEvery = 1.0F;
    public bool generate;

    private TileMapDrawer TileMapDrawer = new TileMapDrawer();
    private MazeBlueprint mazeBlueprint = null;
    private Tilemap tilemap = null;

    void Start()
    {
        TileMapDrawer.setDrawableTiles(ref tileMapDrawerConfig.drawableTiles);

        this.tilemap = GetComponent<Tilemap>();
        Task.Run(() => LoadBluePrint(() =>
        {
            StartLoadMap();
            return "";
        }, () =>
        {
            Generate();
            return "";
        }));


        // StartCoroutine(drawInitialSeeder());
        // StartCoroutine("drawWholeMapPartially");   
    }

    private void Generate()
    {
        this.mazeBlueprint = generateBlueprint();
    }

    private void LoadBluePrint(Func<string> onSuccess, Func<string> onFailure)
    {
        try
        {
            var map = JsonUtility.FromJson<Map>(Util.Get(NetworkManager.BaseUrl + "/api/map"));
            var blueprint = new MazeBlueprint(map.height, map.width);
            mazeBlueprint = blueprint;
            onSuccess();
        }
        catch (Exception)
        {
            onFailure();
        }
    }

    private void StartLoadMap()
    {
        StartCoroutine(drawMapAroundPlayer());
    }

    private MazeBlueprint generateBlueprint()
    {
        /* Define your maze here ! */

        var mazeHeight = tileMapDrawerConfig.mapSize.y;
        var mazeWidth = tileMapDrawerConfig.mapSize.x;

        var mazeBuilder = new MazeBuilder();
        mazeBuilder.generateMaze(mazeHeight, mazeWidth);

        mazeBuilder.addHallDigger(mazeDiggerConfig.nHallDigger);
        mazeBuilder.addEdgeDigger(mazeDiggerConfig.nEdgeDigger);
        mazeBuilder.addPathDigger(mazeDiggerConfig.nPathDigger);

        mazeBuilder.runHallDigger(mazeDiggerConfig.hallPercent);
        mazeBuilder.runEdgeDigger(mazeDiggerConfig.edgePercent);
        mazeBuilder.runPathDigger(mazeDiggerConfig.pathPercent);

        mazeBuilder.expandMaze(2);
        TileMapDrawer.setMapSize(tileMapDrawerConfig.mapSize * 2);

        mazeBuilder.generateBluePrint();
        var blueprint = mazeBuilder.checkoutBlueprint();

        // Serialize blueprint and store locally (for other module) and on server
        PlayerPrefs.SetString("mazeBlueprint", blueprint.serialize());
        PlayerPrefs.SetInt("mazeBlueprintReady", 200);

        // Maze maze = mazeBuilder.checkoutMaze();
        // String debugPath = "C:\\Code\\Unity\\Bermuda\\Assets\\Bermuda\\Debug\\TileMap\\";
        // maze.printMaze(debugPath + "result-Maze.txt");
        // maze.printStatistic(debugPath + "result-Stats.txt");
        // mazeBlueprint.printBlueprint(debugPath + "result-Blueprint.txt");

        return blueprint;
    }

    private IEnumerator drawWholeMapPartially()
    {
        /* TODO: really needed ?  */

        const int mapWidth = 40;
        const int mapHeight = 40;

        for (var i = 0; i * areaOfView.y < mapHeight * 2; i++)
        {
            for (var j = 0; j * areaOfView.x < mapWidth * 2; j++)
            {
                TileMapDrawer.constructPartialMaze(mazeBlueprint,
                    i * areaOfView.y, j * areaOfView.x,
                    i * areaOfView.y + areaOfView.y, j * areaOfView.x + areaOfView.x);

                drawMapSet();
                yield return new WaitForSeconds(updateMapEvery);
            }
        }
    }

    private IEnumerator drawMapAroundPlayer()
    {
        var player = GameObject.Find(activePlayerReference);
        var transformData = GetComponent<Transform>();

        if (player == null || transformData == null)
        {
            yield return null;
        }

        var tileMapScale = transformData.localScale;

        var lastDrawXonBlueprint = 999;
        var lastDrawYonBlueprint = 999;

        while (true)
        {
            var position = player.transform.position;
            var playerXonBlueprint = (int) (position.x / tileMapScale.x);
            var playerYonBlueprint = (int) (position.y / tileMapScale.y);

            var deltaXonBlueprint = Math.Abs(playerXonBlueprint - lastDrawXonBlueprint);
            var deltaYonBlueprint = Math.Abs(playerYonBlueprint - lastDrawYonBlueprint);

            // If map still relevant
            if (deltaXonBlueprint < (areaOfView.x / 4) && deltaYonBlueprint < (areaOfView.y / 3))
            {
                yield return new WaitForSeconds(0.25F);
                continue;
            }

            // Set update flag
            lastDrawXonBlueprint = playerXonBlueprint;
            lastDrawYonBlueprint = playerYonBlueprint;

            // Fetch data
            TileMapDrawer.constructPartialMazeCenter(mazeBlueprint,
                playerYonBlueprint - areaOfView.y, playerXonBlueprint - areaOfView.x,
                playerYonBlueprint + areaOfView.y, playerXonBlueprint + areaOfView.x);

            var tilePosition = TileMapDrawer.getTilePosition();
            var tileArray = TileMapDrawer.getTileArray();
            for (var i = 0; i < tilePosition.Length; i++)
            {
                tilemap.SetTile(tilePosition[i], tileArray[i]);
                yield return new WaitForSeconds(0.0F);
            }
        }
    }

    private IEnumerator drawInitialSeeder()
    {
        var transformData = GetComponent<Transform>();
        var tileMapScale = transformData.localScale;

        var mazeHeight = tileMapDrawerConfig.mapSize.y * 2;
        var mazeWidth = tileMapDrawerConfig.mapSize.x * 2;

        while (true)
        {
            var drawPositionX = UnityEngine.Random.Range(0, mazeWidth);
            var drawPositionY = UnityEngine.Random.Range(0, mazeHeight);

            // Fetch data
            TileMapDrawer.constructPartialMaze(mazeBlueprint,
                drawPositionY - 5, drawPositionX - 5,
                drawPositionY + 5, drawPositionX + 5);

            // Draw the tile
            var tilePosition = TileMapDrawer.getTilePosition();
            var tileArray = TileMapDrawer.getTileArray();
            for (var i = 0; i < tilePosition.Length; i++)
            {
                tilemap.SetTile(tilePosition[i], tileArray[i]);
                yield return new WaitForSeconds(0.0F);
            }

            yield return new WaitForSeconds(5.0F);
        }
    }


    private void drawMapSet()
    {
        if (tilemap != null)
        {
            tilemap.SetTiles(TileMapDrawer.getTilePosition(), TileMapDrawer.getTileArray());
        }
    }
}