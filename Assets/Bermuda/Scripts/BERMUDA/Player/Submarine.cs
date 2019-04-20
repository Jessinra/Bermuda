using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Submarine : Player {

    public GameObject bubbleParticleLeft = null;
    public GameObject bubbleParticleRight = null;
    

    // Start is called before the first frame update
    new void Start() {
        base.Start();

        positionFaced = "right";
        health = 100;
    }

    // Switch submarine's sprited render side according to it's direction
    public override void SwitchSide(string position) {
        positionFaced = position;
        if (position == "right") {
            spriteRenderer.flipX = false;
            this.bubbleParticleLeft.SetActive(true);
            this.bubbleParticleRight.SetActive(false);
        } else if(position == "left") {
            Debug.Log("Position: " + position);
            spriteRenderer.flipX = true;
            this.bubbleParticleLeft.SetActive(false);
            this.bubbleParticleRight.SetActive(true);
        }
    }

    public AudioSource GetEngineSound() {
        return soundEffects[0];
    }
}