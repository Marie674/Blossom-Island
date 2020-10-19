using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace Game.Items
{
    [System.Serializable]
    public class ItemTool : ItemBase
    {
        [Header("Specific Properties")]
        public float Power;
        public int Level;
        public float UseInterval;
        public float MaxDistance;

        public float EnergyCost;

        public int MaxCharge;
        public int CurrentCharge;
        public string PlayerTrigger;

        public string ToolTrigger;
        public string CropName;
        public List<ItemBase> UsedItems = new List<ItemBase>();

        public List<ToolCursorBase> Cursors = new List<ToolCursorBase>();

        public ToolControllerBase Controller;

        public override ItemBase Clone(ItemBase itemToClone)
        {
            ItemTool itemTool = itemToClone as ItemTool;
            ItemTool newItem = base.Clone(itemToClone) as ItemTool;
            if (newItem == null)
            {
                return null;
            }
            newItem.Power = itemTool.Power;
            newItem.Level = itemTool.Level;
            newItem.UseInterval = itemTool.UseInterval;
            newItem.MaxDistance = itemTool.MaxDistance;
            newItem.EnergyCost = itemTool.EnergyCost;
            newItem.PlayerTrigger = itemTool.PlayerTrigger;
            newItem.ToolTrigger = itemTool.ToolTrigger;
            newItem.CropName = itemTool.CropName;
            newItem.MaxCharge = itemTool.MaxCharge;
            newItem.CurrentCharge = itemTool.CurrentCharge;
            newItem.UsedItems = itemTool.UsedItems;
            newItem.Cursors = itemTool.Cursors;
            if (itemTool.Controller != null)
            {
                newItem.Controller = itemTool.Controller;
            }



            return newItem;
        }
        public override void Use()
        {

        }

    }
}

