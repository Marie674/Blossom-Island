using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;
using UnityEngine.UI;
using TMPro;

public class InventoryTabUI : MonoBehaviour
{
    public List<ItemType> Types = new List<ItemType>();
    public string Title;
    public TextMeshProUGUI TitleText;
}
