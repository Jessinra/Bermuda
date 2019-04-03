using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureClueSpawner : ObjectSpawner {
    [SerializeField] private GameObject treasureClue;
    [SerializeField] private int maxClueSpawnedTogether = 0;
    private static int clueSpawned = 0;

    void Start() {
        base.Initialize();
        StartCoroutine(runClueSpawner());
    }

    IEnumerator runClueSpawner() {

        while (true) {

            if (!mazeBlueprintReady || !(TreasureClueSpawner.clueSpawned < this.maxClueSpawnedTogether)) {
                yield return new WaitForSeconds(5.0F);
                continue;
            }
            
            spawnClue();
            yield return new WaitForSeconds(0.5F);
        }
    }

    private void spawnClue() {

        Vector2Int spawnPosition = getClueSpawnPosition();

        Instantiate(treasureClue,
            new Vector3(spawnPosition.x, spawnPosition.y),
            Quaternion.identity);

        Debug.Log("TreasureClueSpawner : spawned");
        TreasureClueSpawner.clueSpawned++;
    }

    private Vector2Int getClueSpawnPosition() {
        Vector2Int availablePosition = mazeBlueprint.getRandomTile("Empty");
        return (availablePosition * tileScale) + new Vector2Int(tileCenterOffset, tileCenterOffset);
    }

    public static void notifyClueCollected() {
        TreasureClueSpawner.clueSpawned--;
    }
}