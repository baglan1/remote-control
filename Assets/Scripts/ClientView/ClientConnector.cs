using UnityEngine;

public class ClientConnector : MonoBehaviour
{
	[SerializeField] ClientNetworkManager networkManager;
    [SerializeField] CommandView commandView;

    void OnEnable() {
        networkManager.OnMessageReceiveEvent.AddListener(OnNetworkMessageReceive);
    }

    void OnDisable() {
        networkManager.OnMessageReceiveEvent.RemoveListener(OnNetworkMessageReceive);
    }

    void OnNetworkMessageReceive(NetworkMessage message) {
        Debug.Log("network message received");
        if (message is CommandsListMessage listMessage) {
            Debug.Log("is Commandlist");
            commandView.ShowCommands(listMessage.Commands);
        }
    }
}
