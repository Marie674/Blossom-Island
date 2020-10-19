using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Items;
using TMPro;
public class CraftingChoiceUI : MonoBehaviour
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

    public void ShowRecipes()
    {
        Close();
        FindObjectOfType<CraftingStationRecipesUI>().Open(Station);

    }
    public void ShowInput()
    {
        Close();
        FindObjectOfType<CraftingInputUI>().Open(Station);

    }
}
