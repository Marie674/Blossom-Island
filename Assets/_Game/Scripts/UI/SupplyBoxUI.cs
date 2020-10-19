using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PixelCrushers.DialogueSystem;
public class SupplyBoxUI : MonoBehaviour
{

    public VerticalLayoutGroup ItemsContainer;
    public SupplySlotUI SlotUI;
    public Button ConfirmButton;
    public ConstructionSite Site;

    public TextMeshProUGUI TitleText;

    public void Setup(List<BuildingMaterial> pMaterials, ConstructionSite pSite)
    {

        Site = pSite;
        ConfirmButton.interactable = true;

        SupplySlotUI[] children = ItemsContainer.GetComponentsInChildren<SupplySlotUI>();

        foreach (SupplySlotUI child in children)
        {
            if (child != ItemsContainer.transform)
                Destroy(child.gameObject);
        }

        foreach (BuildingMaterial material in pMaterials)
        {
            InventoryItemStack boxStack = Site.Box.FindItemStack(material.ContainedItem);
            int amount = 0;
            if (boxStack != null)
            {
                amount = boxStack.Amount;
            }
            SupplySlotUI slot = Instantiate(SlotUI, ItemsContainer.transform);
            slot.ItemIcon.sprite = material.ContainedItem.Icon;
            slot.NameText.text = material.ContainedItem.Name;
            slot.AmountText.text = amount + "/" + material.TargetAmount;
            if (boxStack == null || boxStack.Amount < material.TargetAmount)
            {
                ConfirmButton.interactable = false;
            }
        }

        GetComponent<WindowToggle>().Toggle();
    }

    public void ReadyConstruction()
    {
        Site.ConstructionReady();
        foreach (BuildingMaterial material in Site.Building.Materials)
        {
            print(material.ContainedItem.Name);
            Site.Box.RemoveItem(material.ContainedItem, (uint)material.TargetAmount);
        }
        Site.Box.DropContents();
        Site.Box.GetComponent<Usable>().enabled = false;
        GetComponent<WindowToggle>().Toggle();
    }

}
