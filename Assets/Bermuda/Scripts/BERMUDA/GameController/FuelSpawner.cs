using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelSpawner : ObjectSpawner {
    [SerializeField] private GameObject fuelTop;
    [SerializeField] private GameObject fuelBottom;

    [SerializeField] private int fuelCount = 100;

    void Start() {
        base.Initialize();
        StartCoroutine(runFuelSpawner());
    }

    IEnumerator runFuelSpawner() {

        while (true) {

            if (!mazeBlueprintReady) {
                yield return new WaitForSeconds(5.0F);
                continue;
            }

            if (this.fuelCount < 0) {
                break;
            }

            spawnFuel();
            yield return new WaitForSeconds(0.2F);
        }
    }

    private void spawnFuel() {

        Vector2Int topSpawnPosition = getFuelTopSpawnPosition();
        Vector2Int bottomSpawnPosition = getFuelBottomSpawnPosition();

        Instantiate(fuelTop,
            new Vector3(topSpawnPosition.x, topSpawnPosition.y),
            Quaternion.identity);

        Instantiate(fuelBottom,
            new Vector3(bottomSpawnPosition.x, bottomSpawnPosition.y),
            Quaternion.identity);

        Debug.Log("FuelSpawner : spawned");
        fuelCount -= 2;
    }

    private Vector2Int getFuelTopSpawnPosition() {

        List<String> allowedTileTypes = new List<String>() {
            "SideTop",
            "EdgeTopLeft",
            "EdgeTopRight",
            "CornerTopLeft",
            "CornerTopRight",
        };

        Vector2Int availablePosition = mazeBlueprint.getRandomTile(allowedTileTypes);
        return (availablePosition * tileScale) + new Vector2Int(tileCenterOffset, tileCenterOffset);
    }

    private Vector2Int getFuelBottomSpawnPosition() {

        List<String> allowedTileTypes = new List<String>() {
            "SideBottom",
            "EdgeBottomLeft",
            "EdgeBottomRight",
            "CornerBottomLeft",
            "CornerBottomRight",
        };

        Vector2Int availablePosition = mazeBlueprint.getRandomTile(allowedTileTypes);
        return (availablePosition * tileScale) + new Vector2Int(tileCenterOffset, tileCenterOffset);
    }
}