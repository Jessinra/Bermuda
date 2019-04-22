
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{	

	public string sceneName;

	void Update () {

		if(Input.GetMouseButtonDown(0)) {
				LoadScene(sceneName);
		}
	}

	public void LoadScene(string sceneName){

		Debug.Log("test");
        SceneManager.LoadScene(sceneName);
    }
	
	
}
