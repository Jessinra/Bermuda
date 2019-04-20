using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    private static readonly List<string> idUsed = new List<string>();
    [SerializeField] [HideInInspector] private string id;
    [SerializeField] [HideInInspector] private string username;
    [SerializeField] [HideInInspector] private int type;
    [SerializeField] [HideInInspector] private int damage;
    public Util util;

    void Start()
    {

    }

    public void SetUsername(string username)
    {
        this.username = username;
    }

    public void SetId()
    {
        string temp_id;
        bool flag = true;
        do
        {
            int id_num = util.GenerateRandomInt(0, 999);
            temp_id = username + "_bolt_" + type.ToString() + "_" + id_num.ToString();

            Debug.Log("temp_id = " + temp_id);

            if (idUsed.Contains(temp_id) == false)
            {
                flag = false;
            }

        } while (flag == true);

        id = temp_id;
        idUsed.Add(id);
    }

    public string GetId()
    {
        return id;
    }

    public void SetType(int type)
    {
        this.type = type;
        damage = type * 10;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            player.decreaseHP(damage);
        }
        idUsed.Remove(id);
        Destroy(gameObject);
    }
}
