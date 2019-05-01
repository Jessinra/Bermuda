using System;
using System.Collections;
using UnityEngine;

enum AIBattleState {
    ENGAGE,
    IDLE,
}

public class AIBattle : MonoBehaviour {

    [SerializeField] private Vector2 aimDelay = new Vector2(0.1F, 0.3F);
    [SerializeField] private AICooldownConfig cooldown = null;

    private Player player;
    private GameObject targetPlayer;

    private AIBattleState state = AIBattleState.IDLE;

    private Vector2 direction;
    private Vector2 offset;
    private Vector2 targetPosition;

    void Start() {
        this.player = this.transform.parent.transform.parent.gameObject.GetComponent<Player>();
        this.targetPosition = (Vector2) this.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            this.targetPlayer = other.gameObject;
            this.state = AIBattleState.ENGAGE;
            StartCoroutine(EngageTarget());
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            this.targetPlayer = null;
            this.state = AIBattleState.IDLE;
            StopCoroutine(EngageTarget());
        }
    }

    IEnumerator EngageTarget() {
        while (this.targetPlayer != null) {

            this.targetPosition = (Vector2) this.targetPlayer.transform.position;
            this.offset = this.targetPosition - (Vector2) this.transform.position;
            this.direction = Vector2.ClampMagnitude(offset, 0.15f);

            if (Math.Abs(this.direction.y) < 0.2F) {
                player.CreateDefaultShot();
                yield return new WaitForSeconds(cooldown.shot);
                continue;
            }

            float reAimDelay = UnityEngine.Random.Range(aimDelay.x, aimDelay.y);
            yield return new WaitForSeconds(reAimDelay);
        }
        yield break;
    }
}

[System.Serializable]
public class AICooldownConfig{
    public float shot = 0.25F;
    public float skill = 3.0F;
    public float shield = 8.0F;
    public float boost = 8.0F;
}
