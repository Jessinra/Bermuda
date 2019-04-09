using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

// Send data to another receiver socket
public class Pusher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AsyncIO.ForceDotNet.Force();
        string address = "localhost";
        string[] ports = { "5557",
             //5558,
         };
        // Bind to address and port as Pusher
        using (var sender = new NetMQ.Sockets.PushSocket("tcp://" + address + ":" + ports[0]))
        //using (var receiver = new NetMQ.Sockets.PullSocket("tcp://" + address + ":" + ports[1]))
        {
            // set timeout for send request
            TimeSpan timeout = new TimeSpan(100000000);
            // try send string frame containing "Hello"
            NetMQ.Msg message = new NetMQ.Msg();
            // Init message with "Hello"
            message.InitGC(Encoding.ASCII.GetBytes("Hello"), "Hello".Length);
            sender.TrySend(msg: ref message, timeout: timeout, more: false);
            // Try to receive reply
            //if (receiver.TryReceive(ref message, timeout))
            //{
            //    Console.WriteLine(Encoding.UTF8.GetString(message.Data, 0, message.Data.Length));
            //}
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
