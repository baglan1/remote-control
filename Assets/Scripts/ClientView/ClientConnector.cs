using UnityEngine;

public class ClientConnector : MonoBehaviour
{
	[SerializeField] ClientNetworkManager networkManager;
    [SerializeField] CommandView commandView;
    [SerializeField] InitialView initialView;

    void OnEnable() {
        networkManager.OnMessageReceiveEvent.AddListener(OnNetworkMessageReceive);
        commandView.SendCommandEvent.AddListener(OnViewSendCommand);
        commandView.OnDisconnectBtnClick.AddListener(OnDisconnectButtonClick);
    }

    void OnDisable() {
        networkManager.OnMessageReceiveEvent.RemoveListener(OnNetworkMessageReceive);
        commandView.SendCommandEvent.RemoveListener(OnViewSendCommand);
        commandView.OnDisconnectBtnClick.RemoveListener(OnDisconnectButtonClick);
    }

    void OnNetworkMessageReceive(NetworkMessage message) {
        if (message is CommandsListMessage listMessage) {
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

    void OnDisconnectButtonClick() {
        networkManager.Disconnect();
        initialView.gameObject.SetActive(true);
        commandView.gameObject.SetActive(false);
    }
}
