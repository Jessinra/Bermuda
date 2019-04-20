using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    private string URL_SEND = "http://192.168.101.130:5000/api/match/update";

    private string room_id;

    [SerializeField] private Player player;

    // Start is called before the first frame update
    void Start()
    {
        SetRoomId("F6MW2");
        InvokeRepeating("SendInformation", 0.05f, 0.05f);
    }

    public void SetRoomId(string id)
    {
        room_id = id;
    }

    public string GetRoomId()
    {
        return room_id;
    }

    private string CreateJSONState()
    {
        string jsonstr = "{ \"roomId\" :" + " \"" + room_id + "\",";
        jsonstr += "\"instances\" : [ ";
        jsonstr += JsonUtility.ToJson(player);
        
        for(int i = 0; i < player.GetBoltsFired().Count; i++)
        {
            jsonstr += ", ";
            jsonstr += JsonUtility.ToJson(player.GetBoltsFired()[i]);
        }

        jsonstr += " ] }";
        Debug.Log(jsonstr);

        return jsonstr;
    }

    private WWW SendInformation()
    {
        
        Hashtable postHeader = new Hashtable();
        postHeader.Add("Content-Type", "application/json");
        var formData = System.Text.Encoding.UTF8.GetBytes(CreateJSONState());
        
        WWW www = new WWW(URL_SEND, formData, postHeader);

        return www;
        /*
        form.AddField("id", player.GetId());
        form.AddField("position_x", player.GetPositionX().ToString());
        form.AddField("position_y", player.GetPositionY().ToString());
        if (player != null)
        {
            form.AddField("type", "submarine");
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
        */
    }
}
