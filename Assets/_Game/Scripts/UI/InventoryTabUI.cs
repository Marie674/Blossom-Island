using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Items;
using UnityEngine.UI;
using TMPro;

public class InventoryTabUI : MonoBehaviour
{
    public List<ItemSystem.ItemTypes> Types = new List<ItemSystem.ItemTypes>();
    public string Title;
    public TextMeshProUGUI TitleText;
}
