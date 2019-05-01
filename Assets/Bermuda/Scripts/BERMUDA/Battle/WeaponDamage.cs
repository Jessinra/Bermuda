using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour {

    [SerializeField] private Vector2Int damageRange = new Vector2Int(15, 25);

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.tag);

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