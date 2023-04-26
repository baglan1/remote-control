using TMPro;
using UnityEngine;

public class CommandButtonView : MonoBehaviour
{
	[SerializeField] TMP_Text nameText;
	[SerializeField] TMP_Text descriptionText;

    public void SetText(string name, string description) {
        nameText.text = name;
        descriptionText.text = description;
    }
}
