using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerListController : MonoBehaviour
{
	public GameObject player;
	public GameObject list;
	public Sprite char1;
	public Sprite char2;
	public Sprite char3;
	Text playerName;
	Image character;
	Text status;
	int charID = 2;
	Vector2 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = new Vector2(80, 80);
        for(int i = 0; i < 5; i++) //foreach (data in playerlists)
        {
        	// instantiate if no submarine/diver
        	Instantiate(player, startingPosition, Quaternion.identity, list.transform);
        	playerName = player.transform.Find("playername").GetComponent<Text>();
        	character = player.transform.Find("player").GetComponent<Image>();
        	status = player.transform.Find("status").GetComponent<Text>();
        	if (charID == 1 ) {
        		character.sprite = char1;
        	}
        	else if (charID == 2 ) {
        		character.sprite = char2;
        	}
        	else {
        		character.sprite = char3;
        	}
        	playerName.text = "Your name"; //playerName.text = data.name;
        	status.text = "has a submarine"; //changes depending the lack of submarine/diver
        	startingPosition = new Vector2(startingPosition.x, startingPosition.y-85);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
