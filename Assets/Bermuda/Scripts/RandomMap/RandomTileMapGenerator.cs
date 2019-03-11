using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomTileMapGenerator : MonoBehaviour {

    [SerializeField] private MazeDiggerConfig mazeDiggerConfig;
    [SerializeField] private TileMapDrawerConfig tileMapDrawerConfig;

    [SerializeField] private int areaOfView = 10;
    [SerializeField] private float updateMapEvery = 1.0F;

    private TileMapDrawer TileMapDrawer = new TileMapDrawer();
    private MazeBlueprint mazeBlueprint = null;

    private GameObject player;
    private Tilemap tilemap;
    private Transform transformData;

    // Start is called before the first frame update
    void Start() {

        // Pass tiles to tileMapDrawer
        TileMapDrawer.setStaticTiles(ref tileMapDrawerConfig.drawableTiles);

        this.findPlayer();
        this.setTileMap();
        this.setTransform();
        this.mazeBlueprint = generateBlueprint();

        StartCoroutine("drawMapAroundPlayer"); // draw little by little
        // StartCoroutine("drawMap");   // draw all at once 
    }

    void Update() { }

    private void findPlayer() {
        player = GameObject.Find("Ellen");
    }

    private void setTileMap() {
        tilemap = GetComponent<Tilemap>();
    }

    private void setTransform() {
        transformData = GetComponent<Transform>();
    }

    private MazeBlueprint generateBlueprint() {

        int mazeHeight = tileMapDrawerConfig.mapSize.y;
        int mazeWidth = tileMapDrawerConfig.mapSize.x;

        MazeBuilder mazeBuilder = new MazeBuilder();
        mazeBuilder.generateMaze(mazeHeight, mazeWidth);

        mazeBuilder.addPathDigger(mazeDiggerConfig.nPathDigger);
        mazeBuilder.addHallDigger(mazeDiggerConfig.nHallDigger);
        mazeBuilder.addEdgeDigger(mazeDiggerConfig.nEdgeDigger);

        mazeBuilder.runPathDigger(mazeDiggerConfig.pathPercent);
        mazeBuilder.runHallDigger(mazeDiggerConfig.hallPercent);
        mazeBuilder.runEdgeDigger(mazeDiggerConfig.edgePercent);

        mazeBuilder.expandMaze(2);
        TileMapDrawer.setMapSize(tileMapDrawerConfig.mapSize * 2);

        mazeBuilder.generateBluePrint();

        Maze maze = mazeBuilder.checkoutMaze();
        MazeBlueprint mazeBlueprint = mazeBuilder.checkoutBlueprint();

        String debugPath = "C:\\Code\\Unity\\Bermuda\\Assets\\Bermuda\\Debug\\TileMap\\";
        maze.printMaze(debugPath + "result-Maze.txt");
        maze.printStatistic(debugPath + "result-Stats.txt");
        mazeBlueprint.printBlueprint(debugPath + "result-Blueprint.txt");

        return mazeBlueprint;
    }

    private void drawMapSet() {
        tilemap.SetTiles(TileMapDrawer.getTilePosition(), TileMapDrawer.getTileArray());
    }

    private IEnumerator drawMap() {
        /* TODO: really ?  */

        int mapWidth = 40;
        int mapHeight = 40;

        for (int i = 0; i * areaOfView < mapHeight * 2; i++) {
            for (int j = 0; j * areaOfView < mapWidth * 2; j++) {
                TileMapDrawer.constructPartialMaze(mazeBlueprint,
                    i * areaOfView, j * areaOfView,
                    i * areaOfView + areaOfView, j * areaOfView + areaOfView);

                drawMapSet();
                yield return new WaitForSeconds(updateMapEvery);
            }
        }
    }

    private IEnumerator drawMapAroundPlayer() {

        while (true) {

            Vector3 position = player.transform.position;
            Vector3 tileMapScale = transformData.localScale;

            int playerXonBlueprint = (int) (position.x / tileMapScale.x);
            int playerYonBlueprint = (int) (position.y / tileMapScale.y);

            TileMapDrawer.constructPartialMaze(mazeBlueprint,
                playerYonBlueprint - areaOfView, playerXonBlueprint - areaOfView,
                playerYonBlueprint + areaOfView, playerXonBlueprint + areaOfView);

            drawMapSet();
            yield return new WaitForSeconds(updateMapEvery);
        }
    }
}

[System.Serializable]
public class MazeDiggerConfig {
    public int nPathDigger;
    public int nHallDigger;
    public int nEdgeDigger;

    public float pathPercent;
    public float hallPercent;
    public float edgePercent;
}

[System.Serializable]
public class TileMapDrawerConfig {
    public Vector2Int mapSize;
    public DrawableTilesContainer drawableTiles;
}