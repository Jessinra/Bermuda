using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpController : MonoBehaviour
{
	public Text title;
	public Text description;

    // Start is called before the first frame update
    void Start()
    {
        title.text = "";
        description.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
