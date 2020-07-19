using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
[RequireComponent(typeof(WindowToggle))]
public class UIWindowBase : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public TextMeshProUGUI PromptText;
    public Button ConfirmButton;
    public Button CancelButton;
    public Button CloseButton;
    public Transform Content;
    private WindowToggle Window;

    // Use this for initialization
    protected virtual void Start()
    {
        Window = GetComponent<WindowToggle>();
    }


    public virtual void Open(string pTitle = "", string pPrompt = "")
    {
        if (Title != null)
        {
            Title.text = pTitle;
        }
        if (PromptText != null)
        {
            PromptText.text = pPrompt;

        }
        Window.Open();
    }
    public virtual void Close()
    {
        if (ConfirmButton != null)
        {
            ConfirmButton.onClick.RemoveAllListeners();

        }
        Window.Close();
    }

    public virtual void Toggle()
    {
        Window.Toggle();
    }
}
