// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using NetMQ;
// using System;

// // Send data to another receiver socket
// public class Pusher : MonoBehaviour
// {
//     // Start is called before the first frame update
//     void Start()
//     {
//         AsyncIO.ForceDotNet.Force();
//         string address = "localhost";
//         string[] ports = { "5557",
//             //5558,
//         };
//         // Bind to address and port as Pusher
//         using (var sender = new NetMQ.Sockets.PushSocket("tcp://" + address + ":" + ports[0]))
//         //using (var receiver = new NetMQ.Sockets.PullSocket("tcp://" + address + ":" + ports[1]))
//         {
//             // set timeout for send request
//             TimeSpan timeout = new TimeSpan(100000000);
//             // try send string frame containing "Hello"
//             sender.TrySendFrame(timeout, "Hello", false);

//             //string str = "";
//             //if (receiver.TryReceiveFrameString(timeout, out str))
//             //{
//             //    // do something
//             //    Debug.Log(str);
//             //}
//         }
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }
