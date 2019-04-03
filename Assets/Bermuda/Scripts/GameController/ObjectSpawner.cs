using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour {

    protected MazeBlueprint mazeBlueprint;
    protected bool mazeBlueprintReady = false;

    protected const int tileScale = 6;
    protected const int tileCenterOffset = tileScale / 2;

    protected void Initialize() {
        mazeBlueprint = new MazeBlueprint(-1, -1);
        StartCoroutine(loadMazeBlueprint());
    }

    IEnumerator loadMazeBlueprint() {

        int bluePrintAvailable = 0;

        while (bluePrintAvailable != 200) {
            yield return new WaitForSeconds(0.2F);
            bluePrintAvailable = (int) PlayerPrefs.GetInt("mazeBlueprintReady");
        }

        String data = PlayerPrefs.GetString("mazeBlueprint");
        this.mazeBlueprint.deserialize(data);

        this.mazeBlueprintReady = true;
        yield return null;
    }
}