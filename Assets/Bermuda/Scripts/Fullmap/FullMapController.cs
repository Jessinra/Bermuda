using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullMapController : MonoBehaviour
{
    private bool isMapShown;
    public GameObject fullMapBg;
    public GameObject fullMap;

    // Start is called before the first frame update
    void Start()
    {
        isMapShown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            Debug.Log("space pressed");

            isMapShown = !isMapShown;
            this.fullMap.SetActive(isMapShown);
            this.fullMapBg.SetActive(isMapShown);
        }
    }
}
