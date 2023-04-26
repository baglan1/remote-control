using System.Net;
using UnityEngine;
using UnityEngine.Events;

public class ClientNetworkManager : MonoBehaviour
{
    public UnityEvent OnConnectionEvent = new UnityEvent();
    public UnityEvent<NetworkMessage> OnMessageReceiveEvent = new UnityEvent<NetworkMessage>();

	[SerializeField] ServerReceiver serverReceiver;
    [SerializeField] ClientBehavior clientBehavior;

    public void LookForConnection() {
        serverReceiver.SetReceivingState(true);
    }

    public void StopLookingForConnection() {
        serverReceiver.SetReceivingState(false);
    }

    public void SendMessage(NetworkMessage message) {
        clientBehavior.SendMessage(message);
    }

    public void Disconnect() {
        clientBehavior.Disconnect();
    }

    void OnEnable() {
        serverReceiver.OnSuccessfulAuthentificationEvent.AddListener(OnSuccessfulAuthentification);
        clientBehavior.OnConnectionEvent.AddListener(OnConnection);
        clientBehavior.OnMessageReceiveEvent.AddListener(OnMessageReceive);
    }

    void OnSuccessfulAuthentification(IPEndPoint endpoint) {
        clientBehavior.CreateConnection(endpoint.Address.ToString());
    }

    void OnConnection() {
        serverReceiver.SetReceivingState(false);
        OnConnectionEvent.Invoke();
    }

    void OnMessageReceive(NetworkMessage message) {
        OnMessageReceiveEvent.Invoke(message);
    }

    void OnDisable() {
        serverReceiver.OnSuccessfulAuthentificationEvent.RemoveListener(OnSuccessfulAuthentification);
        clientBehavior.OnConnectionEvent.RemoveListener(OnConnection);
        clientBehavior.OnMessageReceiveEvent.RemoveListener(OnMessageReceive);
    }
}
