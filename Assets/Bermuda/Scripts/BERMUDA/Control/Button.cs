using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour {

    protected bool Clicked = false;

    // Detect if click occurs
    protected void OnMouseDown() {
        Clicked = true;
    }

    public bool IsClicked() {
        return Clicked;
    }

    public void setClickedState(bool state) {
        Clicked = state;
    }
}