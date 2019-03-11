using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;

public class JoystickController : MonoBehaviour
{ 
    public Transform player;
    public SubmarineController submarineController;
    public float speed;
    private bool touchStart = false;
    private Vector2 pointA;
    private Vector2 pointB;
    public Camera playerCamera;
    public Transform outerCircle;

    // Update is called once per frame
    void Update()
    {  
     
    }

    private void OnMouseDown()
    {
        pointA = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    private void OnMouseDrag()
    {
        touchStart = true;
        pointB = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    private void OnMouseUp()
    {
        touchStart = false;
    }

    private void FixedUpdate()
    {
        if (touchStart)
        {
            Vector2 offset = pointB - pointA;
            Vector2 direction = Vector2.ClampMagnitude(offset, 1.0f);
            moveCharacter(direction);

            transform.position = new Vector2(outerCircle.position.x + direction.x, outerCircle.position.y + direction.y);
        }
        else
        {
            transform.position = new Vector2(outerCircle.position.x, outerCircle.position.y);
        }
    }

    void moveCharacter(Vector2 direction)
    {
        player.transform.Translate(direction * speed * Time.deltaTime);
        if(direction.x > 0)
        {
            submarineController.switchSide("right");
        }
        else if (direction.x < 0)
        {
            submarineController.switchSide("left");
        }
    }

}
