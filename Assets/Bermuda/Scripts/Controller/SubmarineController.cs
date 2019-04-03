using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SubmarineController : MonoBehaviour {
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

    [SerializeField] private GameObject bubbleParticleLeft;
    [SerializeField] private GameObject bubbleParticleRight;

    public float fireRate;
    private float nextFire = 0.0f;

    // Sound 
    private AudioSource[] soundEffects;

    // Start is called before the first frame update
    void Start() {
        // Load Renderer
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        // Initialize variables
        positionFaced = "right";

        soundEffects = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
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
}