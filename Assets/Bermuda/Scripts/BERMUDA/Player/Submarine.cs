using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Submarine : Player {

    private int sub_type;
    
    [SerializeField] private GameObject bubbleParticleLeft = null;
    [SerializeField] private GameObject bubbleParticleRight = null;

    // Start is called before the first frame update
    new void Start() {
        base.Start();

        positionFaced = "right";
        health = 100;
        sub_type = 1;
    }

    // Switch submarine's sprited render side according to it's direction
    public override void SwitchSide(string position) {
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