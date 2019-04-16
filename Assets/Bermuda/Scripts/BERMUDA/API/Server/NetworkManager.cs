using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    private string URL_SEND = "http://localhost:3333/data";

    [SerializeField] private Submarine player;
    [SerializeField] private List<Bolt> bolts;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SendData());
    }

    // Send data to server
    IEnumerator SendData()
    {
        // Send Player Information starting in 0.5 seconds, then continue every 0.5 seconds
        InvokeRepeating("SendPlayerInformation", 0.5f, 0.5f);

        yield return null;
    }

    private void SendPlayerInformation()
    {
        WWWForm form = new WWWForm();
        form.AddField("position_x", player.GetPositionX().ToString());
        form.AddField("position_y", player.GetPositionY().ToString());
        if (player != null)
        {
            form.AddField("player_type", "submarine");
            form.AddField("sub_type", player.GetPlayerType().ToString());
        }

        UnityWebRequest www = UnityWebRequest.Post(URL_SEND, form);
        www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }

    }
}
