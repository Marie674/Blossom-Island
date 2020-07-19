using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoldUI : MonoBehaviour
{
    public TextMeshProUGUI GoldText;
    PlayerInventory PlayerInventory;
    void OnEnable()
    {
        PlayerInventory = FindObjectOfType<PlayerInventory>();
        PlayerInventory.OnGoldChange += UpdateVisuals;
        UpdateVisuals();
    }
    void OnDisable()
    {
        if (PlayerInventory != null)
        {
            PlayerInventory.OnGoldChange -= UpdateVisuals;
        }
    }
    void UpdateVisuals()
    {
        GoldText.text = PlayerInventory.Gold.ToString("F2");
    }

}
