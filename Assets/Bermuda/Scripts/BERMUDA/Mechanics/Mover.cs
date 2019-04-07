using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    private string direction;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = transform.forward * speed;
    }

    private void FixedUpdate()
    {
        if(direction == "left")
        {
            rb.transform.Translate(new Vector2(speed * -1, 0.0f));
        }
        else if(direction == "right")
        {
            rb.transform.Translate(new Vector2(speed, 0.0f));
        }
        
    }

    public void setDirection(string dir)
    {
        direction = dir;
    }

    public string getDirection()
    {
        return direction;
    }
}
