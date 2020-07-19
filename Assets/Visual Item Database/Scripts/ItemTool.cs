using System;
using UnityEngine;

namespace ItemSystem
{
    [Serializable]
    public class ItemTool : ItemBase
    {
        [SerializeField, Range(1, 5), Header("Unique properties")]
        public int power = 1;

        [SerializeField, Range(0, 5)]
        public uint level = 1;

        [SerializeField, Range(0, 1)]
        public float useInterval = 0.1f;

        [SerializeField, Range(0, 2)]
        public float maxDistance = 1f;

        [SerializeField, Range(0, 5)]
        public float energyCost = 1f;

        [SerializeField]
        public float MaxCharge = 25;

        [SerializeField]
        public float CurrentCharge = 25;

        public ToolControllerBase ToolController;

        //	[SerializeField]
        //		public ToolManager.ToolActions[] actions = new ToolManager.ToolActions[0];

        [SerializeField]
        public ItemContainer[] ItemsUsed = new ItemContainer[0];

        [SerializeField]
        public ToolCursorBase[] Cursors = new ToolCursorBase[0];

        [SerializeField]
        public Color CursorColor = Color.white;

        [SerializeField]
        public bool AffectsObjects;

        [SerializeField]
        public bool AffectsTiles;

        [SerializeField]
        public string cropName;

        [SerializeField]
        public string trigger;

        [SerializeField]
        public string toolTrigger;

        public override void UpdateUniqueProperties(ItemBase itemToChangeTo)
        {
            ItemTool tool = (ItemTool)itemToChangeTo;
            energyCost = tool.energyCost;
            power = tool.power;
            maxDistance = tool.maxDistance;
            useInterval = tool.useInterval;
            //			actions = tool.actions;
            ItemsUsed = tool.ItemsUsed;
            Cursors = tool.Cursors;
            CursorColor = tool.CursorColor;
            AffectsObjects = tool.AffectsObjects;
            AffectsTiles = tool.AffectsTiles;
            ToolController = tool.ToolController;
            cropName = tool.cropName;
            MaxCharge = tool.MaxCharge;
            CurrentCharge = tool.CurrentCharge;
            trigger = tool.trigger;
            toolTrigger = tool.toolTrigger;
        }

    }
}