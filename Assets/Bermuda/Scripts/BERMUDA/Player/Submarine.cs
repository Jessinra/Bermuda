using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Submarine : Player {

    private float MAX_FUEL = 100.0F;
    [SerializeField] private float fuelConsumtionPerSecond = 0.2F;

    public GameObject bubbleParticleLeft = null;
    public GameObject bubbleParticleRight = null;

    private float fuel = 100;

    new void Start() {
        base.Start();
        StartCoroutine(ConsumeFuel());
    }

    // Switch submarine's sprited render side according to it's direction
    public override void SwitchSide(string position) {
        positionFaced = position;
        if (position == "right") {
            spriteRenderer.flipX = false;
            this.bubbleParticleLeft.SetActive(true);
            this.bubbleParticleRight.SetActive(false);
        } else if (position == "left") {
            spriteRenderer.flipX = true;
            this.bubbleParticleLeft.SetActive(false);
            this.bubbleParticleRight.SetActive(true);
        }
    }

    public AudioSource GetEngineSound() {
        return soundEffects[0];
    }

    public void IncreaseFuel(float delta) {
        this.fuel += delta;
        if (this.fuel > this.MAX_FUEL) {
            this.fuel = this.MAX_FUEL;
        }
    }

    IEnumerator ConsumeFuel() {
        while (true) {
            this.DecreaseFuel(fuelConsumtionPerSecond);
            yield return new WaitForSeconds(1.0F);
        }
    }

    public void DecreaseFuel(float delta) {
        this.fuel -= delta;
    }

    public override float GetFuel() {
        return this.fuel;
    }
}