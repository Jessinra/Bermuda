using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpawner : ObjectSpawner {
    [SerializeField] private GameObject BombTop = null;
    [SerializeField] private GameObject BombBottom = null;

    [SerializeField] private int BombCount = 100;
    private BombLocationData BombLocations = new BombLocationData(); // Data container

    void Start() {
        base.Initialize();
        StartCoroutine(runBombSpawner());
    }

    IEnumerator runBombSpawner() {

        while (!mazeBlueprintReady) {
            yield return new WaitForSeconds(3.0F);
        }

        for (int i = 0; i < this.BombCount; i += 2) {
            generateBombTopSpawnPositions();
            generateBombBottomSpawnPositions();
            yield return new WaitForSeconds(0.02F);
        }

        // After finish generating position, spawn it
        StartCoroutine(spawnBombs());
    }

    private void generateBombTopSpawnPositions() {
        Vector2 topSpawnPosition = getBombTopSpawnPosition();
        BombLocations.typeTop.Add(new Tuple<float, float>(topSpawnPosition.x, topSpawnPosition.y));
    }

    private void generateBombBottomSpawnPositions() {
        Vector2 bottomSpawnPosition = getBombBottomSpawnPosition();
        BombLocations.typeBottom.Add(new Tuple<float, float>(bottomSpawnPosition.x, bottomSpawnPosition.y));
    }

    IEnumerator spawnBombs() {

        foreach (Tuple<float, float> position in BombLocations.typeTop) {
            Instantiate(BombTop, new Vector3(position.Item1, position.Item2), Quaternion.identity);
            yield return new WaitForSeconds(0.1F);
        }

        foreach (Tuple<float, float> position in BombLocations.typeBottom) {
            Instantiate(BombBottom, new Vector3(position.Item1, position.Item2), Quaternion.identity);
            yield return new WaitForSeconds(0.1F);
        }
        yield break;
    }

    private Vector2 getBombTopSpawnPosition() {
        Vector2 availablePosition = mazeBlueprint.getRandomTile("SideBottom");
        float randomYOffset = UnityEngine.Random.Range(0.5F, 0.7F) * tileCenterOffset;
        return (availablePosition * tileScale) + new Vector2(tileCenterOffset, randomYOffset);
    }

    private Vector2 getBombBottomSpawnPosition() {
        Vector2 availablePosition = mazeBlueprint.getRandomTile("SideTop");
        float randomYOffset = UnityEngine.Random.Range(1.3F, 1.5F) * tileCenterOffset;
        return (availablePosition * tileScale) + new Vector2(tileCenterOffset, randomYOffset);
    }
}

public class BombLocationData {
    public List<Tuple<float, float>> typeTop = new List<Tuple<float, float>>();
    public List<Tuple<float, float>> typeBottom = new List<Tuple<float, float>>();
}