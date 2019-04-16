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
    private static string URL_REGISTER = "http://192.168.43.27:5000/api/auth/register";
    private static string URL_LOGIN = "http://192.168.43.27:5000/api/auth/login";
    private static string URL_MATCH_CREATE = "http://192.168.43.27:5000/api/match/create";

    private static string roomid;

    // Start is called before the first frame update
    void Start()
    {
        CreateMatch("dummy3");
    }


    public static async Task Login(string username, string password)
    {
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
            var result = streamReader.ReadToEnd();
        }
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

    public static async Task CreateMatch(string username)
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
            var result = streamReader.ReadToEnd();
            roomid = result.ToString();
            UnityEngine.Debug.Log(result);
        }
    }

}
