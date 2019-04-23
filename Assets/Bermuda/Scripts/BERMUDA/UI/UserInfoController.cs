using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoController : MonoBehaviour
{

	public GameObject player;
	public Slider HP;
	public Slider Fuel;
	public Sprite char1;
	public Sprite char2;
	public Sprite char3;
	public Text playerName;
	public Image character;
	int charID = 2;


    // Start is called before the first frame update
    void Start()
    {
        playerName.text = "insert name";
        HP.value = 100;
        Fuel.value = 100;
        
        if (charID == 1 ) {
        		character.sprite = char1;
        	}
        	else if (charID == 2 ) {
        		character.sprite = char2;
        	}
        	else {
        		character.sprite = char3;
        	}

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
