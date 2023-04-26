using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainConnector : MonoBehaviour
{
	[SerializeField] CounterView counterView;
    [SerializeField] StatusView statusView;
    [SerializeField] HostNetworkManager networkManager;

    List<Command> commandList;

    void Start() {
        networkManager.StartBroadcasting();

        CreateCommands();
    }

    void OnEnable() {
        networkManager.OnConnectedEvent.AddListener(OnConnection);
        networkManager.OnDisconnectEvent.AddListener(OnDisconnect);
        networkManager.OnMessageReceiveEvent.AddListener(OnNetworkMessageReceive);
    }

    void OnDisable() {
        networkManager.OnConnectedEvent.RemoveListener(OnConnection);
        networkManager.OnDisconnectEvent.RemoveListener(OnDisconnect);
        networkManager.OnMessageReceiveEvent.RemoveListener(OnNetworkMessageReceive);
    }

    void OnConnection() {
        networkManager.StopBroadcasting();
        statusView.SetSuccessMessage("Connected a new device.");

        Invoke("SendCommandList", 1f);
    }

    void OnDisconnect() {
        networkManager.StartBroadcasting();
        statusView.SetWarningMessage("Device is disconnected.");
    }

    void SendCommandList() {
        var commandListMsg = new CommandsListMessage(commandList);

        networkManager.SendMessage(commandListMsg);
    }

    void CreateCommands() {
        commandList = new List<Command>();

        var incrCommand = new Command("Increment", "Increments the number");
        incrCommand.SetAction(() => counterView.Increment());
        commandList.Add(incrCommand);

        var decrCommand = new Command("Decrement", "Decrements the number");
        decrCommand.SetAction(() => counterView.Decrement());
        commandList.Add(decrCommand);
    }  

    void OnNetworkMessageReceive(NetworkMessage message) {
        if (message is ExecuteCommandMessage executeCommandMessage) {
            var comm = commandList.FirstOrDefault(x => x.Name == executeCommandMessage.Name);

            if (comm is null) {
                Debug.Log($"Command {executeCommandMessage.Name} not found.");
                return;
            }

            comm.CallAction();
        }
    }  
}
