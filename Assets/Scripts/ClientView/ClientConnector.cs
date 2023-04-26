using UnityEngine;

public class ClientConnector : MonoBehaviour
{
	[SerializeField] ClientNetworkManager networkManager;
    [SerializeField] CommandView commandView;

    void OnEnable() {
        networkManager.OnMessageReceiveEvent.AddListener(OnNetworkMessageReceive);
        commandView.SendCommandEvent.AddListener(OnViewSendCommand);
    }

    void OnDisable() {
        networkManager.OnMessageReceiveEvent.RemoveListener(OnNetworkMessageReceive);
        commandView.SendCommandEvent.RemoveListener(OnViewSendCommand);
    }

    void OnNetworkMessageReceive(NetworkMessage message) {
        Debug.Log("network message received");
        if (message is CommandsListMessage listMessage) {
            Debug.Log("is Commandlist");
            commandView.ShowCommands(listMessage.Commands);
        }
    }

    void OnViewSendCommand(Command command) {
        SendCommand(command.Name);
    }

    void SendCommand(string name) {
        var commandMsg = new ExecuteCommandMessage(name);

        networkManager.SendMessage(commandMsg);
    }
}
