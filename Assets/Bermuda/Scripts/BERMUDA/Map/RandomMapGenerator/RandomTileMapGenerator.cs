using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

// using System.Diagnostics;

public class RandomTileMapGenerator : MonoBehaviour {
    [SerializeField] private MazeDiggerConfig mazeDiggerConfig = null;
    [SerializeField] private TileMapDrawerConfig tileMapDrawerConfig = null;

    [SerializeField] private String activePlayerReference = null;
    [SerializeField] private Vector2Int areaOfView = new Vector2Int(10, 6);

    private TileMapDrawer TileMapDrawer = new TileMapDrawer();
    private MazeBlueprint mazeBlueprint = null;
    private Tilemap tilemap = null;

    void Start() {

        UnsetBlueprintFromPlayerPref();

        SetupBlueprint();
        SetupTileMap();
        SetupMapDrawer();

        SetBlueprintToPlayerPref();

        StartCoroutine(drawMapAroundPlayer());
    }

    private void SetupBlueprint() {
        try {
            this.mazeBlueprint = FetchBluePrint();
        } catch (Exception e) {
            this.mazeBlueprint = GenerateBlueprint();
            Debug.Log(e);
        }
    }

    private MazeBlueprint FetchBluePrint() {
        try {
            var serializedBlueprint = Util.Get(NetworkManager.BaseUrl + "/api/map?matchId=33");
            MazeBlueprint blueprint = new MazeBlueprint(-1, -1);
            blueprint.deserialize(serializedBlueprint);
            return blueprint;

        } catch (Exception e) {
            throw e;
        }
    }

    private MazeBlueprint GenerateBlueprint() {
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
        this.tileMapDrawerConfig.mapSize *= 2;

        mazeBuilder.GenerateBluePrint();
        return mazeBuilder.checkoutBlueprint();
    }

    private void UnsetBlueprintFromPlayerPref() {
        PlayerPrefs.SetString("mazeBlueprint", "");
        PlayerPrefs.SetInt("mazeBlueprintReady", -1);
    }

    private void SetBlueprintToPlayerPref() {
        PlayerPrefs.SetString("mazeBlueprint", this.mazeBlueprint.serialize());
        PlayerPrefs.SetInt("mazeBlueprintReady", 200);
    }

    private IEnumerator drawMapAroundPlayer() {
        var player = GameObject.Find(activePlayerReference);
        var transformData = GetComponent<Transform>();

        if (player == null || transformData == null) {
            yield return null;
        }

        var tileMapScale = transformData.localScale;
        var lastDrawXonBlueprint = 999;
        var lastDrawYonBlueprint = 999;

        int bluePrintAvailable = 0;
        while (bluePrintAvailable != 200) {
            yield return new WaitForSeconds(0.2F);
            bluePrintAvailable = (int) PlayerPrefs.GetInt("mazeBlueprintReady");
            Debug.Log("blueprint unv");
        }

        while (player != null) {
            var position = player.transform.position;
            var playerXonBlueprint = (int) (position.x / tileMapScale.x);
            var playerYonBlueprint = (int) (position.y / tileMapScale.y);

            var deltaXonBlueprint = Math.Abs(playerXonBlueprint - lastDrawXonBlueprint);
            var deltaYonBlueprint = Math.Abs(playerYonBlueprint - lastDrawYonBlueprint);

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

            var tilePosition = TileMapDrawer.getTilePosition();
            var tileArray = TileMapDrawer.getTileArray();
            for (var i = 0; i < tilePosition.Length; i++) {
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

    private void SetupMapDrawer() {
        TileMapDrawer.setDrawableTiles(ref this.tileMapDrawerConfig.drawableTiles);
        TileMapDrawer.setMapSize(this.tileMapDrawerConfig.mapSize);
    }

    private void SetupTileMap() {
        this.tilemap = GetComponent<Tilemap>();
    }
}