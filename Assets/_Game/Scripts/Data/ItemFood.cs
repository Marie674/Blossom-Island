using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace Game.Items
{
    [System.Serializable]
    public class ItemFood : ItemBase
    {
        [Header("Specific Properties")]

        public float EnergyRegen;
        public bool Edible;
        public bool BlossomFeed;

        public override ItemBase Clone(ItemBase itemToClone)
        {
            ItemFood itemFood = itemToClone as ItemFood;
            ItemFood newItem = base.Clone(itemToClone) as ItemFood;
            if (newItem == null)
            {
                return null;
            }
            newItem.EnergyRegen = itemFood.EnergyRegen;
            newItem.Edible = itemFood.Edible;
            newItem.BlossomFeed = itemFood.BlossomFeed;
            return newItem;
        }
        public override void Use()
        {

        }
    }
}

