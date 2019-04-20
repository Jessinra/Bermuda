using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    [SerializeField] private Vector2Int damageRange = new Vector2Int(50, 75);
    private Player damagedPlayerScript = null;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {

            if (!damagedPlayerScript) {
                // Finding the player script
                damagedPlayerScript = other.gameObject.GetComponent<Player>();
            }

            // If the script is available, decrease the health
            if (damagedPlayerScript) {

                int damage = UnityEngine.Random.Range(damageRange.x, damageRange.y);
                damagedPlayerScript.decreaseHP(damage);

                Debug.Log("TODO : notify server");
            }
        }
    }
}
