using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// using System.Diagnostics;

public class RandomTileMapGenerator : MonoBehaviour {

    [SerializeField] private MazeDiggerConfig mazeDiggerConfig;
    [SerializeField] private TileMapDrawerConfig tileMapDrawerConfig;

    [SerializeField] private String activePlayerReference = null;
    [SerializeField] private Vector2Int areaOfView = new Vector2Int(10, 6);
    [SerializeField] private float updateMapEvery = 1.0F;

    private TileMapDrawer TileMapDrawer = new TileMapDrawer();
    private MazeBlueprint mazeBlueprint = null;
    private Tilemap tilemap = null;

    void Start() {

        TileMapDrawer.setDrawableTiles(ref tileMapDrawerConfig.drawableTiles);

        this.tilemap = GetComponent<Tilemap>();
        this.mazeBlueprint = generateBlueprint();

        StartCoroutine(drawMapAroundPlayer());
        // StartCoroutine(drawInitialSeeder());
        // StartCoroutine("drawWholeMapPartially");   
    }

    private MazeBlueprint generateBlueprint() {
        /* Define your maze here ! */

        int mazeHeight = tileMapDrawerConfig.mapSize.y;
        int mazeWidth = tileMapDrawerConfig.mapSize.x;

        MazeBuilder mazeBuilder = new MazeBuilder();
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
        MazeBlueprint blueprint = mazeBuilder.checkoutBlueprint();

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

    private IEnumerator drawWholeMapPartially() {
        /* TODO: really needed ?  */

        int mapWidth = 40;
        int mapHeight = 40;

        for (int i = 0; i * areaOfView.y < mapHeight * 2; i++) {
            for (int j = 0; j * areaOfView.x < mapWidth * 2; j++) {
                TileMapDrawer.constructPartialMaze(mazeBlueprint,
                    i * areaOfView.y, j * areaOfView.x,
                    i * areaOfView.y + areaOfView.y, j * areaOfView.x + areaOfView.x);

                drawMapSet();
                yield return new WaitForSeconds(updateMapEvery);
            }
        }
    }

    private IEnumerator drawMapAroundPlayer() {

        GameObject player = GameObject.Find(activePlayerReference);
        Transform transformData = GetComponent<Transform>();

        if (player == null || transformData == null) {
            yield return null;
        }

        Vector3 tileMapScale = transformData.localScale;

        int lastDrawXonBlueprint = 999;
        int lastDrawYonBlueprint = 999;

        while (true) {

            Vector3 position = player.transform.position;
            int playerXonBlueprint = (int) (position.x / tileMapScale.x);
            int playerYonBlueprint = (int) (position.y / tileMapScale.y);

            int deltaXonBlueprint = Math.Abs(playerXonBlueprint - lastDrawXonBlueprint);
            int deltaYonBlueprint = Math.Abs(playerYonBlueprint - lastDrawYonBlueprint);

            // If map still relevant
            if (deltaXonBlueprint < (areaOfView.x / 4) && deltaYonBlueprint < (areaOfView.y / 3)) {
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

            Vector3Int[] tilePosition = TileMapDrawer.getTilePosition();
            TileBase[] tileArray = TileMapDrawer.getTileArray();
            for (int i = 0; i < tilePosition.Length; i++) {
                tilemap.SetTile(tilePosition[i], tileArray[i]);
                yield return new WaitForSeconds(0.0F);
            }
        }
    }

    private IEnumerator drawInitialSeeder() {

        Transform transformData = GetComponent<Transform>();
        Vector3 tileMapScale = transformData.localScale;

        int mazeHeight = tileMapDrawerConfig.mapSize.y * 2;
        int mazeWidth = tileMapDrawerConfig.mapSize.x * 2;

        while (true) {

            int drawPositionX = UnityEngine.Random.Range(0, mazeWidth);
            int drawPositionY = UnityEngine.Random.Range(0, mazeHeight);

            // Fetch data
            TileMapDrawer.constructPartialMaze(mazeBlueprint,
                drawPositionY - 5, drawPositionX - 5,
                drawPositionY + 5, drawPositionX + 5);

            // Draw the tile
            Vector3Int[] tilePosition = TileMapDrawer.getTilePosition();
            TileBase[] tileArray = TileMapDrawer.getTileArray();
            for (int i = 0; i < tilePosition.Length; i++) {
                tilemap.SetTile(tilePosition[i], tileArray[i]);
                yield return new WaitForSeconds(0.0F);
            }

            yield return new WaitForSeconds(5.0F);
        }
    }


    private void drawMapSet() {
        if (tilemap != null) {
            tilemap.SetTiles(TileMapDrawer.getTilePosition(), TileMapDrawer.getTileArray());
        }
    }
}