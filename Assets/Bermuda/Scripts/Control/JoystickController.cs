﻿using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour {
    public Transform player;
    public SubmarineController submarineController;
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
            moveCharacter(direction);

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

    void moveCharacter(Vector2 direction) {
        player.transform.Translate(direction * speed * Time.deltaTime);
        if (direction.x > 0) {
            submarineController.switchSide("right");
        } else if (direction.x < 0) {
            submarineController.switchSide("left");
        }
    }

}