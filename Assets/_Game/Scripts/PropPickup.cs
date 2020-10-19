using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Items;
public class PropPickup : MonoBehaviour
{

    public ItemBase Item;
    public bool Storage = false;
    public void Hit(string pMessage)
    {
        if (pMessage == "Pickaxe")
        {
            Pickup();
        }
    }

    void Pickup()
    {
        if (Item != null)
        {

            if (Storage == true && GetComponent<StorageObject>() != null && GetComponent<StorageObject>().ContainedStacks.Count > 0)
            {
                PixelCrushers.DialogueSystem.DialogueManager.ShowAlert("Empty storage first");
                return;
            }

            ItemSpawner.Instance.SpawnItems(Item, transform.position, 1);
            Destroy(gameObject);
        }

    }
}
