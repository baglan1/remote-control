using System.Net;
using UnityEngine;
using UnityEngine.Events;

public class ClientNetworkManager : MonoBehaviour
{
    public UnityEvent OnConnectionEvent = new UnityEvent();
    public UnityEvent<NetworkMessage> OnMessageReceiveEvent = new UnityEvent<NetworkMessage>();

    [SerializeField] ClientBehavior clientBehavior;
    [SerializeField] Broadcaster broadcaster;

    public void StartBroadcasting() {
        broadcaster.StartBroadcasting();
    }

    public void StopBroadcasting() {
        broadcaster.StopBroadcasting();
    }

    public void SendMessage(NetworkMessage message) {
        clientBehavior.SendMessage(message);
    }

    public void Disconnect() {
        clientBehavior.Disconnect();
    }

    void OnEnable() {
        clientBehavior.OnConnectionEvent.AddListener(OnConnection);
        clientBehavior.OnMessageReceiveEvent.AddListener(OnMessageReceive);
    }

    void OnConnection() {
        OnConnectionEvent.Invoke();
    }

    void OnMessageReceive(NetworkMessage message) {
        OnMessageReceiveEvent.Invoke(message);
    }

    void OnDisable() {
        clientBehavior.OnConnectionEvent.RemoveListener(OnConnection);
        clientBehavior.OnMessageReceiveEvent.RemoveListener(OnMessageReceive);
    }
}
