using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelSpawner : ObjectSpawner {
    [SerializeField] private GameObject fuelTop;
    [SerializeField] private GameObject fuelBottom;

    [SerializeField] private int fuelCount = 100;
    private FuelLocationData fuelLocationData = new FuelLocationData();  // Data container

    void Start() {
        base.Initialize();
        StartCoroutine(runFuelSpawner());
    }

    IEnumerator runFuelSpawner() {

        while (!mazeBlueprintReady) {
            yield return new WaitForSeconds(3.0F);
        }

        while (this.fuelCount > 0) {

            Debug.Log(fuelCount);

            generateFuelTopSpawnPositions();
            generateFuelBottomSpawnPositions();
            yield return new WaitForSeconds(0.02F);
        }

        // After finish generating position, spawn it
        StartCoroutine(spawnFuels());
    }

    private void generateFuelTopSpawnPositions() {
        Vector2 topSpawnPosition = getFuelTopSpawnPosition();
        fuelLocationData.typeTop.Add(new Tuple<float, float>(topSpawnPosition.x, topSpawnPosition.y));
        fuelCount--;
    }

    private void generateFuelBottomSpawnPositions() {
        Vector2 bottomSpawnPosition = getFuelBottomSpawnPosition();
        fuelLocationData.typeBottom.Add(new Tuple<float, float>(bottomSpawnPosition.x, bottomSpawnPosition.y));
        fuelCount--;
    }

    IEnumerator spawnFuels() {

        foreach (Tuple<float, float> position in fuelLocationData.typeTop) {
            Instantiate(fuelTop, new Vector3(position.Item1, position.Item2), Quaternion.identity);
            yield return new WaitForSeconds(0.1F);
        }

        foreach (Tuple<float, float> position in fuelLocationData.typeBottom) {
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

public class FuelLocationData{
    public List<Tuple<float, float>> typeTop = new List<Tuple<float, float>>();
    public List<Tuple<float, float>> typeBottom = new List<Tuple<float, float>>();
}