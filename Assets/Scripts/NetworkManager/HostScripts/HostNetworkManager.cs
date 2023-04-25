using UnityEngine;
using UnityEngine.Events;

public class HostNetworkManager : MonoBehaviour
{
    public UnityEvent OnConnectedEvent = new UnityEvent();

	[SerializeField] Broadcaster broadcaster;
    [SerializeField] ServerBehavior serverBehavior;

    public void StartBroadcasting() {
        broadcaster.StartBroadcasting();
    }

    public void StopBroadcasting() {
        broadcaster.StopBroadcasting();
    }

    public void SendMessage(NetworkMessage message) {
        serverBehavior.SendMessage(message);
    }

    void OnConnect() {
        OnConnectedEvent.Invoke();
    }

    void OnEnable() {
        serverBehavior.OnConnectionEvent.AddListener(OnConnect);
    }

    void OnDisable() {
        serverBehavior.OnConnectionEvent.RemoveListener(OnConnect);
    }
}
