using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CommandView : MonoBehaviour
{
    [SerializeField] TMP_Text loadingTextComp;
    [SerializeField] RectTransform commandsRectTr;
    [SerializeField] RectTransform contentRectTr;
    [SerializeField] GameObject CommandButtonPrefab;

	public void ShowCommands(List<Command> commandList) {
        loadingTextComp.gameObject.SetActive(false);

        foreach (var comm in commandList) {
            Instantiate(CommandButtonPrefab, contentRectTr);
        }

        commandsRectTr.gameObject.SetActive(true);
    }
}
