using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeFuel : MonoBehaviour {
    [SerializeField] private Sprite fuelTopEmpty = null;
    [SerializeField] private Sprite fuelBottomEmpty = null;

    private void OnTriggerEnter2D(Collider2D other) {

        Debug.Log("TODO : Fuel action");

        if (other.CompareTag("Player")) {

            SpriteRenderer renderer = this.transform.parent.gameObject.GetComponent<SpriteRenderer>();

            if (this.gameObject.CompareTag("Fuel Top")) {
                renderer.sprite = fuelTopEmpty;

            } else if (this.gameObject.CompareTag("Fuel Bottom")) {
                renderer.sprite = fuelBottomEmpty;
            }

            Debug.Log("TODO: Increase fuel");
            // Destroy(this.transform.parent.gameObject);
        }
    }
}