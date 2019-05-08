using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ActivePlayer : MonoBehaviour {

    private Player player;

    // Keyboard Inputs
    private String KEY_SHOOT = "s";
    private String KEY_SKILL = "a";
    private String KEY_BOOST = "w";
    private String KEY_SHIELD = "d";

    // Buttons
    [SerializeField] private ButtonCooldown shootButton = null;
    [SerializeField] private ButtonCooldown skillButton = null;
    [SerializeField] private ButtonCooldown shieldButton = null;
    [SerializeField] private ButtonCooldown boostButton = null;

    private GameObject shotCooldownOverlay;
    private GameObject skillCooldownOverlay;
    private GameObject shieldCooldownOverlay;
    private GameObject boostCooldownOverlay;

    private int numOfChest;

    // Boosting
    [SerializeField] private float boostMultiplier = 2F;
    [SerializeField] private float boostDuration = 3F;

    // Shield
    [SerializeField] private float shieldDuration = 3F;

    protected void Start() {
        this.shotCooldownOverlay = shootButton.transform.Find("CooldownOverlay").gameObject;
        this.skillCooldownOverlay = skillButton.transform.Find("CooldownOverlay").gameObject;
        this.shieldCooldownOverlay = shieldButton.transform.Find("CooldownOverlay").gameObject;
        this.boostCooldownOverlay = boostButton.transform.Find("CooldownOverlay").gameObject;
        StartCoroutine(Setup());
    }

    IEnumerator Setup() {

        while (this.player == null) {
            yield return new WaitForSeconds(0.1F);
            this.player = this.transform.parent.gameObject.GetComponent<Player>();
        }

        StartCoroutine(CheckForShot());
        StartCoroutine(CheckForBoost());
        StartCoroutine(checkForShield());
        StartCoroutine(checkForSkill());

        yield break;
    }

    IEnumerator CheckForShot() {

        while (shootButton != null) {
            if (!(shootButton.IsCooldown()) && (shootButton.IsClicked() || Input.GetKeyDown(KEY_SHOOT)) ) {

                shootButton.StartCooldown();
                this.player.CreateDefaultShot();
            } 
            yield return new WaitForSeconds(0.01F);
        }
        yield break;
    }

    IEnumerator checkForSkill() {

        while (skillButton != null) {
            if (!(skillButton.IsCooldown()) && (skillButton.IsClicked() || Input.GetKeyDown(KEY_SKILL)) ) {

                skillButton.StartCooldown();
                this.player.CreateSpecialShot();
            } 
            yield return new WaitForSeconds(0.01F);
        }
        yield break;
    }

    IEnumerator checkForShield() {

        while (shieldButton != null) {
            if (!(shieldButton.IsCooldown()) && (shieldButton.IsClicked() || Input.GetKeyDown(KEY_SHIELD)) ) {

                shieldButton.StartCooldown();

                // Execute shield
                this.player.ActivateShield();
                yield return new WaitForSeconds(shieldDuration);
                this.player.DeactivateShield();

            } 
            yield return new WaitForSeconds(0.01F);
        }
        yield break;
    }

    IEnumerator CheckForBoost() {

        while (boostButton != null) {
            if (!(boostButton.IsCooldown()) && (boostButton.IsClicked() || Input.GetKeyDown(KEY_BOOST)) ) {

                boostButton.StartCooldown();

                // Execute boost
                this.player.ActivateBoost(boostMultiplier);
                yield return new WaitForSeconds(boostDuration);
                this.player.DeactivateBoost(boostMultiplier);

            } 
            yield return new WaitForSeconds(0.01F);
        }
        yield break;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Chest"))
        {
            numOfChest++;
        }

        if (numOfChest > 0)
        {
            SceneManager.LoadScene("EndScene");
        }
    }
}