using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class AuthController : MonoBehaviour
{
    // CONST
    private static string URL_TEST = "http://localhost:3333/data";
    private static string URL_REGISTER = "http://192.168.43.27:5000/api/auth/register";
    private static string URL_LOGIN = "http://192.168.101.130:5000/api/auth/login";
    private static string URL_MATCH_CREATE = "http://192.168.101.130:5000/api/match/create";
    public static string sign_in_result;
    public static string room_id;

    // VARIABLES
    private static string roomid;

    // Start is called before the first frame update
    void Start()
    {
        sign_in_result = "";
        room_id = "";

        CreateMatch("dummy");
    }


    public void Login(string username, string password)
    {
        Thread loginThread = new Thread(() =>
        {
            Debug.Log("LoginThread activated");
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL_LOGIN);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{ \"username\": \"" + username + "\", " +
                              "\"password\": \"" + password + "\" }";
                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                sign_in_result = streamReader.ReadToEnd();
            }

            Debug.Log("Result LoginThread: " + sign_in_result);
        });

        Debug.Log("Result Main Thread: " + sign_in_result);
        loginThread.Start();
    }


    
    public static async Task Register(string username, string password)
    {
        var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL_REGISTER);
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";

        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            string json = "{ \"username\": \"" + username + "\", " +
                          "\"password\": \"" + password + "\" }";
            streamWriter.Write(json);
        }

        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
        }
    }

    public void CreateMatch(string username)
    {
        Thread createMatchThread = new Thread(() =>
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL_MATCH_CREATE);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{ \"username\": \"" + username + "\" }";
                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                room_id = streamReader.ReadToEnd();
            }
            Debug.Log("Room ID (Child Thread) : " + room_id);
        });

        createMatchThread.Start();
    }

    public string GetRoomId()
    {
        return roomid;
    }
}
