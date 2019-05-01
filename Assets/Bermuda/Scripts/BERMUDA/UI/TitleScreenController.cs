using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            Task.Run(() => Login(username, password));
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
        Debug.Log("Payload: " + payload);
        var response = Util.Post(NetworkManager.BaseUrl + "/api/auth", payload);
        Debug.Log("Response: " + response);
    }
}
