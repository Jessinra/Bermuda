using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonCooldown : Button {

    [SerializeField] private float cooldownTime = 0.0F;
    private Text displayText = null;
    private GameObject overlay = null;

    private bool onCooldown = false;

    void Start() {

        try {
            displayText = this
                .transform.Find("CooldownOverlay").gameObject
                .transform.Find("Timer").gameObject
                .transform.Find("Text")
                .GetComponent<Text>();
        } catch (Exception) {
            // pass
        }

        try {
            overlay = this.transform.Find("CooldownOverlay").gameObject;

        } catch (Exception) {
            // pass
        }
    }

    public void StartCooldown() {
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown() {
        onCooldown = true;
        overlay.SetActive(true);

        // Display timer countdown
        int cooldownTimeInt = (int) (cooldownTime * 10);
        for (int i = cooldownTimeInt; i > 0; i--) {
            if (displayText != null) {
                displayText.text = (i / 10 + 1).ToString();
            }
            yield return new WaitForSeconds(0.1F);
        }

        overlay.SetActive(false);
        onCooldown = false;
        this.setClickedState(false); // have to be last, so all click on cooldown is canceled

        yield break;
    }

    public bool IsCooldown(){
        return onCooldown;
    }
}