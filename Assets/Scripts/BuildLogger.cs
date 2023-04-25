using TMPro;
using UnityEngine;

public class BuildLogger : MonoBehaviour
{
	[SerializeField] TMP_Text textComp;

    public void AddText(string text) {
        textComp.text += "\n" + text;
    }
}
