using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByBoundary : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Bolt"))
        {
            Destroy(other.gameObject);
        }
    }
}
