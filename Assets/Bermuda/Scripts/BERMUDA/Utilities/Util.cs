using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Util : MonoBehaviour
{
    public int GenerateRandomInt(int min, int max)
    {
        System.Random rand = new System.Random();

        return rand.Next(min, max);
    }
}
