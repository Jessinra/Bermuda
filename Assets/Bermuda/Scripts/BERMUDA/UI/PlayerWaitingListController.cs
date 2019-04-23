using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerWaitingListController : MonoBehaviour
{
	public Text roomName;
	public Text players;

    // Start is called before the first frame update
    void Start()
    {
        roomName.text = "Battle Royale (10/10)";
        players.text = "";

        for (int i = 0; i < 5; i++) {
        	players.text += "- Team " + (i+1) + ": Nama (Submarine) & Saya (diver) \n";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
