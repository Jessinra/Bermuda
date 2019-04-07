using System;
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