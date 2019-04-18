using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelSpawner : ObjectSpawner {
    [SerializeField] private GameObject fuelTop = null;
    [SerializeField] private GameObject fuelBottom = null;

    [SerializeField] private int fuelCount = 100;
    private FuelLocationData fuelLocations = new FuelLocationData(); // Data container

    void Start() {
        base.Initialize();
        StartCoroutine(runFuelSpawner());
    }

    IEnumerator runFuelSpawner() {

        while (!mazeBlueprintReady) {
            yield return new WaitForSeconds(3.0F);
        }

        for (int i = 0; i < this.fuelCount; i += 2) {
            generateFuelTopSpawnPositions();
            generateFuelBottomSpawnPositions();
            yield return new WaitForSeconds(0.02F);
        }

        // After finish generating position, spawn it
        StartCoroutine(spawnFuels());
    }

    private void generateFuelTopSpawnPositions() {
        Vector2 topSpawnPosition = getFuelTopSpawnPosition();
        fuelLocations.typeTop.Add(new Tuple<float, float>(topSpawnPosition.x, topSpawnPosition.y));
    }

    private void generateFuelBottomSpawnPositions() {
        Vector2 bottomSpawnPosition = getFuelBottomSpawnPosition();
        fuelLocations.typeBottom.Add(new Tuple<float, float>(bottomSpawnPosition.x, bottomSpawnPosition.y));
    }

    IEnumerator spawnFuels() {

        foreach (Tuple<float, float> position in fuelLocations.typeTop) {
            Instantiate(fuelTop, new Vector3(position.Item1, position.Item2), Quaternion.identity);
            yield return new WaitForSeconds(0.1F);
        }

        foreach (Tuple<float, float> position in fuelLocations.typeBottom) {
            Instantiate(fuelBottom, new Vector3(position.Item1, position.Item2), Quaternion.identity);
            yield return new WaitForSeconds(0.1F);
        }
        yield break;
    }

    private Vector2 getFuelTopSpawnPosition() {
        Vector2 availablePosition = mazeBlueprint.getRandomTile("SideBottom");
        float randomYOffset = UnityEngine.Random.Range(0.2F, 1.2F) * tileCenterOffset;
        return (availablePosition * tileScale) + new Vector2(tileCenterOffset, randomYOffset);
    }

    private Vector2 getFuelBottomSpawnPosition() {
        Vector2 availablePosition = mazeBlueprint.getRandomTile("SideTop");
        float randomYOffset = UnityEngine.Random.Range(0.8F, 1.8F) * tileCenterOffset;
        return (availablePosition * tileScale) + new Vector2(tileCenterOffset, randomYOffset);
    }
}

public class FuelLocationData {
    public List<Tuple<float, float>> typeTop = new List<Tuple<float, float>>();
    public List<Tuple<float, float>> typeBottom = new List<Tuple<float, float>>();
}