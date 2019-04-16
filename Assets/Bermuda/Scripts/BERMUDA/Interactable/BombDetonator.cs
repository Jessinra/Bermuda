using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDetonator : MonoBehaviour
{
    [SerializeField] float blinkDelay = 0.1F;
    [SerializeField] GameObject explosion = null;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("enter bomb");
        if(other.gameObject.tag == "Player"){
            StartCoroutine(blink());
        }
    }

    IEnumerator blink(){

        GameObject blinkLight = this.gameObject.transform.Find("BlinkLight").gameObject;
        if(!blinkLight){
            yield break;
        }

        for(int i = 0; i < 5; i++){
            blinkLight.SetActive(true);
            yield return new WaitForSeconds(blinkDelay);

            blinkLight.SetActive(false);
            yield return new WaitForSeconds(blinkDelay);
        }

        Instantiate(explosion, this.gameObject.transform.position, Quaternion.identity);
        Destroy(this.transform.parent.gameObject);
        
        Debug.Log("EKUSUPUROSIONNNNNNN");
        yield break;
    }
}
