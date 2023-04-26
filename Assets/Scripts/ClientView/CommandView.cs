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
            var btnGo = Instantiate(CommandButtonPrefab, contentRectTr);
            var commandBtnComp = btnGo.GetComponent<CommandButtonView>();

            commandBtnComp.SetText(comm.Name, comm.Description);
        }

        commandsRectTr.gameObject.SetActive(true);
    }
}
