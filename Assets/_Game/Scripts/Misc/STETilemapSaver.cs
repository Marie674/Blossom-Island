using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CreativeSpore.SuperTilemapEditor;

namespace PixelCrushers
{

	// Pixel Crushers saver component for an STE tilemap.
	[RequireComponent(typeof(STETilemap))]
    public class STETilemapSaver : Saver
    {
	
		[System.Serializable]
		public class TilemapSerializedData // From: https://creativespore.com/2018/07/03/how-to-iterate-a-tilemap/
		{
			[System.Serializable]
			public class TileData
			{
				public int gridX;
				public int gridY;
				public uint tileData;
			}
			public List<TileData> tileDataList = new List<TileData>();
		}
		 
		public static TilemapSerializedData SerializeTilemap(STETilemap tilemap)
		{
			TilemapSerializedData data = new TilemapSerializedData();
			System.Action<STETilemap, int, int, uint> action = (tmap, gridX, gridY, tileData) =>
			{
				data.tileDataList.Add(new TilemapSerializedData.TileData() { gridX = gridX, gridY = gridY, tileData = tileData});
			};
			TilemapUtils.IterateTilemapWithAction(tilemap, action);
			return data;
		}

		public static void DeserializeTilemap(TilemapSerializedData data, STETilemap tilemap)
		{
			foreach (TilemapSerializedData.TileData dataItem in data.tileDataList)
			{
				tilemap.SetTileData(dataItem.gridX, dataItem.gridY, dataItem.tileData); // NOT SURE ABOUT THIS LINE.

			}
			tilemap.UpdateMesh();

			//TilemapUtils.IterateTilemapWithAction(tilemap, action);
			//return data;
		}

		// RecordData() is called when saving. It returns a string representation of the save data.
        public override string RecordData()
        {
			STETilemap tilemap = GetComponent<STETilemap>();
			if (tilemap == null) return string.Empty;
			TilemapSerializedData data = SerializeTilemap(tilemap);
			return SaveSystem.Serialize(data);
        }

		// ApplyData() is called when loading. It receives a string representation of the
		// save data and applies it to the tilemap.
        public override void ApplyData(string s)
        {
			if (string.IsNullOrEmpty(s)) return;
		  	TilemapSerializedData data = SaveSystem.Deserialize<TilemapSerializedData>(s);
			if (data != null)
			{
				DeserializeTilemap(data, GetComponent<STETilemap>());
			}
        }
    }
}
