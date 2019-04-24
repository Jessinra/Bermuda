using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Submarine : Player {

    private int FUEL_MAX_VALUE = 100;
    private int FUEL_MIN_VALUE = 0;
    private int FUEL_INCREASE_VALUE = 10;
    private int FUEL_DECREASE_VALUE = 2;

    public GameObject bubbleParticleLeft = null;
    public GameObject bubbleParticleRight = null;

    private int fuel;

    // Start is called before the first frame update
    new void Start() {
        base.Start();

        positionFaced = "right";
        health = 100;
        fuel = 100;
        InvokeRepeating("DecreaseFuel", 0.1f, 10.0f);
    }

    // Switch submarine's sprited render side according to it's direction
    public override void SwitchSide(string position) {
        positionFaced = position;
        if (position == "right") {
            spriteRenderer.flipX = false;
            this.bubbleParticleLeft.SetActive(true);
            this.bubbleParticleRight.SetActive(false);
        } else if(position == "left") {
            spriteRenderer.flipX = true;
            this.bubbleParticleLeft.SetActive(false);
            this.bubbleParticleRight.SetActive(true);
        }
    }

    public AudioSource GetEngineSound() {
        return soundEffects[0];
    }

    public void IncreaseFuel()
    {
        if(fuel < FUEL_MAX_VALUE && (fuel+FUEL_INCREASE_VALUE) <= FUEL_MAX_VALUE)
        {
            fuel += FUEL_INCREASE_VALUE;
        }
        else if((fuel+FUEL_INCREASE_VALUE) > FUEL_MAX_VALUE)
        {
            fuel = FUEL_MAX_VALUE;
        }
        
    }

    public void DecreaseFuel()
    {
        if(fuel > FUEL_MIN_VALUE && (fuel-FUEL_DECREASE_VALUE) >= FUEL_MIN_VALUE)
        {
            fuel -= FUEL_DECREASE_VALUE;
        }
        else if((fuel-FUEL_DECREASE_VALUE) < 0)
        {
            fuel = FUEL_MIN_VALUE;
        }

        // Debug.Log("Fuel: " + fuel);
    }
    
}