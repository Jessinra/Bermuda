using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AIMoveState {
    ENGAGE,
    AVOID,
    IDLE,
}

public class AIMove : MonoBehaviour {

    [SerializeField] private Vector2 changeDirectionDelayRange = new Vector2(1F, 5F);

    private Player player;
    private GameObject targetPlayer;

    private AIMoveState state = AIMoveState.IDLE;

    private Vector2 direction;
    private Vector2 offset;
    private Vector2 targetPosition;

    void Start() {
        this.player = this.transform.parent.transform.parent.gameObject.GetComponent<Player>();
        this.direction = new Vector2(0.0F, 0.0F);
        this.offset = new Vector2(0.0F, 0.0F);
        this.targetPosition = (Vector2) this.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            this.targetPlayer = other.gameObject;

            StartCoroutine(CheckSelfCondition());
            StartCoroutine(ExecuteScript());
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            this.targetPlayer = null;
            this.state = AIMoveState.IDLE;

            StopCoroutine(CheckSelfCondition());
            StopCoroutine(ExecuteScript());
        }
    }

    void FixedUpdate() {
        if (this.state != AIMoveState.IDLE) {
            player.Move(direction);
        }
    }

    IEnumerator CheckSelfCondition() {
        while (this.player != null) {
            int engageChance = UnityEngine.Random.Range(0, 100);
            if (engageChance < player.GetHP()){
                this.state = AIMoveState.ENGAGE;
            }else{
                this.state = AIMoveState.AVOID;
            }
            yield return new WaitForSeconds(0.2F);
        }
        yield break;
    }

    IEnumerator ExecuteScript() {

        while (targetPlayer != null) {

            if (this.state == AIMoveState.ENGAGE) {
                this.targetPosition = (Vector2) this.targetPlayer.transform.position;
                this.offset = this.targetPosition - (Vector2) this.transform.position;

            } else if (this.state == AIMoveState.AVOID) {
                this.targetPosition = (Vector2) this.targetPlayer.transform.position;
                this.offset = (Vector2) this.transform.position - this.targetPosition;

            } else if (this.state == AIMoveState.IDLE){
                float randomX = UnityEngine.Random.Range(0.0F, 1.0F);
                float randomY = UnityEngine.Random.Range(0.0F, 1.0F);
                this.targetPosition = new Vector2(randomX, randomY);
                this.offset = this.targetPosition - (Vector2) this.transform.position;
            }

            this.direction = Vector2.ClampMagnitude(offset, 0.15f);
            float changeDirectionDelay = UnityEngine.Random.Range(changeDirectionDelayRange.x, changeDirectionDelayRange.y);
            yield return new WaitForSeconds(changeDirectionDelay);
        }

        if (targetPlayer == null){
            this.state = AIMoveState.IDLE;
            StopCoroutine(CheckSelfCondition());
        }

        yield break;
    }
}