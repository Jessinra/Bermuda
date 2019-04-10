using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMover : Mover {

    [SerializeField] private Vector2 changeDirectionDelayRange = new Vector2(1F, 6F);

    // Start is called before the first frame update
    void Start() {
        base.Start();
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody.velocity = transform.forward * speed;
        StartCoroutine(randomMove());
    }

    IEnumerator randomMove(){
        while(true){

            string currentDirection = getRandomDirection();
            setDirection(currentDirection);

            float changeDirectionDelay = UnityEngine.Random.Range(changeDirectionDelayRange.x, changeDirectionDelayRange.y);
            yield return new WaitForSeconds(changeDirectionDelay);
        }
    }

    string getRandomDirection(){
        List<string> directions = new List<string>(){
            "up", "down", "left", "right"
        };
        return directions[UnityEngine.Random.Range(0,4)];
    }

    void FixedUpdate() {
        base.FixedUpdate();
    }
}