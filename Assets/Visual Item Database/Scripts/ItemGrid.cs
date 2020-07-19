using UnityEngine;
using CreativeSpore.SuperTilemapEditor;

namespace ItemSystem
{
    [System.Serializable]
	public class ItemGrid :ItemBase, IConsumable
    {

		[SerializeField]
		public string AffectedLayer;

		[SerializeField]
		public string CheckLayer;

		[SerializeField]
		public int BrushId=0;

		[SerializeField]
		public int TileId=0;

		[SerializeField]
		public bool IsSeed=true;

		[SerializeField]
		public string CropName;

		[SerializeField, Range(0, 5)]
		public float energyCost = 0;

		[SerializeField]
		public bool usesGrid=true;

		[SerializeField, Range(0, 3)]
		public uint maxGridWidth = 3;

		[SerializeField, Range(0, 3)]
		public uint maxGridHeight = 3;

		[SerializeField]
		public ItemBase[] ItemsUsed = new ItemBase[0];

		public override void UpdateUniqueProperties(ItemBase itemToChangeTo)
		{
			ItemGrid gridItem = (ItemGrid)itemToChangeTo;
			AffectedLayer = gridItem.AffectedLayer;
			CheckLayer = gridItem.CheckLayer;
			IsSeed = gridItem.IsSeed;
			energyCost = gridItem.energyCost;
			usesGrid = gridItem.usesGrid;
			maxGridWidth = gridItem.maxGridWidth;
			maxGridHeight = gridItem.maxGridHeight;
			ItemsUsed = gridItem.ItemsUsed;
			TileId = gridItem.TileId;
			BrushId = gridItem.BrushId;
			CropName = gridItem.CropName;
		}

        public void Consume()
        {
        }
    }
}