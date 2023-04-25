using UnityEngine;

public class ConnectingView : MonoBehaviour
{
    [SerializeField] ClientNetworkManager clientNetworkManager;
    [SerializeField] GameObject commandView;

	void OnEnable() {
        clientNetworkManager.LookForConnection();
        clientNetworkManager.OnConnectionEvent.AddListener(OnConnection);
    }

    void OnConnection() {
        commandView.SetActive(true);
    }

    void OnDisable() {
        clientNetworkManager.OnConnectionEvent.RemoveListener(OnConnection);
    }
}
