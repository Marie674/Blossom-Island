using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers;
using UnityEngine.UI;
using TMPro;
using PixelCrushers.DialogueSystem;
public class NewGame : MonoBehaviour
{
    public string StartingSceneName;
    public GameObject NamePanel;
    public GameObject CharacterPanel;
    public GameObject ConfirmPanel;

    public TMP_InputField NameInput;

    public void ShowCharacterPanel()
    {
        if (NamePanel != null)
        {
            NamePanel.gameObject.SetActive(false);
        }
        if (ConfirmPanel != null)
        {
            ConfirmPanel.gameObject.SetActive(false);
        }
        if (CharacterPanel != null)
        {
            CharacterPanel.SetActive(true);
        }
    }
    public void ShowNamePanel()
    {
        if (ConfirmPanel != null)
        {
            ConfirmPanel.gameObject.SetActive(false);
        }
        if (CharacterPanel != null)
        {
            CharacterPanel.SetActive(false);
        }
        if (NamePanel != null)
        {
            NamePanel.gameObject.SetActive(true);
        }
    }

    public void ShowConfirmPanel()
    {

        if (CharacterPanel != null)
        {
            CharacterPanel.SetActive(false);
        }
        if (NamePanel != null)
        {
            NamePanel.gameObject.SetActive(false);
        }
        if (ConfirmPanel != null)
        {
            ConfirmPanel.gameObject.SetActive(true);
        }
    }

    public void SetPlayerName()
    {
        if (NameInput.text != string.Empty)
        {
            DialogueLua.SetVariable("PlayerName", NameInput.text);

        }
    }
    public void LoadHome()
    {

        GameManager.Instance.StartGame(StartingSceneName);
    }

}
