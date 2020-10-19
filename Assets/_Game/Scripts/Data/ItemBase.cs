using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


namespace Game.Items
{
    [System.Serializable]
    public class ItemBase : ScriptableObject
    {
        [Header("Generic Properties")]
        public int ID;
        public string Name = string.Empty;
        public string Description;
        [PreviewField]
        [SerializeField]
        public Sprite Icon;
        public ItemSystem.ItemTypes Type = ItemSystem.ItemTypes.Generic;
        public float Value;
        public int Markup;
        public float Weight;
        public bool Sellable;
        public bool Consumable;
        public bool UsableFromToolbar;

        [SerializeField]
        public List<ItemSystem.ItemTags> Tags = new List<ItemSystem.ItemTags>();

        public virtual void Use()
        {

        }

        public virtual ItemBase Clone(ItemBase itemToClone)
        {
            if (itemToClone == null)
            {
                return null;
            }
            ItemBase newItem = itemToClone;
            newItem.ID = itemToClone.ID;
            newItem.name = itemToClone.name;
            newItem.Description = itemToClone.Description;
            newItem.Sellable = itemToClone.Sellable;
            newItem.Consumable = itemToClone.Consumable;
            newItem.UsableFromToolbar = itemToClone.UsableFromToolbar;
            newItem.Type = itemToClone.Type;
            newItem.Tags = itemToClone.Tags;
            newItem.Weight = itemToClone.Weight;
            newItem.Value = itemToClone.Value;
            newItem.Markup = itemToClone.Markup;
            newItem.Icon = itemToClone.Icon;
            return newItem;
        }

    }
}

