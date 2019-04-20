using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Submarine : Player {

    [NonSerialized] public GameObject bubbleParticleLeft = null;
    [NonSerialized] public GameObject bubbleParticleRight = null;
    
    List<Bolt> boltFired = new List<Bolt>();

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