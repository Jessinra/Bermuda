// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using NetMQ;
// using System;
// using System.Text;

// // Receive data from another sender socket
// public class Puller : MonoBehaviour
// {
//     // Start is called before the first frame update
//     void Start()
//     {
//         AsyncIO.ForceDotNet.Force();
//         string address = "localhost";
//         string[] ports = { "5557",
//              //5558,
//          };
//         // Bind to address and port as Puller
//         using (var receiver = new NetMQ.Sockets.PullSocket("tcp://" + address + ":" + ports[0]))
//         //using (var sender = new NetMQ.Sockets.PushSocket("tcp://" + address + ":" + ports[1]))
//         {
//             // Init message
//             NetMQ.Msg message = new NetMQ.Msg();
//             message.InitEmpty();
//             // set timeout for receive request
//             TimeSpan timeout = new TimeSpan(100000000);
//             // try receive message and put it to var message
//             if (receiver.TryReceive(ref message, timeout))
//             {
//                 // convert message data to string and print
//                 Debug.Log(Encoding.UTF8.GetString(message.Data, 0, message.Data.Length));
//                 // Send reply message
//                 //message.InitGC(Encoding.ASCII.GetBytes("World"), "World".Length);
//                 //sender.TrySend(msg: ref message, timeout: timeout, more: false);
//             }
//         }
//     }

//     // Update is called once per frame
//     void Update()
//     {

//     }
// }
