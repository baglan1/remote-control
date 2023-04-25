using System.Net;
using UnityEngine;

public class ClientNetworkManager : MonoBehaviour
{
	[SerializeField] ServerReceiver serverReceiver;
    [SerializeField] ClientBehavior clientBehavior;

    public void LookForConnection() {
        serverReceiver.SetReceivingState(true);
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
    }

    void OnDisable() {
        serverReceiver.OnSuccessfulAuthentificationEvent.RemoveListener(OnSuccessfulAuthentification);
        clientBehavior.OnConnectionEvent.RemoveListener(OnConnection);
    }
}
