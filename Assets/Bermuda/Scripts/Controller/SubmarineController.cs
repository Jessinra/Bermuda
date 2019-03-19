using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SubmarineController : MonoBehaviour
{
    // Sprites
    private SpriteRenderer spriteRenderer;
    private string positionFaced;

    // Buttons
    public Button shootButton;

    // Shots
    public GameObject shotPrefab;
    private GameObject shot;
    public Transform shotSpawnRight;
    public Transform shotSpawnLeft;
    public float fireRate;
    private float nextFire = 0.0f;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        // Load Renderer
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        // Initialize variables
        positionFaced = "right";
    }

    // Update is called once per frame
    void Update()
    {
        if (shootButton.getClickedState() == true)
        {
            nextFire = Time.time + fireRate;
            if (positionFaced == "right")
            {
                shot = (GameObject) Instantiate(shotPrefab, shotSpawnRight.position, shotSpawnRight.rotation);
                shot.GetComponent<Mover>().setDirection("right");
            }
            else if (positionFaced == "left")
            {
                shot = (GameObject) Instantiate(shotPrefab, shotSpawnLeft.position, shotSpawnLeft.rotation);
                shot.GetComponent<Mover>().setDirection("left");
            }

            shootButton.setClickedState(false);
        }

    }

    // Switch submarine's sprited render side according to it's direction
    public void switchSide(string position)
    {
        positionFaced = position;
        if (position == "right")
        {
            animator.SetBool("isFacingRight", true);
            //spriteRenderer.flipX = false;
        }
        else
        {
            animator.SetBool("isFacingRight", false);
            //spriteRenderer.flipX = true;
        }
        
    }
}
