using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActivePlayer : MonoBehaviour {

    private Player player;

    // Buttons
    [SerializeField] private ButtonCooldown shootButton = null;
    [SerializeField] private ButtonCooldown skillButton = null;
    [SerializeField] private ButtonCooldown shieldButton = null;
    [SerializeField] private ButtonCooldown boostButton = null;

    private GameObject shotCooldownOverlay;
    private GameObject skillCooldownOverlay;
    private GameObject shieldCooldownOverlay;
    private GameObject boostCooldownOverlay;

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
            if (!(shootButton.IsCooldown()) && shootButton.IsClicked()) {

                shootButton.StartCooldown();
                this.player.CreateDefaultShot();
            } 
            yield return new WaitForSeconds(0.01F);
        }
        yield break;
    }

    IEnumerator checkForSkill() {

        while (skillButton != null) {
            if (!(skillButton.IsCooldown()) && skillButton.IsClicked()) {

                skillButton.StartCooldown();
                this.player.CreateSpecialShot();
            } 
            yield return new WaitForSeconds(0.01F);
        }
        yield break;
    }

    IEnumerator checkForShield() {

        while (shieldButton != null) {
            if (!(shieldButton.IsCooldown()) && shieldButton.IsClicked()) {

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
            if (!(boostButton.IsCooldown()) && boostButton.IsClicked()) {

                boostButton.StartCooldown();

                // Execute boost
                this.player.SetSpeed(this.player.GetSpeed() * boostMultiplier);
                yield return new WaitForSeconds(boostDuration);
                this.player.SetSpeed(this.player.GetSpeed() / boostMultiplier);

            } 
            yield return new WaitForSeconds(0.01F);
        }
        yield break;
    }
}