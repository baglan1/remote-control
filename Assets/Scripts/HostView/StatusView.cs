using TMPro;
using UnityEngine;

public class StatusView : MonoBehaviour
{
	[SerializeField] TMP_Text textComp;

    [SerializeField] Color successColor;

    void Start() {
        Clear();
    }

    public void SetSuccessMessage(string message) {
        textComp.color = successColor;
        textComp.text = message;

        Invoke("Clear", 10f);
    }

    void Clear() {
        textComp.text = string.Empty;
    }
 }
