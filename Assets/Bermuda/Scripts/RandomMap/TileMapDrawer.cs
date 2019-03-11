using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class DrawableTilesContainer {

    public TileBase[] cornerTopLeft = null;
    public TileBase[] cornerTopRight = null;
    public TileBase[] cornerBottomLeft = null;
    public TileBase[] cornerBottomRight = null;
    public TileBase[] edgeTopLeft = null;
    public TileBase[] edgeTopRight = null;
    public TileBase[] edgeBottomLeft = null;
    public TileBase[] edgeBottomRight = null;
    public TileBase[] sideTop = null;
    public TileBase[] sideBottom = null;
    public TileBase[] sideLeft = null;
    public TileBase[] sideRight = null;
    public TileBase[] wallTile = null;

    private System.Random randomGenerator = new System.Random();

    public TileBase getTile(String tileType) {

        if (tileType == "CornerTopLeft") {
            return getRandomTile(cornerTopLeft);

        } else if (tileType == "CornerTopRight") {
            return getRandomTile(cornerTopRight);

        } else if (tileType == "CornerBottomLeft") {
            return getRandomTile(cornerBottomLeft);

        } else if (tileType == "CornerBottomRight") {
            return getRandomTile(cornerBottomRight);

        } else if (tileType == "EdgeTopLeft") {
            return getRandomTile(edgeTopLeft);

        } else if (tileType == "EdgeTopRight") {
            return getRandomTile(edgeTopRight);

        } else if (tileType == "EdgeBottomLeft") {
            return getRandomTile(edgeBottomLeft);

        } else if (tileType == "EdgeBottomRight") {
            return getRandomTile(edgeBottomRight);

        } else if (tileType == "SideTop") {
            return getRandomTile(sideTop);

        } else if (tileType == "SideBottom") {
            return getRandomTile(sideBottom);

        } else if (tileType == "SideLeft") {
            return getRandomTile(sideLeft);

        } else if (tileType == "SideRight") {
            return getRandomTile(sideRight);

        } else if (tileType == "Wall") {
            return getRandomTile(wallTile);
        }
        else{
            Debug.Log("Returning unknown type of tile");
            return null;
        }
    }

    private TileBase getRandomTile(TileBase[] tilePool) {
        int index = randomGenerator.Next(tilePool.Length);
        return tilePool[index];
    }
}

public class TileMapDrawer {
    /* 
    Part of RandomTileMapGenerator, function morely as data cointainer 
    Separated from RandomTileMapGenerator to avoid inheriting MonoBehaviour
    */

    // Static so it only create once, and all child use the same attribute
    private static Vector2Int mapSize;
    private static DrawableTilesContainer drawableTiles;

    private static Vector3Int[] tilePosition;
    private static TileBase[] tileArray;
    private static bool[] tileDrawn;

    /* ========== Getter Setter ========== */
    public void setMapSize(Vector2Int size) {
        mapSize = size;
        tilePosition = new Vector3Int[size.x * size.y];
        tileArray = new TileBase[tilePosition.Length];
        tileDrawn = new bool[tilePosition.Length];
    }

    public void setStaticTiles(ref DrawableTilesContainer drawableTiles) {
        TileMapDrawer.drawableTiles = drawableTiles;
    }

    public Vector3Int[] getTilePosition() {
        return tilePosition;
    }

    public TileBase[] getTileArray() {
        return tileArray;
    }

    /* ========== Main method ========== */

    public void constructMaze(MazeBlueprint blueprint) {
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

    public void constructBigMaze(MazeBlueprint blueprint, int scale) {
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

    /* ========== Drawing ========== */
    private bool isTileDrawn(int x, int y) {
        int index = (y * mapSize.x) + x;
        return tileDrawn[index];
    }

    protected void addBlockTile(int xStart, int yStart, int xEnd, int yEnd, TileBase tile) {

        if (yStart > yEnd) {
            swapTiles(ref yStart, ref yEnd);
        }

        if (xStart > xEnd) {
            swapTiles(ref xStart, ref xEnd);
        }

        for (int i = yStart; i < yEnd; i++) {
            addHorizontalLine(xStart, xEnd, i, tile);
        }
    }

    protected void addHorizontalLine(int xStart, int xEnd, int y, TileBase tile) {
        for (int i = xStart; i < xEnd; i++) {
            addTile(i, y, tile);
        }
    }

    protected void addVerticalLine(int x, int yStart, int yEnd, TileBase tile) {
        for (int i = yStart; i < yEnd; i++) {
            addTile(x, i, tile);
        }
    }

    protected void addTile(int x, int y, TileBase tile) {
        int index = (y * mapSize.x) + x;
        tilePosition[index] = new Vector3Int(x, y, 0);
        tileArray[index] = tile;
        tileDrawn[index] = true;
    }

    private void swapTiles(ref int a, ref int b) {
        int temp = a;
        a = b;
        b = temp;
    }

    /* ========== Virtual Method ========== */
    public virtual void generatePattern() { }

}