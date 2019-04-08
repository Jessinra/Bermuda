using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetMQ;
using System;

// Receive data from another sender socket
public class Puller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AsyncIO.ForceDotNet.Force();
        string address = "localhost";
        string[] ports = { "5557",
            //5558,
        };
        // Bind to address and port as Puller
        using (var receiver = new NetMQ.Sockets.PullSocket("tcp://" + address + ":" + ports[0]))
        //using (var sender = new NetMQ.Sockets.PushSocket("tcp://" + address + ":" + ports[1]))
        {
            string str = "";
            // set timeout for receive request
            TimeSpan timeout = new TimeSpan(100000000);
            // try receive string frame and put it to str
            if (receiver.TryReceiveFrameString(timeout, out str))
            {
                Debug.Log(str);
                //sender.TrySendFrame(timeout, "World", false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
