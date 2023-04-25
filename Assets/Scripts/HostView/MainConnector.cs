using System.Collections.Generic;
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
    }

    void OnDisable() {
        networkManager.OnConnectedEvent.RemoveListener(OnConnection);
    }

    void OnConnection() {
        networkManager.StopBroadcasting();
        statusView.SetSuccessMessage("Connected a new device.");
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
}
