using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour {

    // Attributes
    [SerializeField] [HideInInspector] protected string id;
    [SerializeField] [HideInInspector] protected string username;
    [SerializeField] [HideInInspector] protected string status;
    [SerializeField] [HideInInspector] protected float position_x;
    [SerializeField] [HideInInspector] protected float position_y;
    [SerializeField] [HideInInspector] protected string type;
    protected int health;

    // Sprites
    protected SpriteRenderer spriteRenderer;
    protected string positionFaced;

    // Buttons
    public Button shootButton = null;
    public Button actionButton = null;

    // Shots
    public GameObject shotPrefab;
    protected GameObject shot;

    public Transform shotSpawnRight;
    public Transform shotSpawnLeft;

    public float fireRate;
    protected float nextFire;

    // Sound 
    protected AudioSource[] soundEffects;

    // Start is called before the first frame update
    protected void Start() {
        // Load Renderer
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        // Load Buttons
        //shootButton = GameObject.Find("ShootButton").GetComponent<Button>();

        // Load Shot Spawn
        //shotSpawnRight = GameObject.Find("ShotSpawnRight").GetComponent<Transform>();
        //shotSpawnLeft = GameObject.Find("ShotSpawnLeft").GetComponent<Transform>();
        
        // Initialize variables
        position_x = transform.position.x;
        position_y = transform.position.y;
        positionFaced = "Right";
        health = 100;
        nextFire = 0.0f;
        status = "alive";
        type = "submarine";
        id = "dummy";
        username = "dummy";
        fireRate = 0.25f;
        soundEffects = this.GetComponents<AudioSource>();
    }

    // Update is called once per frame
    protected void Update() {

        if (!shootButton) {
            return;
        }

        if (shootButton.getClickedState() == true) {
            nextFire = Time.time + fireRate;
            if (positionFaced == "right") {
                soundEffects[1].Play();
                shot = (GameObject) Instantiate(shotPrefab, shotSpawnRight.position, shotSpawnRight.rotation);
                shot.GetComponent<Bolt>().SetUsername(username);
                shot.GetComponent<Bolt>().SetType(1);
                shot.GetComponent<Bolt>().SetId();
                shot.GetComponent<Mover>().setDirection("right");
                shot.GetComponent<Mover>().setSpeed(0.5f);

            } else if (positionFaced == "left") {
                soundEffects[1].Play();
                shot = (GameObject) Instantiate(shotPrefab, shotSpawnLeft.position, shotSpawnLeft.rotation);
                shot.GetComponent<Bolt>().SetUsername(username);
                shot.GetComponent<Bolt>().SetType(1);
                shot.GetComponent<Bolt>().SetId();
                shot.GetComponent<Mover>().setDirection("left");
                shot.GetComponent<Mover>().setSpeed(0.5f);
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

    public void increaseHP(int delta) {
        health += delta;
    }

    public void decreaseHP(int delta) {
        health -= delta;
    }

    public void SetType(string value)
    {
        type = value;
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

    public string GetPlayerType()
    {
        return type;
    }

    public string GetId()
    {
        return id;
    }

    public void SetId(string id)
    {
        this.id = id;
    }

    public string GetUsername()
    {
        return username;
    }

    public void SetUsername(string username)
    {
        this.username = username;
    }
}