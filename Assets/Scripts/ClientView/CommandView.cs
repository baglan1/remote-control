using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CommandView : MonoBehaviour
{
    [SerializeField] TMP_Text loadingTextComp;
    [SerializeField] RectTransform commandsRectTr;
    [SerializeField] RectTransform contentRectTr;
    [SerializeField] GameObject CommandButtonPrefab;
    [SerializeField] Button DisconnectButton;

    public UnityEvent<Command> SendCommandEvent = new UnityEvent<Command>();
    public UnityEvent OnDisconnectBtnClick {
        get {
            return DisconnectButton.onClick;
        }
    }

    Dictionary<CommandButtonView, Command> btnCommandDict;

	public void ShowCommands(List<Command> commandList) {
        loadingTextComp.gameObject.SetActive(false);

        btnCommandDict = new Dictionary<CommandButtonView, Command>();

        foreach (var comm in commandList) {
            var btnGo = Instantiate(CommandButtonPrefab, contentRectTr);
            var commandBtnComp = btnGo.GetComponent<CommandButtonView>();

            commandBtnComp.SetText(comm.Name, comm.Description);

            btnCommandDict.Add(commandBtnComp, comm);
            
            var btn = btnGo.GetComponent<Button>();
            btn.onClick.AddListener(() => OnBtnClick(btnCommandDict[commandBtnComp]));
        }

        commandsRectTr.gameObject.SetActive(true);
    }

    void OnBtnClick(Command command) {
        SendCommandEvent.Invoke(command);
    }
}
