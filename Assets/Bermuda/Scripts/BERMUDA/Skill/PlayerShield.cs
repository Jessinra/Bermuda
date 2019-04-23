using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bolt")) {
            Destroy(this.gameObject);
            Debug.Log("TODO : notify server bolt destroyed");
        }
    }
}
