using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Items
{
    [RequireComponent(typeof(Collider2D))]
    public class WorldItem : MonoBehaviour
    {


        public uint Amount = 1;
        public Sprite DefaultItemSprite;
        public ItemBase ContainedItem;

        void Start()
        {
            UpdateItem();
        }

        void Reset()
        {
            UpdateItem();
        }

        public void SetItem(ItemBase pItem, uint pAmount = 1)
        {

            ContainedItem = pItem.Clone(pItem);
            Amount = pAmount;

            UpdateItem();
        }

        public void UpdateItem()
        {

            SpriteRenderer sprite = GetComponent<SpriteRenderer>();

            if (ContainedItem.Icon != null)
            {
                sprite.sprite = ContainedItem.Icon;
            }
            else
            {
                sprite.sprite = DefaultItemSprite;
            }
            gameObject.name = "Item: " + ContainedItem.Name;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                int amountAdded = other.GetComponent<PlayerInventory>().Add(ContainedItem, Amount);
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
}