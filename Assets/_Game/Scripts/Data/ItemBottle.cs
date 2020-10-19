using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace Game.Items
{
    [System.Serializable]
    public class ItemBottle : ItemBase
    {
        [Header("Specific Properties")]
        public ItemFood HeldLiquid;
        public int MaxCharge;
        public int CurrentCharge;

        public override ItemBase Clone(ItemBase itemToClone)
        {
            ItemBottle itemBottle = itemToClone as ItemBottle;
            ItemBottle newItem = base.Clone(itemToClone) as ItemBottle;
            if (newItem == null)
            {
                return null;
            }
            if (itemBottle.HeldLiquid != null)
            {
                newItem.HeldLiquid = itemBottle.HeldLiquid;
            }
            newItem.MaxCharge = itemBottle.MaxCharge;
            newItem.CurrentCharge = itemBottle.CurrentCharge;
            return newItem;
        }
        public override void Use()
        {

        }

    }
}

