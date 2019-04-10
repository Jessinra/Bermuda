using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerInsert : MonoBehaviour
{
    public UnityEngine.UI.Button button;

    void OnTriggerEnter(Collider other)
    {
        button.onClick.AddListener(() => InsertGameObject(other));
    }

    private void OnTriggerExit(Collider other)
    {
        button.onClick.RemoveListener(() => InsertGameObject(other));
    }

    void InsertGameObject(Collider other)
    {
        other.gameObject.GetComponent<InventoryController>().Insert(gameObject);
        Destroy(this.gameObject);
    }
}
