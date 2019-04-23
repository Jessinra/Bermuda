using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomController : MonoBehaviour
{
	public InputField roomName;
	public InputField playerName;
	public Dropdown types;
	private string playerType;
	private int characterType;

    public void saveRoomData() {


    	 PlayerPrefs.SetString("roomName", roomName.text);
    	 Debug.Log("room saved");
  
    }

    public void saveNewPlayerData() {

    	 PlayerPrefs.SetString("playerName", playerName.text);
    	 Debug.Log("name saved");
    	 PlayerPrefs.SetString("playerType", playerType);
    	 Debug.Log("type saved");
    	 PlayerPrefs.SetInt("characterType", characterType);
    	 Debug.Log("character saved");
    }

    public void saveCharacter(int idx) {
    	characterType = idx;
    	Debug.Log(characterType);
    }

    public void savePlayerType() {
    	playerType = types.options[types.value].text;
    	Debug.Log(playerType);
    }

}
