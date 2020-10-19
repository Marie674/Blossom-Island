using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace Game.Items
{
    [System.Serializable]
    public class ItemMaterial : ItemBase
    {
        [Header("Specific Properties")]

        public float BurnTime;

        public override ItemBase Clone(ItemBase itemToClone)
        {
            ItemMaterial itemMat = itemToClone as ItemMaterial;
            ItemMaterial newItem = base.Clone(itemToClone) as ItemMaterial;
            if (newItem == null)
            {
                return null;
            }
            newItem.BurnTime = itemMat.BurnTime;
            return newItem;
        }
        public override void Use()
        {

        }

    }
}

