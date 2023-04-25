using System.Net;
using UnityEngine;
using UnityEngine.Events;

public class ClientNetworkManager : MonoBehaviour
{
    public UnityEvent OnConnectionEvent = new UnityEvent();

	[SerializeField] ServerReceiver serverReceiver;
    [SerializeField] ClientBehavior clientBehavior;

    public void LookForConnection() {
        serverReceiver.SetReceivingState(true);
    }

    public void StopLookingForConnection() {
        serverReceiver.SetReceivingState(false);
    }

    void OnEnable() {
        serverReceiver.OnSuccessfulAuthentificationEvent.AddListener(OnSuccessfulAuthentification);
        clientBehavior.OnConnectionEvent.AddListener(OnConnection);
    }

    void OnSuccessfulAuthentification(IPEndPoint endpoint) {
        clientBehavior.CreateConnection(endpoint.Address.ToString());
    }

    void OnConnection() {
        serverReceiver.SetReceivingState(false);
        OnConnectionEvent.Invoke();
    }

    void OnDisable() {
        serverReceiver.OnSuccessfulAuthentificationEvent.RemoveListener(OnSuccessfulAuthentification);
        clientBehavior.OnConnectionEvent.RemoveListener(OnConnection);
    }
}
