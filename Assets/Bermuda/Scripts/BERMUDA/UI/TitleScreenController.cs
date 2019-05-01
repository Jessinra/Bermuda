using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class TitleScreenController : MonoBehaviour
{
    public Button startButton;
    public InputField usernameInputField;
    public InputField passwordInputField;
    public string nextScene = "SelectRoom";

    private void Update()
    {
        if (startButton.IsClicked())
        {
            var username = usernameInputField.text;
            var password = passwordInputField.text;
            Login(username, password);
            SceneManager.LoadScene(nextScene);
        }
    }

    private void Login(string username, string password)
    {
        var requestObject = new
        {
            username,
            password
        };
        var payload = JsonUtility.ToJson(requestObject);
        Debug.Log(payload);
    }
}
