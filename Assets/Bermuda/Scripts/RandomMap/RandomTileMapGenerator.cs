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
    private Tilemap tilemap = null;

    void Start() {

        TileMapDrawer.setDrawableTiles(ref tileMapDrawerConfig.drawableTiles);

        this.tilemap = GetComponent<Tilemap>();
        this.mazeBlueprint = generateBlueprint();

        StartCoroutine("drawMapAroundPlayer");
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
        MazeBlueprint mazeBlueprint = mazeBuilder.checkoutBlueprint();

        // Maze maze = mazeBuilder.checkoutMaze();
        // String debugPath = "C:\\Code\\Unity\\Bermuda\\Assets\\Bermuda\\Debug\\TileMap\\";
        // maze.printMaze(debugPath + "result-Maze.txt");
        // maze.printStatistic(debugPath + "result-Stats.txt");
        // mazeBlueprint.printBlueprint(debugPath + "result-Blueprint.txt");

        return mazeBlueprint;
    }

    private IEnumerator drawWholeMapPartially() {
        /* TODO: really needed ?  */

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

        GameObject player = GameObject.Find("Submarine01");
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
            if (deltaXonBlueprint < areaOfView / 2 && deltaYonBlueprint < areaOfView / 2) {
                yield return new WaitForSeconds(0.1F);
                continue;
            }

            // Set update flag
            lastDrawXonBlueprint = playerXonBlueprint;
            lastDrawYonBlueprint = playerYonBlueprint;
            
            // Fetch data
            TileMapDrawer.constructPartialMaze(mazeBlueprint,
                playerYonBlueprint - areaOfView, playerXonBlueprint - areaOfView,
                playerYonBlueprint + areaOfView, playerXonBlueprint + areaOfView);

            // Draw the tile
            Vector3Int[] tilePosition = TileMapDrawer.getTilePosition();
            TileBase[] tileArray = TileMapDrawer.getTileArray();
            for (int i = 0; i < tilePosition.Length; i++) {
                tilemap.SetTile(tilePosition[i], tileArray[i]);
                yield return new WaitForSeconds(0.0F);
            }
        }
    }

    private void drawMapSet() {
        if (tilemap != null) {
            tilemap.SetTiles(TileMapDrawer.getTilePosition(), TileMapDrawer.getTileArray());
        }
    }
}