using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawner : ObjectSpawner {
    [SerializeField] private List<GameObject> TreasureChest = new List<GameObject>();
    private ChestLocationData ChestLocations = new ChestLocationData(); // Data container

    [SerializeField] private int ChestCount = 3;
    [SerializeField] private static int ChestSpawned = 0;
    [SerializeField] private static bool ShouldSpawnChest = false;

    void Start() {
        base.Initialize();
        StartCoroutine(runChestSpawner());
    }

    IEnumerator runChestSpawner() {

        while (!mazeBlueprintReady) {
            yield return new WaitForSeconds(3.0F);
        }

        for (int i = 0; i < this.ChestCount; i++) {
            generateTreasureChestSpawnPositions();
            yield return new WaitForSeconds(0.02F);
        }

        StartCoroutine(spawnChest());
        yield break;
    }

    private void generateTreasureChestSpawnPositions() {
        Vector2 spawnPosition = getTreasureChestSpawnPosition();
        ChestLocations.location.Add(new Tuple<float, float>(spawnPosition.x, spawnPosition.y));
    }

    IEnumerator spawnChest() {

        while (ChestSpawner.ChestSpawned < ChestCount) {

            if (!(ChestSpawner.ShouldSpawnChest)) {
                yield return new WaitForSeconds(10F);
                continue;
            }

            Tuple<float, float> position = ChestLocations.get(ChestSpawner.ChestSpawned);
            Instantiate(
                TreasureChest[ChestSpawner.ChestSpawned],
                new Vector3(position.Item1, position.Item2),
                Quaternion.identity);

            ShouldSpawnChest = false;
            notifyChestSpawned();

        }

        yield break;
    }

    private Vector2 getTreasureChestSpawnPosition() {
        Vector2 availablePosition = mazeBlueprint.getRandomTile("SideTop");
        float randomYOffset = UnityEngine.Random.Range(1.0F, 1.1F) * tileCenterOffset;
        return (availablePosition * tileScale) + new Vector2(tileCenterOffset, randomYOffset);
    }

    public static void notifyLastClueCollected() {
        // On last clue collected :
        ChestSpawner.ShouldSpawnChest = true;
    }

    // Should be called whenever player get last clue
    public static void notifyChestSpawned() {
        ChestSpawner.ChestSpawned++;
    }
}

public class ChestLocationData {
    public List<Tuple<float, float>> location = new List<Tuple<float, float>>();

    public Tuple<float, float> get(int idx) {
        return location[idx];
    }
}