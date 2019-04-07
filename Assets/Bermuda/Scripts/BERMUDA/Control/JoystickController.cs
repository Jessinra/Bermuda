using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour {
    public Submarine submarine;
    public float speed;
    private bool touchStart = false;
    private Vector3 pointA;
    private Vector3 pointB;
    public Camera playerCamera;
    public Transform outerCircle;

    // Update is called once per frame
    void Update() {

    }

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
            MoveCharacter(direction);

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

    void MoveCharacter(Vector2 direction) {
        submarine.gameObject.transform.Translate(direction * speed * Time.deltaTime);
        if (direction.x > 0) {
            submarine.SwitchSide("right");
        } else if (direction.x < 0) {
            submarine.SwitchSide("left");
        }
    }

}