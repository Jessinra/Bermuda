using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapDrawer {
    /* Design note: 
    Part of RandomTileMapGenerator, function as data cointainer 
    Separated from RandomTileMapGenerator to avoid inheriting MonoBehaviour (Segregation)
    */

    private Vector2Int mapSize;
    private List<TileBase> tileList;
    private List<Vector3Int> tilePosition;    
    private bool[] tileDrawn;

    private DrawableTilesContainer drawableTiles;

    /* =================================================
                        Getter Setter
    ================================================= */

    public void setMapSize(Vector2Int size) {
        this.mapSize = size;
        this.tileDrawn = new bool[size.x * size.y];
    }

    public void initTileList() {
        this.tileList = new List<TileBase>();
    }

    public void initTilePositionList() {
        this.tilePosition = new List<Vector3Int>();
    }

    public void setDrawableTiles(ref DrawableTilesContainer drawableTiles) {
        this.drawableTiles = drawableTiles;
    }

    public Vector3Int[] getTilePosition() {
        return tilePosition.ToArray();
    }

    public TileBase[] getTileArray() {
        return tileList.ToArray();
    }

    /* =================================================
                        Main Method
    ================================================= */

    public void constructMaze(MazeBlueprint blueprint) {

        this.initTileList();
        this.initTilePositionList();

        for (int i = 0; i < blueprint.getMazeHeight(); i++) {
            for (int j = 0; j < blueprint.getMazeWidth(); j++) {

                String tileType = blueprint.getTileType(i, j);
                if (tileType != "Empty") {
                    addTile(j, i, drawableTiles.getTile(tileType));
                }
            }
        }
    }

    public void constructPartialMaze(MazeBlueprint blueprint, int startRow, int startCol, int endRow, int endCol) {

        this.adjustBorder(ref blueprint, ref startRow, ref startCol, ref endRow, ref endCol);
        this.initTileList();
        this.initTilePositionList();        

        for (int i = startRow; i < endRow; i++) {
            for (int j = startCol; j < endCol; j++) {

                if (this.isTileDrawn(j, i)) {
                    break;
                }

                String tileType = blueprint.getTileType(i, j);
                if (tileType != "Empty") {
                    addTile(j, i, drawableTiles.getTile(tileType));
                }
            }
        }
    }

    /* DEPRECATED : use scale to resize map instead of using more tiles. */
    public void constructBigMaze(MazeBlueprint blueprint, int scale) {

        this.initTileList();
        this.initTilePositionList();

        for (int i = 0; i < blueprint.getMazeHeight(); i++) {
            for (int j = 0; j < blueprint.getMazeWidth(); j++) {

                String tileType = blueprint.getTileType(i, j);
                if (tileType != "Empty") {
                    addTile(j * scale, i * scale, drawableTiles.getTile(tileType));
                }
            }
        }
    }

    private void adjustBorder(ref MazeBlueprint blueprint, ref int startRow, ref int startCol, ref int endRow, ref int endCol) {
        if (startRow < 0) {
            startRow = 0;
        }
        if (endRow > blueprint.getMazeHeight()) {
            endRow = blueprint.getMazeHeight();
        }
        if (startCol < 0) {
            startCol = 0;
        }
        if (endCol > blueprint.getMazeWidth()) {
            endCol = blueprint.getMazeWidth();
        }
    }

    /* =================================================
                        Drawing Method
    ================================================= */

    private void addTile(int x, int y, TileBase tile) {
        tilePosition.Add(new Vector3Int(x, y, 0));
        tileList.Add(tile);
        tileDrawn[getLinearIndex(x, y)] = true;
    }

    private void addBlockTile(int xStart, int yStart, int xEnd, int yEnd, TileBase tile) {

        if (yStart > yEnd) {
            swapInt(ref yStart, ref yEnd);
        }

        if (xStart > xEnd) {
            swapInt(ref xStart, ref xEnd);
        }

        for (int i = yStart; i < yEnd; i++) {
            addHorizontalLine(xStart, xEnd, i, tile);
        }
    }

    private void addHorizontalLine(int xStart, int xEnd, int y, TileBase tile) {
        for (int i = xStart; i < xEnd; i++) {
            addTile(i, y, tile);
        }
    }

    private void addVerticalLine(int x, int yStart, int yEnd, TileBase tile) {
        for (int i = yStart; i < yEnd; i++) {
            addTile(x, i, tile);
        }
    }

    private bool isTileDrawn(int x, int y) {
        return tileDrawn[getLinearIndex(x, y)];
    }

    private int getLinearIndex(int x, int y) {
        return (y * this.mapSize.x) + x;
    }

    private void swapInt(ref int a, ref int b) {
        int temp = a;
        a = b;
        b = temp;
    }
}