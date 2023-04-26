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
        if (message is CommandsListMessage listMessage) {
            commandView.ShowCommands(listMessage.Commands);
        }
    }
}
