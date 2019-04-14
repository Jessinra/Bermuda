using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Diver : MonoBehaviour {
    // Attributes
    private int health;
    private int sub_type;
    
    // Sprites
    private SpriteRenderer spriteRenderer;
    private string positionFaced;

    // Buttons
    public Button shootButton = null;
    public Button actionButton = null;

    // Shots
    public GameObject shotPrefab;
    private GameObject shot;
    public Transform shotSpawnRight;
    public Transform shotSpawnLeft; 
    public float fireRate;
    private float nextFire;

    // Sound 
    private AudioSource[] soundEffects;

    // Start is called before the first frame update
    void Start() {
        // Load Renderer
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        // Initialize variables
        positionFaced = "left";
        health = 100;
        sub_type = 1;
        nextFire = 0.0f;

        soundEffects = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update() {

        if(!shootButton){
            return;
        }

        if (shootButton.getClickedState() == true) {
            nextFire = Time.time + fireRate;
            if (positionFaced == "right") {
                soundEffects[0].Play();
                shot = (GameObject) Instantiate(shotPrefab, shotSpawnRight.position, shotSpawnRight.rotation);
                shot.GetComponent<Mover>().setDirection("right");

            } else if (positionFaced == "left") {
                soundEffects[0].Play();
                shot = (GameObject) Instantiate(shotPrefab, shotSpawnLeft.position, shotSpawnLeft.rotation);
                shot.GetComponent<Mover>().setDirection("left");
            }

            shootButton.setClickedState(false);
        }

    }

    // Switch submarine's sprited render side according to it's direction
    public void SwitchSide(string position) {
        positionFaced = position;
        if (position == "left") {
            spriteRenderer.flipX = false;
        } else {
            spriteRenderer.flipX = true;
        }

    }

    public void increaseHP(int delta)
    {
        health += delta;
    }

    public void DecreaseHP(int delta)
    {
        health -= delta;
    }
}