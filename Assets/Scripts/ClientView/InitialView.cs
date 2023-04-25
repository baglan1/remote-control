using UnityEngine;

public class InitialView : MonoBehaviour
{
    [SerializeField] GameObject connectingView;

	public void SwitchToConnecting() {
        connectingView.SetActive(true);
    }
}
