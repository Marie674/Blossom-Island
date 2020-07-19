using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmountInputUI : MonoBehaviour
{

    public TextMeshProUGUI Title;
    public AmountInputArrows ValueInput;
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

    public void Open(string pTitle, string pPrompt, int pMinAmount, int pMaxAmount, int pCurrentValue, string pTextToAdd = "")
    {
        Title.text = pTitle;
        PromptText.text = pPrompt;
        ValueInput.Minvalue = pMinAmount;
        ValueInput.MaxValue = pMaxAmount;
        ValueInput.CurrentValue = pCurrentValue;
        ValueInput.TextToAdd = pTextToAdd;
        ValueInput.UpdateText();
        Window.Open();
    }

    public void Toggle()
    {
        Window.Toggle();
    }
}
