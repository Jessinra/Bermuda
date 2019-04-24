using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetMQ;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class NetworkManager : MonoBehaviour
{
    // public static string BaseUrl = "http://167.205.34.54:5000";
    
    public static string BaseUrl = "http://192.168.43.27:5000";
    public static string BaseSocket = "tcp://192.168.43.27:12345";
    
    private string URL_SEND = "http://192.168.101.130:5000/api/match/update";
    private string URL_RECV = "tcp://167.205.34.54:12345";

    private string room_id;
    
    [SerializeField]private List<Submarine> submarines;
    [SerializeField]private List<Diver> divers;

    [SerializeField] private Player player;

    // Start is called before the first frame update
    void Start()
    {
        // PLayerpref get room_id
        SetRoomId("F6MW2");
        InvokeRepeating("SendInformation", 0.05f, 0.05f);
        Task.Run(() => ReceiveInformation());
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

    private void ReceiveInformation()
    {
        WWW www = new WWW(URL_RECV);   
        string topic = room_id;
        
        using (var subSocket = new NetMQ.Sockets.SubscriberSocket())
        {
            subSocket.Options.ReceiveHighWatermark = 1000;
            subSocket.Connect(URL_RECV);
            subSocket.Subscribe(topic);
            //Console.WriteLine("Subscriber socket connecting...");
            
            while (true)
            {
                var messageTopicReceived = subSocket.ReceiveFrameString();
                var messageReceived = subSocket.ReceiveFrameString();
                //Debug.Log(messageReceived);
                PlayerJson[] players = JsonHelper.FromJson<PlayerJson>(messageReceived);

                foreach (var playerData in players)
                {
                    bool found = false;
                    for(int i = 0; i < submarines.Count && !found; ++i){
                        if(submarines[i].GetId() == playerData.id){
                            submarines[i].SetPositionX(playerData.pos_x);
                            submarines[i].SetPositionY(playerData.pos_y);
                            found = true;
                        }
                    }
                    for (int i = 0; i < divers.Count && !found; ++i)
                    {
                        if (divers[i].GetId() == playerData.id)
                        {
                            divers[i].SetPositionX(playerData.pos_x);
                            divers[i].SetPositionY(playerData.pos_y);
                            found = true;
                        }
                    }
                }
            }
        }
    }
    //{"players":[{"team":0,"pos_x":0.0,"pos_y":0.0,"id":null,"status":null,"username":"gerywahyu","type":null,"position":null,"angle":0.0}]}
}