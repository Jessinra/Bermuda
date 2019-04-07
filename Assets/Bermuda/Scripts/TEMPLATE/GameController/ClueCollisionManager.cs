using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueCollisionManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {   
        Debug.Log("collide");
        if(other.CompareTag("Player"))
        {
            TreasureClueSpawner.notifyClueCollected();
            Destroy(this.gameObject);
        }
    }
}