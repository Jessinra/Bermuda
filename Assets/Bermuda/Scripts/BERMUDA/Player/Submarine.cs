using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Submarine : Player {

    [SerializeField] private SubmarineBubbleParticle bubbleParticle = null;
    [SerializeField] private float fuelConsumtionPerSecond = 0.2F;

    private float fuel = 100;
    private float MAX_FUEL = 100.0F;

    new void Start() {
        base.Start();
        StartCoroutine(ConsumeFuel());
    }

    IEnumerator ConsumeFuel() {
        while (true) {
            this.DecreaseFuel(fuelConsumtionPerSecond);
            yield return new WaitForSeconds(1.0F);
        }
    }

    public void IncreaseFuel(float delta) {
        this.fuel += delta;
        if (this.fuel > this.MAX_FUEL) {
            this.fuel = this.MAX_FUEL;
        }
    }

    public void DecreaseFuel(float delta) {
        this.fuel -= delta;
    }

    public override float GetFuel() {
        return this.fuel;
    }

    public AudioSource GetEngineSound() {
        return soundEffects[0];
    }

    // Switch submarine's sprited render side according to it's direction
    protected override void SwitchSide(string direction) {
        faceDirection = direction;
        if (faceDirection == "right") {
            spriteRenderer.flipX = false;
            this.bubbleParticle.left.SetActive(true);
            this.bubbleParticle.right.SetActive(false);

        } else if (faceDirection == "left") {
            spriteRenderer.flipX = true;
            this.bubbleParticle.left.SetActive(false);
            this.bubbleParticle.right.SetActive(true);
        }
    }
}

[System.Serializable]
public class SubmarineBubbleParticle {
    public GameObject left = null;
    public GameObject right = null;
}