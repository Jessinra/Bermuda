using System.Collections.Generic;

[System.Serializable]
public class RoomData
{
    public int roomID;
    public string roomName = "Jax's quest";
    public List<PlayerData> players; // List of player in the room
    public bool isFull = false;
}
