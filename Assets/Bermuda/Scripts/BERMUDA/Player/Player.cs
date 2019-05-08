using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    // Attributes
    [SerializeField][HideInInspector] protected string id;
    [SerializeField][HideInInspector] protected string username;
    [SerializeField][HideInInspector] protected string status;
    [SerializeField][HideInInspector] protected float position_x;
    [SerializeField][HideInInspector] protected float position_y;
    [SerializeField][HideInInspector] protected int type; // 1 2 3 for submarine, 4 5 6 for diver

    protected int health = 100;
    protected int MAX_HP = 100;
    protected float speed = 70;

    [SerializeField] protected PlayerPrefabConfig prefab = null;

    // Sprites
    protected SpriteRenderer spriteRenderer;
    protected string faceDirection = "right";

    // Shots
    private List<Bolt> boltsFired = new List<Bolt>();
    [SerializeField] private ShotSpawner shotSpawner = null;

    // Shield
    private bool shieldActive = false;
    private List<Bolt> specialShotFired = new List<Bolt>();

    // Sound 
    protected AudioSource[] soundEffects;

    protected void Start() {

        // Dummy userdata, TODO: replace with data received from server
        id = "dummy";
        username = "dummy";
        status = "alive";
        position_x = transform.position.x;
        position_y = transform.position.y;
        type = 1;

        // Load Renderer
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        soundEffects = this.GetComponents<AudioSource>();

        StartCoroutine(CheckForDeath());
    }

    /* =====================================================
                    API methods Section
    ===================================================== */

    public void Move(Vector2 direction) {
        this.gameObject.transform.Translate(direction * this.speed * Time.deltaTime);
        this.UpdatePosition(gameObject.transform.position.x, gameObject.transform.position.y);

        if (direction.x > 0) {
            this.SwitchSide("right");
        } else if (direction.x < 0) {
            this.SwitchSide("left");
        }
    }

    public void IncreaseHP(int delta) {
        this.health += delta;

        if (this.health > this.MAX_HP) {
            this.health = this.MAX_HP;
        }
    }

    public void DecreaseHP(int delta) {
        if (!shieldActive) {
            this.health -= delta;
        }
    }

    /* =====================================================
                        Helper Methods Section
    ===================================================== */

    IEnumerator CheckForDeath() {
        while (!(this.IsDead())) {
            yield return new WaitForSeconds(0.5F);
        }

        Instantiate(prefab.explosionPrefab,
            this.gameObject.transform.position,
            Quaternion.identity);

        yield return new WaitForSeconds(4F);
        SceneManager.LoadScene("EndScene");
        
        Debug.Log("notify server dead");
        this.status = "dead";
        Destroy(this.gameObject);
    }

    // Switch player's sprited render side according to it's direction
    protected virtual void SwitchSide(string direction) {
        this.faceDirection = direction;
        if (faceDirection == "left") {
            spriteRenderer.flipX = false;
        } else {
            spriteRenderer.flipX = true;
        }
    }
    

    private void UpdatePosition(float new_pos_x, float new_pos_y) {
        position_x = new_pos_x;
        position_y = new_pos_y;
    }

    private bool IsDead() {
        return this.GetHP() <= 0 || this.GetOxygen() <= 0 || this.GetFuel() <= 0;
    }

    /* =====================================================
                    Getter Setter Section
    ===================================================== */

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

    public float GetPositionX() {
        return position_x;
    }

    public float GetPositionY() {
        return position_y;
    }

    public void SetPositionX(float pos_x) {
        position_x = pos_x;
    }

    public void SetPositionY(float pos_y) {
        position_y = pos_y;
    }

    public int GetPlayerType() {
        return type;
    }

    public void SetPlayerType(int type) {
        this.type = type;
    }

    public int GetHP() {
        return this.health;
    }

    public float GetSpeed() {
        return this.speed;
    }

    public void SetSpeed(float speed) {
        this.speed = speed;
    }

    public List<Bolt> GetBoltsFired() {
        return this.boltsFired;
    }

    public List<Bolt> GetSpecialShotFired() {
        return this.specialShotFired;
    }

    public virtual float GetFuel() {
        return 99999;
    }
    public virtual float GetOxygen() {
        return 99999;
    }

    /* =====================================================
                    Active Player Section
    ===================================================== */

    public void CreateDefaultShot() {
        GameObject shot;
        Transform spawner;

        if (faceDirection == "right") {
            spawner = shotSpawner.right;
        } else if (faceDirection == "left") {
            spawner = shotSpawner.left;
        } else {
            throw new Exception("faceDirection not defined");
        }

        shot = (GameObject) Instantiate(prefab.shotPrefab, spawner.position, spawner.rotation);
        shot.GetComponent<Mover>().setDirection(faceDirection);
        shot.GetComponent<Bolt>().SetUsername(username);
        shot.GetComponent<Bolt>().SetType(1);
        shot.GetComponent<Bolt>().SetId();
        shot.GetComponent<Mover>().setSpeed(0.5f);

        boltsFired.Add(shot.GetComponent<Bolt>());

        soundEffects[1].Play();
    }

    public void ActivateShield() {
        shieldActive = true;
        prefab.shield.SetActive(true);
    }

    public void DeactivateShield() {
        shieldActive = false;
        prefab.shield.SetActive(false);
    }

    public void ActivateBoost(float boostMultiplier) {
        this.speed *= boostMultiplier;
    }

    public void DeactivateBoost(float boostMultiplier) {
        this.speed /= boostMultiplier;
    }

    public void CreateSpecialShot() {
        GameObject specialShot;
        Transform spawner;

        if (faceDirection == "right") {
            spawner = shotSpawner.right;
        } else if (faceDirection == "left") {
            spawner = shotSpawner.left;
        } else {
            throw new Exception("faceDirection not defined");
        }

        specialShot = (GameObject) Instantiate(prefab.specialShotPrefab, spawner.position, spawner.rotation);
        specialShot.GetComponent<Mover>().setDirection(faceDirection);
        specialShot.GetComponent<Bolt>().SetUsername(username);
        specialShot.GetComponent<Bolt>().SetType(1);
        specialShot.GetComponent<Bolt>().SetId();
        specialShot.GetComponent<Mover>().setSpeed(0.5f);

        specialShotFired.Add(specialShot.GetComponent<Bolt>());

        soundEffects[1].Play();
    }

}

[System.Serializable]
public class PlayerPrefabConfig {
    [SerializeField] public GameObject shotPrefab = null;
    [SerializeField] public GameObject shield = null;
    [SerializeField] public GameObject specialShotPrefab = null;
    [SerializeField] public GameObject explosionPrefab = null;
}

[System.Serializable]
public class ShotSpawner {
    [SerializeField] public Transform right = null;
    [SerializeField] public Transform left = null;
}