using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ServerReceiver : MonoBehaviour
{
    [SerializeField] int port = 8001;
    UdpClient udpClient;

    async void Start()
    {
        udpClient = new UdpClient();
        udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, port));

        await StartTask();
    }

    Task StartTask()
    {

        var from = new IPEndPoint(0, 0);
        var task = Task.Run(() =>
        {
            while (true)
            {
                var recvBuffer = udpClient.Receive(ref from);
                if (recvBuffer != null)
                {
                    Debug.Log(Encoding.UTF8.GetString(recvBuffer));
                    Debug.Log($"{from.Address}:{from.Port}");

                    from.Port = 8003;
                    udpClient = new UdpClient();
                    udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, 8009));

                    var msg = Encoding.UTF8.GetBytes("Hello back");

                    // udpClient.Connect(from);
                    udpClient.Send(msg, msg.Length, from);
                }
                else
                {
                    Debug.Log("No data");
                }
            }
        });

        return task;
    }
}
