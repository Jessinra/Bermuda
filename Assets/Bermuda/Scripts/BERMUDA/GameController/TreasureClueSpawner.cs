using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureClueSpawner : ObjectSpawner {
    [SerializeField] private List<GameObject> treasureClue = new List<GameObject>();

    [SerializeField] private int maxClueSpawnedTogether = 5;
    [SerializeField] private int totalClue = 50;

    private static Queue<int> clueTypeToSpawn = new Queue<int>();
    private static int clueSpawned = 0;
    private int clueIdxToSpawn = 0;
    private ClueLocationData clueLocations = new ClueLocationData();

    // Data container

    void Start() {
        base.Initialize();
        StartCoroutine(runClueSpawner());
    }

    IEnumerator runClueSpawner() {

        while (!mazeBlueprintReady) {
            yield return new WaitForSeconds(3.0F);
        }

        for (int i = 0; i < totalClue; i++) {
            generateClueLocation();
        }

        for (int i = 0; i < maxClueSpawnedTogether; i++) {
            TreasureClueSpawner.addClueTypeToSpawn(0);
        }

        StartCoroutine(runSpawnClues());
    }

    private void generateClueLocation() {
        Vector2 spawnPosition = getClueSpawnPosition();
        clueLocations.location.Add(new Tuple<float, float>(spawnPosition.x, spawnPosition.y));
    }

    private static void addClueTypeToSpawn(int clueType) {
        if (clueType < 0 || clueType >= 3) {
            clueType = 0;
            Debug.Log("clueType outside range");
        }
        TreasureClueSpawner.clueTypeToSpawn.Enqueue(clueType);
    }

    IEnumerator runSpawnClues() {
        while (true) {
            if (!(shouldSpawnClue())) {
                yield return new WaitForSeconds(10.0F);
                continue;
            }
            spawnClue();
        }
    }

    private bool shouldSpawnClue() {
        return mazeBlueprintReady && TreasureClueSpawner.clueSpawned < this.maxClueSpawnedTogether;
    }

    private void spawnClue() {
        Tuple<float, float> spawnPosition = clueLocations.get(this.clueIdxToSpawn);
        int clueType = TreasureClueSpawner.clueTypeToSpawn.Dequeue();
        Instantiate(
            treasureClue[clueType],
            new Vector3(spawnPosition.Item1, spawnPosition.Item2),
            Quaternion.identity);

        this.clueIdxToSpawn++;
        TreasureClueSpawner.clueSpawned++;
    }

    private Vector2 getClueSpawnPosition() {
        Vector2 availablePosition = mazeBlueprint.getRandomTile("Empty");
        return (availablePosition * tileScale) + new Vector2(tileCenterOffset, tileCenterOffset);
    }

    public static void notifyClueCollected(int clueType) {
        TreasureClueSpawner.clueSpawned--;
        TreasureClueSpawner.addClueTypeToSpawn(clueType);
    }
}

public class ClueLocationData {
    public List<Tuple<float, float>> location = new List<Tuple<float, float>>();

    public Tuple<float, float> get(int idx) {
        return location[idx];
    }
}