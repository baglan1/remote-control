using UnityEngine;

public class MainConnector : MonoBehaviour
{
	[SerializeField] CounterView counterView;
    [SerializeField] StatusView statusView;
    [SerializeField] HostNetworkManager networkManager;

    void Start() {
        networkManager.StartBroadcasting();
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
}
