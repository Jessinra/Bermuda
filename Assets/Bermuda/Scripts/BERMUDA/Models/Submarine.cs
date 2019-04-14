using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Submarine : MonoBehaviour {
    
    // Include class List of Bolt
    List<Bolt> boltFired;

    // Attributes
    private int type;
    private float position_x;
    private float position_y;


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

    [SerializeField] private GameObject bubbleParticleLeft;
    [SerializeField] private GameObject bubbleParticleRight;

    public float fireRate;
    private float nextFire;

    // Sound 
    private AudioSource[] soundEffects;

    // Start is called before the first frame update
    void Start() {
        // Load Renderer
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        // Initialize variables
        positionFaced = "right";
        type = 3;
        nextFire = 0.0f;
        position_x = transform.position.x;
        position_y = transform.position.y;

        boltFired = new List<Bolt>();
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
    public void SwitchSide(string position) {
        positionFaced = position;
        if (position == "right") {
            spriteRenderer.flipX = false;
            this.bubbleParticleLeft.SetActive(true);
            this.bubbleParticleRight.SetActive(false);
        } else {
            spriteRenderer.flipX = true;
            this.bubbleParticleLeft.SetActive(false);
            this.bubbleParticleRight.SetActive(true);
        }

    }

    public AudioSource GetEngineSound() {
        return soundEffects[0];
    }

    public void SetType(int value)
    {
        type = value;
    }

    public int GetSubmarineType()
    {
        return type;
    }

    public void UpdatePosition(float new_pos_x, float new_pos_y)
    {
        position_x = new_pos_x;
        position_y = new_pos_y;
    }

    public float GetPositionX()
    {
        return position_x;
    }

    public float GetPositionY()
    {
        return position_y;
    }
}