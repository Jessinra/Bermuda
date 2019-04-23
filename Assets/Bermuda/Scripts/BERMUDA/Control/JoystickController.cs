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
        // Debug.Log("PointA : " + pointA.x.ToString() + ", " + pointA.y.ToString());
    }

    private void OnMouseDrag() {
        touchStart = true;
        pointB = new Vector3(Input.mousePosition.x, Input.mousePosition.y, outerCircle.transform.position.z);
        // Debug.Log("PointB : " + pointB.x.ToString() + ", " + pointB.y.ToString());
    }

    private void OnMouseUp() {
        touchStart = false;
    }

    private void FixedUpdate() {
        if (touchStart) {
            Vector2 offset = pointB - pointA;
            Vector2 direction = Vector2.ClampMagnitude(offset, 0.15f);
            MovePlayer(direction);
            
        } else {
            transform.position = new Vector3(
                outerCircle.position.x,
                outerCircle.position.y,
                outerCircle.transform.position.z);
        }
    }

    void MovePlayer(Vector2 direction) {
        transform.position = new Vector3(
                outerCircle.position.x + direction.x,
                outerCircle.position.y + direction.y,
                outerCircle.transform.position.z);

        player.Move(direction);
    }
}