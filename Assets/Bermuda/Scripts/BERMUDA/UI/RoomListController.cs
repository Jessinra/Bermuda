using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


public class RoomListController : MonoBehaviour
{
    public GameObject room;
    public GameObject list;
    Text roomName;
    Text players;
    int playerCount;
    Vector2 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        Task.Run(PopulateRooms);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void PopulateRooms()
    {
        startingPosition = new Vector2(0, 105);
        var rooms = GetWaitingRooms();
        foreach(var roomData in rooms)
        {
            Instantiate(room, startingPosition, Quaternion.identity, list.transform);
            roomName = room.transform.Find("Image/roomname").GetComponent<Text>();
            players = room.transform.Find("Image/players").GetComponent<Text>();
            
            roomName.text = "Your name"; //roomName.text = data.name;
            players.text = roomData.numOfPlayer + " / 16 joined";
            
            startingPosition = new Vector2(startingPosition.x, startingPosition.y - 105);
        }
    }
    
    private static IEnumerable<Room> GetWaitingRooms()
    {
        var response = Util.Get(NetworkManager.BaseUrl + "/match");
        return JsonUtility.FromJson<List<Room>>(response);
    }
}