using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CraftingResetUI : MonoBehaviour
{

    public TextMeshProUGUI Title;

    WindowToggle Window;

    CraftingStation Station;

    // Start is called before the first frame update

    void Start()
    {
        Window = GetComponent<WindowToggle>();
    }
    public void Open(CraftingStation pStation, string pTitle)
    {
        Station = pStation;
        Title.text = pTitle;
        Window.Open();
    }

    public void Close()
    {
        Window.Close();
    }

    public void Reset()
    {
        Station.Reset();
        Close();
    }

}
