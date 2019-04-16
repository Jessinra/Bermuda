using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour {
    public Player player;

    public float speed;
    private bool touchStart = false;
    private Vector3 pointA;
    private Vector3 pointB;
    public Camera playerCamera;
    public Transform outerCircle;

    private void OnMouseDown() {
        pointA = new Vector3(Input.mousePosition.x, Input.mousePosition.y, outerCircle.transform.position.z);
    }

    private void OnMouseDrag() {
        touchStart = true;
        pointB = new Vector3(Input.mousePosition.x, Input.mousePosition.y, outerCircle.transform.position.z);
    }

    private void OnMouseUp() {
        touchStart = false;
    }

    private void FixedUpdate() {
        if (touchStart) {
            Vector2 offset = pointB - pointA;
            Vector2 direction = Vector2.ClampMagnitude(offset, 1.0f);
            MovePlayer(direction);

            transform.position = new Vector3(
                outerCircle.position.x + direction.x,
                outerCircle.position.y + direction.y,
                outerCircle.transform.position.z);

        } else {
            transform.position = new Vector3(
                outerCircle.position.x,
                outerCircle.position.y,
                outerCircle.transform.position.z);
        }
    }

    void MovePlayer(Vector2 direction) {
        player.gameObject.transform.Translate(direction * speed * Time.deltaTime);
        player.UpdatePosition(player.gameObject.transform.position.x, player.gameObject.transform.position.y);
        if (direction.x > 0) {
            player.SwitchSide("right");
        } else if (direction.x < 0) {
            player.SwitchSide("left");
        }
    }
}