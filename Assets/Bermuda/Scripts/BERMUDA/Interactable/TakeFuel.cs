using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeFuel : MonoBehaviour {
    [SerializeField] private Sprite fuelTopEmpty = null;
    [SerializeField] private Sprite fuelBottomEmpty = null;
    [SerializeField] private float fuelIncreaseValue = 15.0F;

    private SpriteRenderer spriteRenderer;
    private Sprite originalSprite = null;

    [SerializeField] private Vector2Int respawnTime = new Vector2Int(6, 9);
    private bool fuelReady = true;

    IEnumerator respawnFuel() {
        int delay = UnityEngine.Random.Range(respawnTime.x, respawnTime.y);
        yield return new WaitForSeconds(delay);

        this.fuelReady = true;
        spriteRenderer.sprite = originalSprite;

        yield break;
    }

    void Start() {
        spriteRenderer = this.transform.parent.gameObject.GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if (this.fuelReady && other.CompareTag("Player")) {

            Submarine submarine = other.gameObject.GetComponent<Submarine>();
            if (submarine == null) {
                return;
            }

            if (this.gameObject.CompareTag("Fuel Top")) {
                spriteRenderer.sprite = fuelTopEmpty;

            } else if (this.gameObject.CompareTag("Fuel Bottom")) {
                spriteRenderer.sprite = fuelBottomEmpty;
            }

            submarine.IncreaseFuel(fuelIncreaseValue);
            this.fuelReady = false;
            StartCoroutine(respawnFuel());
        }
    }
}