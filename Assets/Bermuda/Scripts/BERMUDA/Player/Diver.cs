using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Diver : Player {
    
    // Attributes
    private int diverType;

    // Start is called before the first frame update
    new void Start() {
        base.Start();

        health = 100;
        diverType = 1;
    }

    // Switch submarine's sprited render side according to it's direction
    public override void SwitchSide(string position) {
        positionFaced = position;
        if (position == "left") {
            spriteRenderer.flipX = false;
        } else {
            spriteRenderer.flipX = true;
        }
    }
}