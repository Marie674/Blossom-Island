using System;
using UnityEngine;
using System.Collections.Generic;

namespace ItemSystem
{
    [Serializable]
    public class ItemPlaceable: ItemBase
    {


		public GameObject[] ObjectPrefabs;

		public bool CanRotate=false;
        public List<string> ValidLevels = new List<string>();

        public override void UpdateUniqueProperties(ItemBase itemToChangeTo)
        {
			ItemPlaceable placeable = (ItemPlaceable)itemToChangeTo;
			CanRotate = placeable.CanRotate;
			ObjectPrefabs = placeable.ObjectPrefabs;
            ValidLevels = placeable.ValidLevels;

        }

    }
}