using System;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Configuration;
using UnityEngine;

public class EmployeeTCPServer : MonoBehaviour
{
    static TcpListener listener;
    const int LIMIT = 5; //5 concurrent clients

    void Start()
    {
        listener = new TcpListener(IPAddress.Loopback, 8000);
        listener.Start();
        Debug.Log("Server mounted, listening to port 8000");


        for (int i = 0; i < LIMIT; i++)
        {
            Thread t = new Thread(new ThreadStart(Service));
            t.Start();
        }
    }

    public void Service()
    {
        while (true)
        {
            AcceptSocket();
        }
    }

    void AcceptSocket()
    {
        Socket soc = listener.AcceptSocket();
        //soc.SetSocketOption(SocketOptionLevel.Socket,
        //        SocketOptionName.ReceiveTimeout,10000);
        Debug.Log($"Connected: {soc.RemoteEndPoint}");
        try
        {
            Stream s = new NetworkStream(soc);
            StreamReader sr = new StreamReader(s);
            StreamWriter sw = new StreamWriter(s);
            sw.AutoFlush = true; // enable automatic flushing
            while (true)
            {
                string name = sr.ReadLine();
                if (name == "" || name == null) break;
                string job = "kk";
                if (job == null) job = "No such employee";
                sw.WriteLine(job);
            }
            s.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        Debug.Log("Disconnected: {soc.RemoteEndPoint}");
        soc.Close();
    }
}