using System.Collections.Generic;

[System.Serializable]
public class RoomData
{
    public int roomID;
    public string roomName;
    public List<PlayerData> players; // List of player in the room
    public bool isFull;

    public RoomData(int id, string name) {
    	roomID = id;
    	roomName = name;
    	isFull = false;
    }
}
