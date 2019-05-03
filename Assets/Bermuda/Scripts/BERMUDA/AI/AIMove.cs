using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AIMoveState {
    ENGAGE,
    AVOID,
    IDLE,
    CATCH,
}

public class AIMove : MonoBehaviour {

    [SerializeField] private Vector2 changeDirectionDelayRange = new Vector2(1F, 5F);

    private Player player;
    private GameObject targetPlayer;
    private GameObject targetObject;

    private AIMoveState state;

 
    private Vector2 lastPosition;
    private Vector2 direction;
    private Vector2 offset;
    private Vector2 targetPosition;


    void Start() {
        this.player = this.transform.parent.transform.parent.gameObject.GetComponent<Player>();
        this.direction = new Vector2(0.0F, 0.0F);
        this.offset = new Vector2(0.0F, 0.0F);
        this.targetPosition = (Vector2) this.transform.position;
        state = AIMoveState.IDLE;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            this.targetPlayer = other.gameObject;
        }
        else if(other.CompareTag("Fuel Bottom") || other.CompareTag("Fuel Top"))
        {
            this.targetObject = other.gameObject;
        }
        StartCoroutine(CheckSelfCondition());
        StartCoroutine(ExecuteScript());
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            this.targetPlayer = null;
        }
        else if(other.CompareTag("Fuel Bottom") || other.CompareTag("Fuel Top"))
        {
            this.targetObject = null;
        }

        this.state = AIMoveState.IDLE;
        StopCoroutine(CheckSelfCondition());
        StopCoroutine(ExecuteScript());
    }

    void FixedUpdate() {
        if (this.state == AIMoveState.IDLE) {
            RandomMove();   
        }
        player.Move(direction);
    }

    IEnumerator CheckSelfCondition() {
        if (this.targetPlayer != null) {
            int engageChance = UnityEngine.Random.Range(0, 100);

            if (player.GetHP() > engageChance){
                this.state = AIMoveState.ENGAGE;
            }else{
                this.state = AIMoveState.AVOID;
            }
            yield return new WaitForSeconds(0.2F);
        }
        else if(this.targetObject != null && this.targetPlayer == null)
        {
            TakeFuel fuel_condition = targetObject.GetComponent<TakeFuel>();
            if (fuel_condition.IsFuelReady())
            {
                this.state = AIMoveState.CATCH;
                yield return new WaitForSeconds(0.2F);
            }
        }

        yield break;
    }

    IEnumerator ExecuteScript() {

        if (targetPlayer != null) {

            if (this.state == AIMoveState.ENGAGE) {
                if (IsStuck())
                {
                    RandomMove();
                }
                else
                {
                    this.targetPosition = (Vector2)this.targetPlayer.transform.position;
                    this.offset = this.targetPosition - (Vector2)this.transform.position;
                }

            } else if (this.state == AIMoveState.AVOID) {

                this.targetPosition = (Vector2)this.targetPlayer.transform.position;
                this.offset = (Vector2)this.transform.position - this.targetPosition;

            } 

            this.lastPosition = new Vector2(player.GetPositionX(), player.GetPositionY());
            this.direction = Vector2.ClampMagnitude(offset, 0.15f);
            float changeDirectionDelay = UnityEngine.Random.Range(changeDirectionDelayRange.x, changeDirectionDelayRange.y);
            yield return new WaitForSeconds(changeDirectionDelay);
        }
        else if(this.targetObject != null && this.targetPlayer == null && this.state == AIMoveState.CATCH)
        {
            this.targetPosition = (Vector2)this.targetObject.transform.position;
            this.offset = this.targetPosition - (Vector2)this.transform.position;
            this.direction = Vector2.ClampMagnitude(offset, 0.15f);
            yield return new WaitForSeconds(0.5f);
        }

        if (targetPlayer == null && targetObject == null){
            this.state = AIMoveState.IDLE;
            StopCoroutine(CheckSelfCondition());
        }

        yield break;
    }

    private void RandomMove()
    {
        float randomX = UnityEngine.Random.Range(0.0F, 1.0F);
        float randomY = UnityEngine.Random.Range(0.0F, 1.0F);
        this.targetPosition = new Vector2(randomX, randomY);
        this.offset = this.targetPosition - (Vector2)this.transform.position;
        this.direction = Vector2.ClampMagnitude(offset, 0.15f);
    }
    
    private bool IsStuck()
    {
        float diff_x = Math.Abs(lastPosition.x - player.transform.position.x);
        float diff_y = Math.Abs(lastPosition.y - player.transform.position.y);
        
        if(diff_x <= 0.02 && diff_y <= 0.02)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}