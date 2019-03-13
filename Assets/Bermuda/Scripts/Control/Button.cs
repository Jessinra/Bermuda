using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour
{
    private bool isClicked = false;

    // Detect if click occurs
    private void OnMouseDown()
    {
        isClicked = true;
    }

    public bool getClickedState()
    {
        return isClicked;
    }

    public void setClickedState(bool state)
    {
        isClicked = state;
    }

}
