using System.Collections;
using System.Collections.Generic;
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
    	startingPosition = new Vector2(250, 210);
        for(int i = 0; i < 5; i++) //foreach (data in roomlists)
        {
        	//if (!data.isFull) {
        	Instantiate(room, startingPosition, Quaternion.identity, list.transform);
        	roomName = room.transform.Find("Image/roomname").GetComponent<Text>();
        	players = room.transform.Find("Image/players").GetComponent<Text>();
        	roomName.text = "Your name"; //roomName.text = data.name;
        	playerCount = 0; //playerCount = data.players.Count();
        	players.text = playerCount + " / 16 joined";
        	startingPosition = new Vector2(startingPosition.x, startingPosition.y-210);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
