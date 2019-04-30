using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Diver : Player {
    
    // Start is called before the first frame update
    new void Start() {
        base.Start();
    }

    // Switch submarine's sprited render side according to it's direction
    protected override void SwitchSide(string direction) {
        faceDirection = direction;
        if (faceDirection == "left") {
            spriteRenderer.flipX = false;
        } else if (faceDirection == "right"){
            spriteRenderer.flipX = true;
        }
    }
}