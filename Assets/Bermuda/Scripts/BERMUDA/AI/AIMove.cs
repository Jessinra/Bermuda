using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AIMoveState
{
    CHASING,
    IDLE,
}

public class AIMove : MonoBehaviour {

    [SerializeField] private Vector2 changeDirectionDelayRange = new Vector2(1F, 6F);

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
            this.state = AIMoveState.CHASING;
            StartCoroutine(TrackTarget());
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Player")) {
            this.targetPlayer = null;
            this.state = AIMoveState.IDLE;
            StopCoroutine(TrackTarget());
        }
    }

    void FixedUpdate() {
        if(this.state == AIMoveState.CHASING){
            player.Move(direction);
        }
    }

    IEnumerator TrackTarget() {
        while(this.targetPlayer != null){
            
            this.targetPosition = (Vector2) this.targetPlayer.transform.position;
            this.offset = this.targetPosition - (Vector2) this.transform.position;
            this.direction = Vector2.ClampMagnitude(offset, 0.15f);

            float changeDirectionDelay = UnityEngine.Random.Range(changeDirectionDelayRange.x, changeDirectionDelayRange.y);
            yield return new WaitForSeconds(changeDirectionDelay);
        }
        yield break;
    }
}