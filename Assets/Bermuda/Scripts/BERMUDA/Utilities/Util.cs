using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = System.Object;

public class Util : MonoBehaviour
{
    public int GenerateRandomInt(int min, int max)
    {
        System.Random rand = new System.Random();

        return rand.Next(min, max);
    }

    public static string Post(string url, string request)
    {
        var header = new Hashtable();
        header.Add("Content-Type", "application/json");
        var payload = Encoding.UTF8.GetBytes(request);
        
        var www = new WWW(url, payload, header);
        while (!www.isDone)
        {
            
        }

        if (www.error != null)
        {
            throw new Exception();
        }

        return www.text;
    }

    public static string Get(string url)
    {
        var www = new WWW(url);
        while (!www.isDone)
        {
            
        }

        if (www.error != null)
        {
            throw new Exception();
        }

        return www.text;
    }
}
