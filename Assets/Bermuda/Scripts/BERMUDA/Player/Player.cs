using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour {

    // Attributes
    [SerializeField][HideInInspector] protected string id;
    [SerializeField][HideInInspector] protected string username;
    [SerializeField][HideInInspector] protected string status;
    [SerializeField][HideInInspector] protected float position_x;
    [SerializeField][HideInInspector] protected float position_y;
    [SerializeField][HideInInspector] protected string type;
    protected int health;
    protected float speed = 70;

    // Sprites
    protected SpriteRenderer spriteRenderer;
    protected string positionFaced;

    // Buttons
    public Button shootButton = null;
    public Button skillButton = null;
    public Button shieldButton = null;
    public Button boostButton = null;

    // Shots
    public GameObject shotPrefab;
    protected GameObject shot;
    private List<Bolt> boltsFired;

    public Transform shotSpawnRight;
    public Transform shotSpawnLeft;

    public float fireRate;
    protected float nextFire;

    // Boosting
    [SerializeField] private float boostMultiplier = 2F;
    [SerializeField] private float boostDuration = 3F;

    // Shield
    private bool shieldActive = false;
    [SerializeField] private float shieldDuration = 3F;
    [SerializeField] private GameObject shield = null;

    // Special skill
    [SerializeField] private GameObject specialShotPrefab = null;
    protected GameObject specialShot;
    private List<Bolt> specialShotFired;

    // Sound 
    protected AudioSource[] soundEffects;

    // Explosion
    [SerializeField] protected GameObject explosion = null;

    // Start is called before the first frame update
    protected void Start() {
        // Load Renderer
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

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

        // Initialize list of bolts fired
        boltsFired = new List<Bolt>();

        StartCoroutine(CheckForDeath());
        StartCoroutine(CheckForShot());
        StartCoroutine(CheckForBoost());
        StartCoroutine(checkForShield());
        StartCoroutine(checkForSkill());
    }

    IEnumerator CheckForShot() {

        while (shootButton) {
            if (shootButton.IsClicked()) {

                if (positionFaced == "right") {
                    shot = (GameObject) Instantiate(shotPrefab, shotSpawnRight.position, shotSpawnRight.rotation);
                    shot.GetComponent<Mover>().setDirection("right");

                } else if (positionFaced == "left") {
                    soundEffects[1].Play();
                    shot = (GameObject) Instantiate(shotPrefab, shotSpawnLeft.position, shotSpawnLeft.rotation);
                    shot.GetComponent<Mover>().setDirection("left");
                }

                soundEffects[1].Play();
                shot.GetComponent<Bolt>().SetUsername(username);
                shot.GetComponent<Bolt>().SetType(1);
                shot.GetComponent<Bolt>().SetId();
                shot.GetComponent<Mover>().setSpeed(0.5f);
                boltsFired.Add(shot.GetComponent<Bolt>());

                shootButton.setClickedState(false);
            }

            yield return new WaitForSeconds(0.01F);
        }

        yield break;
    }

    IEnumerator CheckForBoost() {

        while (boostButton) {
            if (boostButton.IsClicked()) {

                this.speed *= boostMultiplier;
                yield return new WaitForSeconds(boostDuration);

                this.speed /= boostMultiplier;
                boostButton.setClickedState(false);
            }
            yield return new WaitForSeconds(0.01F);
        }
    }

    IEnumerator checkForShield() {

        while (shieldButton) {
            if (shieldButton.IsClicked()) {
                shieldActive = true;
                this.shield.SetActive(true);
                yield return new WaitForSeconds(shieldDuration);

                shieldActive = false;
                this.shield.SetActive(false);
                shieldButton.setClickedState(false);
            }
            yield return new WaitForSeconds(0.01F);
        }
    }

    IEnumerator checkForSkill() {

        while (skillButton) {
            if (skillButton.IsClicked()) {

                if (positionFaced == "right") {
                    specialShot = (GameObject) Instantiate(specialShotPrefab, shotSpawnRight.position, shotSpawnRight.rotation);
                    specialShot.GetComponent<Mover>().setDirection("right");

                } else if (positionFaced == "left") {
                    specialShot = (GameObject) Instantiate(specialShotPrefab, shotSpawnLeft.position, shotSpawnLeft.rotation);
                    specialShot.GetComponent<Mover>().setDirection("left");
                }

                soundEffects[1].Play();
                specialShot.GetComponent<Bolt>().SetUsername(username);
                specialShot.GetComponent<Bolt>().SetType(1);
                specialShot.GetComponent<Bolt>().SetId();
                specialShot.GetComponent<Mover>().setSpeed(0.5f);
                specialShotFired.Add(specialShot.GetComponent<Bolt>());

                skillButton.setClickedState(false);
            }
            yield return new WaitForSeconds(0.01F);
        }
    }

    IEnumerator CheckForDeath() {
        while (!(this.IsDead())) {
            yield return new WaitForSeconds(0.5F);
        }

        Instantiate(explosion,
            this.gameObject.transform.position,
            Quaternion.identity);

        Destroy(this.gameObject);
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

    public bool IsDead() {
        return health <= 0;
    }

    public void increaseHP(int delta) {
        health += delta;
    }

    public void decreaseHP(int delta) {
        if (!shieldActive) {
            health -= delta;
        }
    }

    public void SetType(string value) {
        type = value;
    }

    public void Move(Vector2 direction) {
        this.gameObject.transform.Translate(direction * this.speed * Time.deltaTime);
        this.UpdatePosition(gameObject.transform.position.x, gameObject.transform.position.y);

        if (direction.x > 0) {
            this.SwitchSide("right");
        } else if (direction.x < 0) {
            this.SwitchSide("left");
        }
    }

    public void UpdatePosition(float new_pos_x, float new_pos_y) {
        position_x = new_pos_x;
        position_y = new_pos_y;
    }

    public float GetPositionX() {
        return position_x;
    }

    public float GetPositionY() {
        return position_y;
    }

    public string GetPlayerType() {
        return type;
    }

    public string GetId() {
        return id;
    }

    public void SetId(string id) {
        this.id = id;
    }

    public string GetUsername() {
        return username;
    }

    public void SetUsername(string username) {
        this.username = username;
    }

    public List<Bolt> GetBoltsFired() {
        return boltsFired;
    }

    public void TestCollider(int damage) {
        decreaseHP(damage);
        Debug.Log(health);
    }
}