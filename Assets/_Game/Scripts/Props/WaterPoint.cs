using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Items;

public class WaterPoint : MonoBehaviour
{

    public ItemBottle WaterBottle;

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

        if (item.Type == ItemSystem.ItemTypes.Bottle)
        {
            RefillBottle(item as ItemBottle);
        }
        if (ToolManager.Instance.CurrentToolController.GetComponent<ToolControllerWateringCan>() != null)
        {
            RefillWateringCan();
        }
        ParticleSpawner.Instance.SpawnOneShot(ParticleSpawner.ParticleTypes.Water, Center);
        AkSoundEngine.PostEvent("Play_SFX_Water_Well", gameObject);
    }

    void RefillWateringCan()
    {
        ToolManager.Instance.CurrentToolController.GetComponent<ToolControllerWateringCan>().Refill();
    }

    void RefillBottle(ItemBottle pBottle)
    {

        if (pBottle.Name != "Empty Bottle" && pBottle.Name != "Water Bottle")
        {
            return;
        }

        if (pBottle.Name == "Empty Bottle")
        {
            FindObjectOfType<PlayerInventory>().RemoveFromStack(Toolbar.Instance.SelectedSlot.ReferencedItemStack, 1);
            ItemSpawner.Instance.SpawnItems(WaterBottle, this.transform.position, 1);
        }

        pBottle.CurrentCharge = pBottle.MaxCharge;
    }

}
