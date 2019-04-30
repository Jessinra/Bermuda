using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullMapController : MonoBehaviour
{
    [SerializeField] private Button mapButton = null;
    [SerializeField] private GameObject fullMapBg = null;
    [SerializeField] private GameObject fullMap = null;

    private bool isMapShown = false;

    void Update()
    {
        if (mapButton.IsClicked()){

            isMapShown = !isMapShown;
            this.fullMap.SetActive(isMapShown);
            this.fullMapBg.SetActive(isMapShown);
            mapButton.setClickedState(false);
        }
    }
}
