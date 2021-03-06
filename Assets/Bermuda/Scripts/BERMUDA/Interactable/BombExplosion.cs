﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion : MonoBehaviour {
    
    [SerializeField] private Vector2Int damageRange = new Vector2Int(50, 75);
   
    private void OnTriggerEnter2D(Collider2D other) {
        
        if (other.CompareTag("Player")) {

            Player damagedPlayer = other.gameObject.GetComponent<Player>();
            if (damagedPlayer == null) {
                return;
            }

            int damage = UnityEngine.Random.Range(damageRange.x, damageRange.y);
            damagedPlayer.DecreaseHP(damage);
        }
    }
}