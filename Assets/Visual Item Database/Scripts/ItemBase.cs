using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct ItemTrait
{
    public string Name;
    public string Description;
    public bool Known;
}

namespace ItemSystem
{
    [System.Serializable]
    public class ItemBase
    {
        [Header("Generic properties")]
        public string itemName = string.Empty;  //To make sure its defaulted to "", to avoid some weird button name problems
        [TextArea(1, 10)]
        public string itemDescription;

        /// <summary>Unique item ID, should ONLY be changed by the item database!!</summary>
        [HideInInspector]
        public int itemID;

        public bool consumable = false;

        public bool sellable = true;
        public float value = 0;
        public int shopMarkupPercentage = 400;

        [Range(0, 100)]
        public float weight = 1;

        public Sprite itemSprite, itemIcon;

        public bool UsableFromToolbar = false;

        //[Header("Equipping settings")]
        //public bool headEquip;
        //public bool torsoEquip;
        //public bool handEquip;
        //public bool legEquip;
        //public bool twoHanded;

        [HideInInspector/*, Space(10)*/]
        public ItemType itemType;
        //public ElementalDamage elementalDamage;

        public List<CraftingManager.ItemTags> ItemTags = new List<CraftingManager.ItemTags>();

        public List<CraftingIngredient> Ingredients = new List<CraftingIngredient>();

        [SerializeField]
        public List<ItemTrait> Traits = new List<ItemTrait>();

        /// <summary>
        /// Makes this item an instance of the passed item if found
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="type"></param>
        public void UpdateGenericProperties(ItemBase itemToChangeTo)
        {
            //Updates generic properties
            itemName = itemToChangeTo.itemName;
            itemDescription = itemToChangeTo.itemDescription;
            itemID = itemToChangeTo.itemID;
            sellable = itemToChangeTo.sellable;
            value = itemToChangeTo.value;
            weight = itemToChangeTo.weight;
            itemSprite = itemToChangeTo.itemSprite;
            itemIcon = itemToChangeTo.itemIcon;
            itemType = itemToChangeTo.itemType;
            consumable = itemToChangeTo.consumable;
            UsableFromToolbar = itemToChangeTo.UsableFromToolbar;
            ItemTags = itemToChangeTo.ItemTags;
            Ingredients = itemToChangeTo.Ingredients;
            Traits = itemToChangeTo.Traits;
            shopMarkupPercentage = itemToChangeTo.shopMarkupPercentage;
        }

        /// <summary>
        /// Updates any unique properties of the item
        /// </summary>
        /// <param name="itemToChangeTo"></param>
        public virtual void UpdateUniqueProperties(ItemBase itemToChangeTo)
        {
            //Since this is the base item, we have no unique properties to update
        }

    }

    public enum ItemType
    {//#VID-ITB
        Generic,
        Food,
        Tool,
        Bottle,
        PlaceableItem,
        Material,
        GridItem,
        Building,
		Recipe,
    };//#VID-ITE

}
