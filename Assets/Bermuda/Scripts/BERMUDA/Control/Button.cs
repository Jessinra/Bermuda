using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour
{
    private bool Clicked = false;

    // Detect if click occurs
    private void OnMouseDown()
    {
        Clicked = true;
    }

    public bool IsClicked()
    {
        return Clicked;
    }

    public void setClickedState(bool state)
    {
        Clicked = state;
    }

}
