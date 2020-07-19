﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BuyUI : MonoBehaviour
{

    public TextMeshProUGUI Title;

    public Image ItemIcon;
    public TextMeshProUGUI Description;

    public int Min;
    public int Max;
    public StoreItemSlot Slot;

    public Button ConfirmButton;


    public void Open(StoreItemSlot pSlot, int pBuyableAmount)
    {
        Slot = pSlot;
        Title.text = "Buy " + Slot.CurrentItem.item.itemName + "?";
        ItemIcon.sprite = Slot.CurrentItem.item.itemIcon;
        Description.text = Slot.CurrentItem.item.itemDescription;
        Description.text += "\n \n" + "Price: " + Slot.CurrentItem.item.value + " each.";
        Min = 0;
        Max = pBuyableAmount;
        if (GameManager.Instance.Player.GetComponent<PlayerInventory>().Gold < Slot.CurrentItem.item.value)
        {
            ConfirmButton.interactable = false;
        }
        else
        {
            ConfirmButton.interactable = true;
        }
        GetComponent<WindowToggle>().Open();
    }

    public void Accept()
    {
        GetComponent<WindowToggle>().Close();
        AmountInputUI inputUI = FindObjectOfType<AmountInputUI>();
        inputUI.PromptText.text = "Buy how many?";
        inputUI.Title.text = "Buy";
        inputUI.AcceptButton.onClick.AddListener(delegate { Slot.Buy((int)inputUI.ValueInput.CurrentValue); inputUI.Close(); });
        inputUI.Open("Buy", "Buy how many?", Min, Max, 1);
    }



    public void Cancel()
    {
        GetComponent<WindowToggle>().Close();
    }
}
