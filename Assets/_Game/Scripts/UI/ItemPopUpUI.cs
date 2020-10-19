using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Items;
using UnityEngine.UI;
using TMPro;
public class ItemPopUpUI : MonoBehaviour
{

    public TextMeshProUGUI Description;
    public Image ItemIcon;

    public void Open(ItemBase pItem)
    {


        ItemIcon.sprite = pItem.Icon;
        Description.text = "<b><align=center>" + pItem.Name + "</align></b>";
        Description.text += "\n \n" + pItem.Description;
        GetComponent<WindowToggle>().Open();


    }

    public void Close()
    {
        GetComponent<WindowToggle>().Close();
    }

}
