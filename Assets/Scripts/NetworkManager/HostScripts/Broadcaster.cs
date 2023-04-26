using System.Collections;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Broadcaster : MonoBehaviour
{
    Socket socket;
    bool isBroadcasting;

    Coroutine broadcastingCoroutineHandle;

    void Start() {
        socket = new Socket(AddressFamily.InterNetwork,
        SocketType.Dgram, ProtocolType.Udp);
        socket.Bind(new IPEndPoint(IPAddress.Any, Constants.BROADCAST_TRANSMITTER_PORT));
        socket.Connect(new IPEndPoint(IPAddress.Broadcast, Constants.BROADCAST_RECEIVER_PORT));
    }

    public void StartBroadcasting() {
        if (isBroadcasting || broadcastingCoroutineHandle != null) 
            return;

        isBroadcasting = true;
        broadcastingCoroutineHandle = StartCoroutine(BroadcastingCoroutine());
    }

    public void StopBroadcasting() {
        isBroadcasting = false;

        StopCoroutine(broadcastingCoroutineHandle);
        broadcastingCoroutineHandle = null;
    }

    IEnumerator BroadcastingCoroutine() {
        while (isBroadcasting) {
            BroadcaseSignal();

            yield return new WaitForSeconds(0.5f);
        }
    }

    void BroadcaseSignal() {
        
        socket.Send(System.Text.ASCIIEncoding.ASCII.GetBytes(Constants.CONNECTION_KEY));
    }
}
