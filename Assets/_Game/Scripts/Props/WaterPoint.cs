using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;

public class WaterPoint : MonoBehaviour
{

    public ItemContainer WaterBottle;

    private Vector3 Center;
    void Start()
    {
        Center = transform.GetChild(0).transform.position;
        Center.z -= 0.02f;
    }

    public void Interact()
    {
        if (Toolbar.Instance.SelectedSlot.ReferencedItemStack == null)
        {
            return;
        }
        ItemBase item = Toolbar.Instance.SelectedSlot.ReferencedItemStack.ContainedItem;

        if (item.itemType == ItemType.Bottle)
        {
            UseBottle(item as ItemBottle);
        }
        if (ToolManager.Instance.CurrentToolController.GetComponent<ToolControllerWateringCan>() != null)
        {
            RefillWateringCan();
        }
        ParticleSpawner.Instance.SpawnOneShot(ParticleSpawner.ParticleTypes.Water, Center);
        AkSoundEngine.PostEvent("Play_SFX_Water_Well",gameObject);
    }

    void RefillWateringCan()
    {
        ToolManager.Instance.CurrentToolController.GetComponent<ToolControllerWateringCan>().Refill();
    }

    void UseBottle(ItemBottle pBottle)
    {

        if (pBottle.itemName != "Empty Bottle" && pBottle.itemName != "Water Bottle")
        {
            return;
        }

        if (pBottle.itemName == "Empty Bottle")
        {
            FindObjectOfType<PlayerInventory>().RemoveFromStack(Toolbar.Instance.SelectedSlot.ReferencedItemStack, 1);
            ItemSpawner.Instance.SpawnItems(WaterBottle.item, this.transform.position, 1);
        }

        pBottle.currentCharge = pBottle.maxCharge;
    }

}
