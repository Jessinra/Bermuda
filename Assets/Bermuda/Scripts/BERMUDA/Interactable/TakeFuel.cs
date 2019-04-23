using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeFuel : MonoBehaviour {
    [SerializeField] private Sprite fuelTopEmpty = null;
    [SerializeField] private Sprite fuelBottomEmpty = null;

    private SpriteRenderer spriteRenderer;
    private Sprite originalSprite = null;

    private Submarine submarine = null;

    [SerializeField] private Vector2Int respawnTime = new Vector2Int(6, 9);
    private bool fuelReady = true;

    IEnumerator respawnFuel() {
        int delay = UnityEngine.Random.Range(respawnTime.x, respawnTime.y);
        yield return new WaitForSeconds(delay);

        this.fuelReady = true;
        spriteRenderer.sprite = originalSprite;

        yield break;
    }

    void Start(){
        spriteRenderer = this.transform.parent.gameObject.GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
    }

    private void OnTriggerEnter2D(Collider2D other) {

        Debug.Log("TODO : Fuel action");

        if (this.fuelReady && other.CompareTag("Player")) {

            if (this.gameObject.CompareTag("Fuel Top")) {
                spriteRenderer.sprite = fuelTopEmpty;

            } else if (this.gameObject.CompareTag("Fuel Bottom")) {
                spriteRenderer.sprite = fuelBottomEmpty;
            }

            // Debug.Log("TODO: Increase fuel");
            submarine = other.gameObject.GetComponent<Submarine>();
            submarine.IncreaseFuel();
            this.fuelReady = false;
            StartCoroutine(respawnFuel());
        }
    }
}