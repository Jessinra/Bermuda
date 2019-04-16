using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour {
    // Attributes
    protected int health;
    
    // Sprites
    protected SpriteRenderer spriteRenderer;
    protected string positionFaced;

    // Buttons
    [SerializeField] protected Button shootButton = null;
    [SerializeField] protected Button actionButton = null;

    // Shots
    [SerializeField] protected GameObject shotPrefab;
    protected GameObject shot;

    [SerializeField] protected Transform shotSpawnRight;
    [SerializeField] protected Transform shotSpawnLeft; 

    [SerializeField] protected float fireRate;
    protected float nextFire;

    // Sound 
    protected AudioSource[] soundEffects;

    // Start is called before the first frame update
    protected void Start() {
        // Load Renderer
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        // Initialize variables
        positionFaced = "left";
        health = 100;
        nextFire = 0.0f;

        soundEffects = this.GetComponents<AudioSource>();
    }

    // Update is called once per frame
    protected void Update() {

        if(!shootButton){
            return;
        }

        if (shootButton.getClickedState() == true) {
            nextFire = Time.time + fireRate;
            if (positionFaced == "right") {
                soundEffects[1].Play();
                shot = (GameObject) Instantiate(shotPrefab, shotSpawnRight.position, shotSpawnRight.rotation);
                shot.GetComponent<Mover>().setDirection("right");

            } else if (positionFaced == "left") {
                soundEffects[1].Play();
                shot = (GameObject) Instantiate(shotPrefab, shotSpawnLeft.position, shotSpawnLeft.rotation);
                shot.GetComponent<Mover>().setDirection("left");
            }

            shootButton.setClickedState(false);
        }

    }

    // Switch submarine's sprited render side according to it's direction
    public virtual void SwitchSide(string position) {
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

    public void decreaseHP(int delta)
    {
        health -= delta;
    }
}