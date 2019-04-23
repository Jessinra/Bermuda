using UnityEngine;

public class TestJsonHelper : MonoBehaviour
{
    PlayerData[] players;
    string json;
    private void Start()
    {
        players = new PlayerData[2];
        players[0] = new PlayerData
        {
            id = "0",
            pos_x = 24,
            pos_y = 52
        };
        players[1] = new PlayerData
        {
            id = "1",
            pos_x = 17,
            pos_y = 48
        };
        json = JsonHelper.ToJson<PlayerData>(players, false);
        Debug.Log(json);
        players[0].id = "ss";
        players[0].pos_x = 25;
        players[1].id = "sst";
        players = JsonHelper.FromJson<PlayerData>(json);
        Debug.Log(players[0].pos_x);
        Debug.Log(players[1].id);
    }
}
