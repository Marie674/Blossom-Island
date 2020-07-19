using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(ItemContainer))]
public class WorldItem : MonoBehaviour
{


    public uint Amount = 1;
    public Sprite DefaultItemSprite;
    public ItemContainer Container;


    void Start()
    {
        Container = GetComponent<ItemContainer>();
        UpdateItem();
    }

    void Reset()
    {
        UpdateItem();
    }

    public void SetItem(ItemBase pItem, uint pAmount = 1)
    {
        if (Container == null)
        {
            Container = GetComponent<ItemContainer>();
        }
        if (Container == null)
        {
            Debug.LogError("WorldItem needs Container component");
        }

        Container.item = ItemSystemUtility.GetItemCopy(pItem.itemName, pItem.itemType);
        Amount = pAmount;

        UpdateItem();
    }

    public void UpdateItem()
    {
        if (Container == null)
        {
            Container = GetComponent<ItemContainer>();
        }
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        if (Container.item.itemIcon != null)
        {
            sprite.sprite = Container.item.itemIcon;
        }
        else
        {
            sprite.sprite = DefaultItemSprite;
        }
        gameObject.name = "Item: " + Container.item.itemName;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            int amountAdded = other.GetComponent<PlayerInventory>().Add(Container.item, Amount);
            int amountLeft = (int)Amount - amountAdded;
            Amount = (uint)amountLeft;
            if (Amount > 0)
            {
                //inventory full
            }
            else
            {
                Destroy(this.gameObject);
            }

        }
    }

    void OnDestroy()
    {
        gameObject.SetActive(false);
    }
}
