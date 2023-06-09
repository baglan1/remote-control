using UnityEngine;
using UnityEngine.Events;

public class HostNetworkManager : MonoBehaviour
{
    public UnityEvent OnConnectedEvent = new UnityEvent();
    public UnityEvent OnDisconnectEvent = new UnityEvent();
    public UnityEvent<NetworkMessage> OnMessageReceiveEvent = new UnityEvent<NetworkMessage>();

	[SerializeField] Broadcaster broadcaster;
    [SerializeField] ServerBehavior serverBehavior;

    public void StartBroadcasting() {
        broadcaster.StartBroadcasting();
    }

    public void StopBroadcasting() {
        broadcaster.StopBroadcasting();
    }

    public void SendMessage(NetworkMessage message) {
        serverBehavior.SendMessage(message);
    }

    void OnConnect() {
        OnConnectedEvent.Invoke();
    }

    void OnDisconnect() {
        OnDisconnectEvent.Invoke();
    }

    void OnMessageReceive(NetworkMessage message) {
        OnMessageReceiveEvent.Invoke(message);
    }

    void OnEnable() {
        serverBehavior.OnConnectionEvent.AddListener(OnConnect);
        serverBehavior.OnDisconnectEvent.AddListener(OnDisconnect);
        serverBehavior.OnMessageReceiveEvent.AddListener(OnMessageReceive);
    }

    void OnDisable() {
        serverBehavior.OnConnectionEvent.RemoveListener(OnConnect);
        serverBehavior.OnDisconnectEvent.RemoveListener(OnDisconnect);
        serverBehavior.OnMessageReceiveEvent.RemoveListener(OnMessageReceive);
    }
}
