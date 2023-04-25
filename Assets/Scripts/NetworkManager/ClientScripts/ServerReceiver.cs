using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Client class that receives a broadcasted signal from a server/host.
/// </summary>
public class ServerReceiver : MonoBehaviour
{
    public UnityEvent<IPEndPoint> OnSuccessfulAuthentificationEvent = new UnityEvent<IPEndPoint>();

    bool isReceiving;
    UdpClient udpClient;

    public void SetReceivingState(bool flag) {
        isReceiving = flag;
    }

    async void Start()
    {
        udpClient = new UdpClient();
        udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, Constants.BROADCAST_RECEIVER_PORT));

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
                    var message = Encoding.UTF8.GetString(recvBuffer);
                    // Debug.Log(message);
                    // Debug.Log($"{from.Address}:{from.Port}");

                    if (isReceiving) {
                        Authenticate(message, from);
                    }
                }
                else
                {
                    Debug.Log("No data");
                }
            }
        });

        return task;
    }

    void Authenticate(string code, IPEndPoint endPoint) {
        if (code == Constants.CONNECTION_KEY) {
            OnSuccessfulAuthentificationEvent.Invoke(endPoint);
        }
    }
}
