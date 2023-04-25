using System.Collections;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Broadcaster : MonoBehaviour
{
    bool isBroadcasting;

    Coroutine broadcastingCoroutineHandle;

    public void StartBroadcasting() {
        if (isBroadcasting || broadcastingCoroutineHandle != null) 
            return;

        isBroadcasting = true;
        broadcastingCoroutineHandle = StartCoroutine(BroadcastingCoroutine());
    }

    public void StopBroadcasting() {
        isBroadcasting = false;

        StopCoroutine(broadcastingCoroutineHandle);
    }

    IEnumerator BroadcastingCoroutine() {
        while (isBroadcasting) {
            BroadcaseSignal();

            yield return new WaitForSeconds(0.1f);
        }
    }

    void BroadcaseSignal() {
        Socket socket = new Socket(AddressFamily.InterNetwork,
        SocketType.Dgram, ProtocolType.Udp);
        socket.Bind(new IPEndPoint(IPAddress.Any, Constants.BROADCAST_TRANSMITTER_PORT));
        socket.Connect(new IPEndPoint(IPAddress.Broadcast, Constants.BROADCAST_RECEIVER_PORT));
        socket.Send(System.Text.ASCIIEncoding.ASCII.GetBytes(Constants.CONNECTION_KEY));
    }
}
