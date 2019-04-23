using UnityEngine;

public class TestJsonHelper : MonoBehaviour
{
    PlayerJson[] players;
    string json;
    private void Start()
    {
        players = new PlayerJson[2];
        players[0] = new PlayerJson
        {
            id = "0",
            pos_x = 24,
            pos_y = 52
        };
        players[1] = new PlayerJson
        {
            id = "1",
            pos_x = 17,
            pos_y = 48
        };
        json = JsonHelper.ToJson<PlayerJson>(players, false);
        Debug.Log(json);
        players[0].id = "ss";
        players[0].pos_x = 25;
        players[1].id = "sst";
        players = JsonHelper.FromJson<PlayerJson>(json);
        Debug.Log(players[0].pos_x);
        Debug.Log(players[1].id);
    }
}
