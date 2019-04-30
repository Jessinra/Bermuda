using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActivePlayer : MonoBehaviour {

    private Player player;

    // Buttons
    [SerializeField] private Button shootButton = null;
    [SerializeField] private Button skillButton = null;
    [SerializeField] private Button shieldButton = null;
    [SerializeField] private Button boostButton = null;

    // Cooldowns
    [SerializeField] private float shotCooldown = 0.25F;
    [SerializeField] private float skillCooldown = 5.0F;
    [SerializeField] private float shieldCooldown = 8.0F;
    [SerializeField] private float boostCooldown = 8.0F;

    // Boosting
    [SerializeField] private float boostMultiplier = 2F;
    [SerializeField] private float boostDuration = 3F;

    // Shield
    [SerializeField] private float shieldDuration = 3F;

    protected void Start() {
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

        while (shootButton) {
            if (shootButton.IsClicked()) {

                this.player.CreateDefaultShot();
                yield return new WaitForSeconds(shotCooldown);
                shootButton.setClickedState(false);

            } else {
                yield return new WaitForSeconds(0.01F);
            }
        }
        yield break;
    }

    IEnumerator checkForSkill() {

        while (skillButton) {
            if (skillButton.IsClicked()) {

                this.player.CreateSpecialShot();
                yield return new WaitForSeconds(skillCooldown);
                skillButton.setClickedState(false);

            } else {
                yield return new WaitForSeconds(0.01F);
            }
        }
        yield break;
    }

    IEnumerator checkForShield() {

        while (shieldButton) {
            if (shieldButton.IsClicked()) {

                this.player.ActivateShield();
                yield return new WaitForSeconds(shieldDuration);
                this.player.DeactivateShield();
                yield return new WaitForSeconds(shieldCooldown);
                shieldButton.setClickedState(false);

            } else {
                yield return new WaitForSeconds(0.01F);
            }
        }
        yield break;
    }

    IEnumerator CheckForBoost() {

        while (boostButton) {
            if (boostButton.IsClicked()) {

                this.player.SetSpeed(this.player.GetSpeed() * boostMultiplier);
                yield return new WaitForSeconds(boostDuration);
                this.player.SetSpeed(this.player.GetSpeed() / boostMultiplier);
                yield return new WaitForSeconds(boostCooldown);
                boostButton.setClickedState(false);

            } else {
                yield return new WaitForSeconds(0.01F);
            }
        }
        yield break;
    }
}