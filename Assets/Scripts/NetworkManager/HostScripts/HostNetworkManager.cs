using System.Net;
using UnityEngine;
using UnityEngine.Events;

public class HostNetworkManager : MonoBehaviour
{
    public UnityEvent OnConnectedEvent = new UnityEvent();
    public UnityEvent OnDisconnectEvent = new UnityEvent();
    public UnityEvent<NetworkMessage> OnMessageReceiveEvent = new UnityEvent<NetworkMessage>();

	[SerializeField] ServerReceiver serverReceiver;
    [SerializeField] ServerBehavior serverBehavior;

    public void SetReceivingState(bool flag) {
        serverReceiver.SetReceivingState(flag);
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
        serverReceiver.OnSuccessfulAuthentificationEvent.AddListener(OnSuccessfulAuthentification);
        serverBehavior.OnConnectionEvent.AddListener(OnConnect);
        serverBehavior.OnDisconnectEvent.AddListener(OnDisconnect);
        serverBehavior.OnMessageReceiveEvent.AddListener(OnMessageReceive);
    }

    void OnDisable() {
        serverReceiver.OnSuccessfulAuthentificationEvent.RemoveListener(OnSuccessfulAuthentification);
        serverBehavior.OnConnectionEvent.RemoveListener(OnConnect);
        serverBehavior.OnDisconnectEvent.RemoveListener(OnDisconnect);
        serverBehavior.OnMessageReceiveEvent.RemoveListener(OnMessageReceive);
    }

    void OnSuccessfulAuthentification(IPEndPoint endPoint) {
        serverBehavior.CreateConnection(endPoint.Address.ToString());
        Debug.Log($"Succesfull auth from {endPoint.Address.ToString()}");
    }
}
