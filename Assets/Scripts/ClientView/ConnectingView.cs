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
        this.gameObject.SetActive(false);
    }

    void OnDisable() {
        clientNetworkManager.StopLookingForConnection();
        clientNetworkManager.OnConnectionEvent.RemoveListener(OnConnection);
    }
}
