using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : ObjectSpawner {
    [SerializeField] private List<GameObject> characterType = new List<GameObject>();
    [SerializeField] private int totalPlayerCount = 20;

    public static PlayerSpawnData spawnData = new PlayerSpawnData(); // Data container

    void Start() {
        base.Initialize();
        StartCoroutine(runPlayerSpawner());
    }

    IEnumerator runPlayerSpawner() {

        while (!mazeBlueprintReady) {
            yield return new WaitForSeconds(3.0F);
        }

        for (int i = 0; i < this.totalPlayerCount; i++) {
            generatePlayerSpawnPositions();
            yield return new WaitForSeconds(0.02F);
        }

        // After finish generating location, spawn it
        StartCoroutine(spawnPlayers());
    }

    private void generatePlayerSpawnPositions() {
        Vector2 topSpawnPosition = getPlayerSpawnPosition();
        PlayerSpawner.spawnData.location.Add(new Tuple<float, float>(topSpawnPosition.x, topSpawnPosition.y));

        int playerType = UnityEngine.Random.Range(0, characterType.Count);
        PlayerSpawner.spawnData.type.Add(playerType);
    }

    IEnumerator spawnPlayers() {

        for (int i = 0; i < totalPlayerCount; i++) {

            GameObject player = characterType[PlayerSpawner.spawnData.type[i]];
            Tuple<float, float> position = PlayerSpawner.spawnData.location[i];

            Instantiate(player, new Vector3(position.Item1, position.Item2), Quaternion.identity);
            yield return new WaitForSeconds(0.1F);
        }
        yield break;
    }

    private Vector2 getPlayerSpawnPosition() {
        // TODO : try to generate player outside map

        Vector2 availablePosition = mazeBlueprint.getRandomTile("Empty");
        return (availablePosition * tileScale) + new Vector2(tileCenterOffset, tileCenterOffset);
    }
}

public class PlayerSpawnData {
    public List<Tuple<float, float>> location = new List<Tuple<float, float>>();
    public List<int> type = new List<int>();
}