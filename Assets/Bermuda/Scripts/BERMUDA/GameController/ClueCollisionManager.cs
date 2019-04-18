using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueCollisionManager : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("collide");
        if (other.CompareTag("Player")) {

            if (this.gameObject.CompareTag("Clue")) {
                TreasureClueSpawner.notifyClueCollected(1);

            } else if (this.gameObject.CompareTag("Clue 02")) {
                TreasureClueSpawner.notifyClueCollected(2);

            } else if (this.gameObject.CompareTag("Clue 03")) {
                ChestSpawner.notifyLastClueCollected();
            }

            Destroy(this.gameObject);
        }
    }
}