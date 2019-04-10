using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {
    public float speed = 1;

    private string direction = "null";
    protected Rigidbody2D rigidBody = null;
    protected SpriteRenderer spriteRenderer = null;

    // Start is called before the first frame update
    protected void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody.velocity = transform.forward * speed;
    }

    protected void FixedUpdate() {
        if (direction == "null"){
            return;
        }

        if (direction == "left") {
            rigidBody.transform.Translate(new Vector2(-speed, 0.0f));
        } else if (direction == "right") {
            rigidBody.transform.Translate(new Vector2(speed, 0.0f));
        } else if (direction == "up") {
            rigidBody.transform.Translate(new Vector2(0.0f, speed));
        } else if (direction == "down") {
            rigidBody.transform.Translate(new Vector2(0.0f, -speed));
        }
    }

    public void setDirection(string dir) {
        direction = dir;
    }

    public string getDirection() {
        return direction;
    }
}