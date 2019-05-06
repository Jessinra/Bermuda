using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour{

    private Player player;

    void Start() {
        this.player = this.gameObject.GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if (other.CompareTag("Tilemap")){
        
            List<Tuple<float, float>> possiblePos = PlayerSpawner.spawnData.location;
            int randomIdx = UnityEngine.Random.Range(0, possiblePos.Count);
            Tuple<float, float> randomPosition = possiblePos[randomIdx];

            this.player.transform.position = new Vector3(randomPosition.Item1, randomPosition.Item2);
        }
    }
}