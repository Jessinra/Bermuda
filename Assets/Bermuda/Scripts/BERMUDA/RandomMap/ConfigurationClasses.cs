using UnityEngine;

/* =================================================
            Configuration Holder Class
================================================= */

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