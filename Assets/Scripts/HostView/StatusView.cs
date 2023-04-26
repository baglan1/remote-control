using TMPro;
using UnityEngine;

public class StatusView : MonoBehaviour
{
	[SerializeField] TMP_Text textComp;

    [SerializeField] Color successColor;
    [SerializeField] Color errorColor;

    void Start() {
        Clear();
    }

    public void SetSuccessMessage(string message) {
        textComp.color = successColor;
        textComp.text = message;

        Invoke("Clear", 10f);
    }

    public void SetWarningMessage(string message) {
        textComp.color = errorColor;
        textComp.text = message;

        Invoke("Clear", 10f);
    }

    void Clear() {
        textComp.text = string.Empty;
    }
 }
