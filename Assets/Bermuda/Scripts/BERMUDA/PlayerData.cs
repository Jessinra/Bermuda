using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public int playerID;
    public string playerName;
    public string role;
    public int characterID; // avatar from pre-defined assets
    public int teamID; // to determine which team the player belong to

    public PlayerData(int id, string name, string r, int charid, int team) {
    	playerID = id;
    	playerName = name;
    	role = r;
    	characterID = charid;
    	teamID = team;
    }
}
