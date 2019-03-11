using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomTileMapGenerator : MonoBehaviour {

    [SerializeField] private MazeDiggerConfig mazeDiggerConfig;
    [SerializeField] private TileMapDrawerConfig tileMapDrawerConfig;

    [SerializeField] private int areaOfView = 10;
    [SerializeField] private int updateMapEvery = 500;

    private TileMapDrawer TileMapDrawer = new TileMapDrawer();
    private MazeBlueprint mazeBlueprint = null;
    private GameObject player;
    private int updateTimer = 0;

    // Start is called before the first frame update
    void Start() {

        // Pass tiles to tileMapDrawer
        TileMapDrawer.setStaticTiles(ref tileMapDrawerConfig.drawableTiles);

        this.findPlayer();
        this.mazeBlueprint = generateBlueprint();
    }

    void Update() {

        if (updateTimer == 0) {
            StartCoroutine("drawMapAroundPlayer");
            updateTimer = updateMapEvery;
        } else {
            updateTimer--;
        }
    }

    private void findPlayer() {
        player = GameObject.Find("Ellen");
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
        Tilemap tilemap = GetComponent<Tilemap>();
        tilemap.SetTiles(TileMapDrawer.getTilePosition(), TileMapDrawer.getTileArray());
    }

    private IEnumerator drawMapAroundPlayer() {

        Vector3 position = player.transform.position;
        int playerX = (int) (position.x);
        int playerY = (int) (position.y);

        TileMapDrawer.constructPartialMaze(mazeBlueprint,
            playerY - areaOfView, playerX - areaOfView,
            playerY + areaOfView, playerX + areaOfView);

        drawMapSet();
        yield return null;
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