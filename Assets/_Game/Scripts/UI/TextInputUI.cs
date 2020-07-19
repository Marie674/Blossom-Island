using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class TextInputUI : MonoBehaviour
{

    public TextMeshProUGUI Title;
    public TMP_InputField NameInput;
    public Button AcceptButton;
    public TextMeshProUGUI PromptText;
    private WindowToggle Window;

    // Use this for initialization
    void Start()
    {
        Window = GetComponent<WindowToggle>();
    }
    public void Close()
    {
        AcceptButton.onClick.RemoveAllListeners();
        Window.Close();
    }

    public void Open(string pTitle, string pPrompt)
    {
        Title.text = pTitle;
        PromptText.text = pPrompt;
        Window.Open();
    }

    public void Toggle()
    {
        Window.Toggle();
    }
}
