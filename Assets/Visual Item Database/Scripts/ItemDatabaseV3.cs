using UnityEngine;

namespace ItemSystem.Database
{
    public class ItemDatabaseV3 : ScriptableObject
    {
        VIDItemListsV3 autoVidLists;
        public static readonly string dbName = "ItemDatabaseV3";

        void OnEnable()
        {
            autoVidLists = Resources.Load<VIDItemListsV3>(VIDItemListsV3.itemListsName);
        }

        /// <summary>
        /// Checks whether an item exists based on id
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool ItemExists(ItemBase item)
        {
            for (int i = 0; i < autoVidLists.usedIDs.Count; i++)
                if (autoVidLists.usedIDs[i] == item.itemID)
                    return true;

            return false;
        }

        /// <summary>
        /// Checks whether an item exists based on id
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool ItemExists(int id)
        {
            for (int i = 0; i < autoVidLists.usedIDs.Count; i++)
                if (autoVidLists.usedIDs[i] == id)
                    return true;

            return false;
        }

        /// <summary>
        /// Gets a new item ID and adds it to list of used ID's
        /// </summary>
        /// <param name="IDType"></param>
        /// <returns></returns>
        public int GetNewID(ItemType IDType)
        {
            int newID = 0;

            //Loop until you find a new id
            while (true)
            {
                newID = Random.Range(int.MinValue, int.MaxValue);

                //If this ID hasn't been used before and is NOT reserved then add and return it
                if (newID != 0 && newID != -1 && !ItemExists(newID))
                {
                    AddID(newID, IDType);
                    return newID;
                }
            }
        }

        /// <summary>
        /// Adds ID of to used id list
        /// </summary>
        /// <param name="item"></param>
        void AddID(int id, ItemType type)
        {
            autoVidLists.usedIDs.Add(id);
            autoVidLists.typesOfUsedIDs.Add(type);
        }

        /// <summary>
        /// Removes ID from id list
        /// </summary>
        /// <param name="item"></param>
        public void DeleteID(ItemBase item)
        {
            if (!ItemExists(item.itemID))
                return;

            for (int i = 0; i < autoVidLists.usedIDs.Count; i++)
            {
                if (autoVidLists.usedIDs[i] == item.itemID)
                {
                    autoVidLists.usedIDs.RemoveAt(i);
                    autoVidLists.typesOfUsedIDs.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Removes ID from id list
        /// </summary>
        /// <param name="id"></param>
        public void DeleteID(int id)
        {
            if (!ItemExists(id))
                return;

            for (int i = 0; i < autoVidLists.usedIDs.Count; i++)
            {
                if (autoVidLists.usedIDs[i] == id)
                {
                    autoVidLists.usedIDs.RemoveAt(i);
                    autoVidLists.typesOfUsedIDs.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Returns item index in respective item list if item exists otherwise returns -1
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int GetItemIndex(ItemBase item)
        {
            if (!ItemExists(item))
                return -1;

            //Check which type and search repective list
            switch (item.itemType)
            {//#VID-GIIB

				case ItemType.Generic:
					for (int i = 0; i < autoVidLists.autoGeneric.Count; i++)
					{
						if (autoVidLists.autoGeneric[i].itemID == item.itemID)
							return i;
					}
					break;

				case ItemType.Tool:
					for (int i = 0; i < autoVidLists.autoTool.Count; i++)
					{
						if (autoVidLists.autoTool[i].itemID == item.itemID)
							return i;
					}
					break;

				case ItemType.Bottle:
					for (int i = 0; i < autoVidLists.autoBottle.Count; i++)
					{
					if (autoVidLists.autoBottle[i].itemID == item.itemID)
							return i;
					}
					break;

				case ItemType.Food:
					for (int i = 0; i < autoVidLists.autoFood.Count; i++)
					{
						if (autoVidLists.autoFood[i].itemID == item.itemID)
							return i;
					}
					break;

				case ItemType.PlaceableItem:
					for (int i = 0; i < autoVidLists.autoPlaceableItem.Count; i++)
					{
						if (autoVidLists.autoPlaceableItem[i].itemID == item.itemID)
							return i;
					}
					break;

				case ItemType.GridItem:
					for (int i = 0; i < autoVidLists.autoGridItem.Count; i++)
					{
						if (autoVidLists.autoGridItem[i].itemID == item.itemID)
							return i;
					}
					break;

				case ItemType.Material:
					for (int i = 0; i < autoVidLists.autoMaterial.Count; i++)
					{
						if (autoVidLists.autoMaterial[i].itemID == item.itemID)
							return i;
					}
					break;

				case ItemType.Building:
					for (int i = 0; i < autoVidLists.autoBuilding.Count; i++)
					{
						if (autoVidLists.autoBuilding[i].itemID == item.itemID)
							return i;
					}
					break;

				case ItemType.Recipe:
					for (int i = 0; i < autoVidLists.autoRecipe.Count; i++)
					{
						if (autoVidLists.autoRecipe[i].itemID == item.itemID)
							return i;
					}
					break;
            }//#VID-GIIE

            return -1;
        }

        /// <summary>
        /// Returns item index in respective item list if item exists otherwise returns -1
        /// </summary>
        /// <param name="id">ID of item to get</param>
        /// <param name="type">Type of item to get</param>
        /// <returns></returns>
        public int GetItemIndex(int id, ItemType type)
        {
            if (!ItemExists(id))
                return -1;

            //Check which type and search repective list
            switch (type)
            {//#VID-2GIIB

				case ItemType.Generic:
					for (int i = 0; i < autoVidLists.autoGeneric.Count; i++)
					{
						if (autoVidLists.autoGeneric[i].itemID == id)
							return i;
					}
					break;

				case ItemType.Tool:
					for (int i = 0; i < autoVidLists.autoTool.Count; i++)
					{
						if (autoVidLists.autoTool[i].itemID == id)
							return i;
					}
					break;

				case ItemType.Bottle:
				for (int i = 0; i < autoVidLists.autoBottle.Count; i++)
					{
					if (autoVidLists.autoBottle[i].itemID == id)
							return i;
					}
					break;

				case ItemType.Food:
					for (int i = 0; i < autoVidLists.autoFood.Count; i++)
					{
						if (autoVidLists.autoFood[i].itemID == id)
							return i;
					}
					break;

				case ItemType.PlaceableItem:
					for (int i = 0; i < autoVidLists.autoPlaceableItem.Count; i++)
					{
						if (autoVidLists.autoPlaceableItem[i].itemID == id)
							return i;
					}
					break;

				case ItemType.GridItem:
				for (int i = 0; i < autoVidLists.autoGridItem.Count; i++)
					{
					if (autoVidLists.autoGridItem[i].itemID == id)
							return i;
					}
					break;

				case ItemType.Material:
					for (int i = 0; i < autoVidLists.autoMaterial.Count; i++)
					{
						if (autoVidLists.autoMaterial[i].itemID == id)
							return i;
					}
					break;

				case ItemType.Building:
					for (int i = 0; i < autoVidLists.autoBuilding.Count; i++)
					{
						if (autoVidLists.autoBuilding[i].itemID == id)
							return i;
					}
					break;

				case ItemType.Recipe:
					for (int i = 0; i < autoVidLists.autoRecipe.Count; i++)
					{
						if (autoVidLists.autoRecipe[i].itemID == id)
							return i;
					}
					break;
            }//#VID-2GIIE

            return -1;
        }

        public ItemBase GetItem(int id, ItemType type)
        {
            if (!ItemExists(id))
            {
                Debug.LogError(string.Format("Item of Type '{0}' and of ID '{1}' Does NOT exist", type.ToString(), id));
                return null;
            }

            switch (type)
            {//#VID-GIB

				case ItemType.Generic:
					for (int i = 0; i < autoVidLists.autoGeneric.Count; i++)
					{
						if (autoVidLists.autoGeneric[i].itemID == id)
							return autoVidLists.autoGeneric[i];
					}
					break;

				case ItemType.Tool:
					for (int i = 0; i < autoVidLists.autoTool.Count; i++)
					{
						if (autoVidLists.autoTool[i].itemID == id)
							return autoVidLists.autoTool[i];
					}
					break;

			case ItemType.Bottle:
				for (int i = 0; i < autoVidLists.autoBottle.Count; i++)
					{
					if (autoVidLists.autoBottle[i].itemID == id)
							return autoVidLists.autoBottle[i];
					}
					break;

				case ItemType.Food:
					for (int i = 0; i < autoVidLists.autoFood.Count; i++)
					{
						if (autoVidLists.autoFood[i].itemID == id)
							return autoVidLists.autoFood[i];
					}
					break;

				case ItemType.PlaceableItem:
					for (int i = 0; i < autoVidLists.autoPlaceableItem.Count; i++)
					{
						if (autoVidLists.autoPlaceableItem[i].itemID == id)
							return autoVidLists.autoPlaceableItem[i];
					}
					break;

				case ItemType.GridItem:
				for (int i = 0; i < autoVidLists.autoGridItem.Count; i++)
					{
					if (autoVidLists.autoGridItem[i].itemID == id)
						return autoVidLists.autoGridItem[i];
					}
					break;

				case ItemType.Material:
					for (int i = 0; i < autoVidLists.autoMaterial.Count; i++)
					{
						if (autoVidLists.autoMaterial[i].itemID == id)
							return autoVidLists.autoMaterial[i];
					}
					break;

				case ItemType.Building:
					for (int i = 0; i < autoVidLists.autoBuilding.Count; i++)
					{
						if (autoVidLists.autoBuilding[i].itemID == id)
							return autoVidLists.autoBuilding[i];
					}
					break;

				case ItemType.Recipe:
					for (int i = 0; i < autoVidLists.autoRecipe.Count; i++)
					{
						if (autoVidLists.autoRecipe[i].itemID == id)
							return autoVidLists.autoRecipe[i];
					}
					break;
            }//#VID-GIE

            Debug.LogError(string.Format("Item of Type '{0}' and of ID '{1}' Does NOT exist", type.ToString(), id));
            return null;
        }

        public ItemBase GetItem(string itemName, ItemType type)
        {
            switch (type)
            {//#VID-2GIB

				case ItemType.Generic:
					for (int i = 0; i < autoVidLists.autoGeneric.Count; i++)
					{
						if (autoVidLists.autoGeneric[i].itemName == itemName)
							return autoVidLists.autoGeneric[i];
					}
					break;

				case ItemType.Tool:
					for (int i = 0; i < autoVidLists.autoTool.Count; i++)
					{
						if (autoVidLists.autoTool[i].itemName == itemName)
							return autoVidLists.autoTool[i];
					}
					break;

				case ItemType.Bottle:
					for (int i = 0; i < autoVidLists.autoBottle.Count; i++)
					{
						if (autoVidLists.autoBottle[i].itemName == itemName)
							return autoVidLists.autoBottle[i];
					}
					break;

				case ItemType.Food:
					for (int i = 0; i < autoVidLists.autoFood.Count; i++)
					{
						if (autoVidLists.autoFood[i].itemName == itemName)
							return autoVidLists.autoFood[i];
					}
					break;

				case ItemType.PlaceableItem:
					for (int i = 0; i < autoVidLists.autoPlaceableItem.Count; i++)
					{
						if (autoVidLists.autoPlaceableItem[i].itemName == itemName)
							return autoVidLists.autoPlaceableItem[i];
					}
					break;

				case ItemType.GridItem:
				for (int i = 0; i < autoVidLists.autoGridItem.Count; i++)
					{
					if (autoVidLists.autoGridItem[i].itemName == itemName)
						return autoVidLists.autoGridItem[i];
					}
					break;

				case ItemType.Material:
					for (int i = 0; i < autoVidLists.autoMaterial.Count; i++)
					{
						if (autoVidLists.autoMaterial[i].itemName == itemName)
							return autoVidLists.autoMaterial[i];
					}
					break;

				case ItemType.Building:
					for (int i = 0; i < autoVidLists.autoBuilding.Count; i++)
					{
						if (autoVidLists.autoBuilding[i].itemName == itemName)
							return autoVidLists.autoBuilding[i];
					}
					break;

				case ItemType.Recipe:
					for (int i = 0; i < autoVidLists.autoRecipe.Count; i++)
					{
						if (autoVidLists.autoRecipe[i].itemName == itemName)
							return autoVidLists.autoRecipe[i];
					}
					break;
            }//#VID-2GIE

            Debug.LogError(string.Format("Item of Type '{0}' and of Name '{1}' Does NOT exist", type.ToString(), itemName));
            return null;
        }

        /// <summary>
        /// Returns a random item of the passed type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ItemBase GetRandomItem(ItemType type)
        {
            switch (type)
            {//#VID-GRIB

				case ItemType.Generic:
					if (autoVidLists.autoGeneric.Count == 0)
					{
						Debug.LogError(type.ToString() + " has no items in it");
						return null;
					}
					return autoVidLists.autoGeneric[Random.Range(0, autoVidLists.autoGeneric.Count)];

				case ItemType.Tool:
					if (autoVidLists.autoTool.Count == 0)
					{
						Debug.LogError(type.ToString() + " has no items in it");
						return null;
					}
					return autoVidLists.autoTool[Random.Range(0, autoVidLists.autoTool.Count)];

				case ItemType.Bottle:
					if (autoVidLists.autoBottle.Count == 0)
					{
						Debug.LogError(type.ToString() + " has no items in it");
						return null;
					}
					return autoVidLists.autoBottle[Random.Range(0, autoVidLists.autoBottle.Count)];

				case ItemType.Food:
					if (autoVidLists.autoFood.Count == 0)
					{
						Debug.LogError(type.ToString() + " has no items in it");
						return null;
					}
					return autoVidLists.autoFood[Random.Range(0, autoVidLists.autoFood.Count)];

				case ItemType.PlaceableItem:
					if (autoVidLists.autoPlaceableItem.Count == 0)
					{
						Debug.LogError(type.ToString() + " has no items in it");
						return null;
					}
					return autoVidLists.autoPlaceableItem[Random.Range(0, autoVidLists.autoPlaceableItem.Count)];

			case ItemType.GridItem:
				if (autoVidLists.autoGridItem.Count == 0)
					{
						Debug.LogError(type.ToString() + " has no items in it");
						return null;
					}
				return autoVidLists.autoGridItem[Random.Range(0, autoVidLists.autoGridItem.Count)];

				case ItemType.Material:
					if (autoVidLists.autoMaterial.Count == 0)
					{
						Debug.LogError(type.ToString() + " has no items in it");
						return null;
					}
					return autoVidLists.autoMaterial[Random.Range(0, autoVidLists.autoMaterial.Count)];

				case ItemType.Building:
					if (autoVidLists.autoBuilding.Count == 0)
					{
						Debug.LogError(type.ToString() + " has no items in it");
						return null;
					}
					return autoVidLists.autoBuilding[Random.Range(0, autoVidLists.autoBuilding.Count)];

				case ItemType.Recipe:
					if (autoVidLists.autoRecipe.Count == 0)
					{
						Debug.LogError(type.ToString() + " has no items in it");
						return null;
					}
					return autoVidLists.autoRecipe[Random.Range(0, autoVidLists.autoRecipe.Count)];
            }//#VID-GRIE

            Debug.LogError(type.ToString() + " type was not found, did you forget to add a check for it?");
            return null;
        }

        /// <summary>
        /// Makes sure no extra/missing IDs exist by checking the item lists against the ID list
        /// </summary>
        /// <param name="listType"></param>
        public void ValidateDatabase()
        {
            //Those id problems only occur because of undo/redo, like when something is deleted and an undo is done, the ID won't be in the database anymore,
            //so we need to readd, similary if an ID is not used at all we remove it
            AddMissingIDs();
            //RemoveExtraIDs();
        }

        void AddMissingIDs()
        {//#VID-AMIDB

			//Generic items
			for (int i = 0; i < autoVidLists.autoGeneric.Count; i++)
				if (!ItemExists(autoVidLists.autoGeneric[i].itemID))
					AddID(autoVidLists.autoGeneric[i].itemID, ItemType.Generic);

			//Tool items
			for (int i = 0; i < autoVidLists.autoTool.Count; i++)
				if (!ItemExists(autoVidLists.autoTool[i].itemID))
					AddID(autoVidLists.autoTool[i].itemID, ItemType.Tool);

			//Container items
			for (int i = 0; i < autoVidLists.autoBottle.Count; i++)
				if (!ItemExists(autoVidLists.autoBottle[i].itemID))
					AddID(autoVidLists.autoBottle[i].itemID, ItemType.Bottle);

			//Food items
			for (int i = 0; i < autoVidLists.autoFood.Count; i++)
				if (!ItemExists(autoVidLists.autoFood[i].itemID))
					AddID(autoVidLists.autoFood[i].itemID, ItemType.Food);

			//PlaceableItem items
			for (int i = 0; i < autoVidLists.autoPlaceableItem.Count; i++)
				if (!ItemExists(autoVidLists.autoPlaceableItem[i].itemID))
					AddID(autoVidLists.autoPlaceableItem[i].itemID, ItemType.PlaceableItem);

			// items
			for (int i = 0; i < autoVidLists.autoGridItem.Count; i++)
				if (!ItemExists(autoVidLists.autoGridItem[i].itemID))
					AddID(autoVidLists.autoGridItem[i].itemID, ItemType.GridItem);

			//Material items
			for (int i = 0; i < autoVidLists.autoMaterial.Count; i++)
				if (!ItemExists(autoVidLists.autoMaterial[i].itemID))
					AddID(autoVidLists.autoMaterial[i].itemID, ItemType.Material);

			//Building items
			for (int i = 0; i < autoVidLists.autoBuilding.Count; i++)
				if (!ItemExists(autoVidLists.autoBuilding[i].itemID))
					AddID(autoVidLists.autoBuilding[i].itemID, ItemType.Building);

			//Recipe items
			for (int i = 0; i < autoVidLists.autoRecipe.Count; i++)
				if (!ItemExists(autoVidLists.autoRecipe[i].itemID))
					AddID(autoVidLists.autoRecipe[i].itemID, ItemType.Recipe);
        }//#VID-AMIDE

        void RemoveExtraIDs()
        {
            bool removeKey = false;

            //Loop through all keys, find if they are used or not. If a key isn't used then remove it from used keys
            //The loop is reversed since we are iterating and removing at the same time
            for (int i = autoVidLists.usedIDs.Count - 1; i >= 0; i--)
            {
                removeKey = true;

                switch (autoVidLists.typesOfUsedIDs[i])
                {//#VID-REIDB

					//Generic
					case ItemType.Generic:
						for (int j = 0; j < autoVidLists.autoGeneric.Count; j++)
						{
							if (autoVidLists.autoGeneric[j].itemID == autoVidLists.usedIDs[i])
							{
								removeKey = false;
								break;
							}
						}
						break;

					//Tool
					case ItemType.Tool:
						for (int j = 0; j < autoVidLists.autoTool.Count; j++)
						{
							if (autoVidLists.autoTool[j].itemID == autoVidLists.usedIDs[i])
							{
								removeKey = false;
								break;
							}
						}
						break;

					//Container
					case ItemType.Bottle:
						for (int j = 0; j < autoVidLists.autoBottle.Count; j++)
						{
							if (autoVidLists.autoBottle[j].itemID == autoVidLists.usedIDs[i])
							{
								removeKey = false;
								break;
							}
						}
						break;

					//Food
					case ItemType.Food:
						for (int j = 0; j < autoVidLists.autoFood.Count; j++)
						{
							if (autoVidLists.autoFood[j].itemID == autoVidLists.usedIDs[i])
							{
								removeKey = false;
								break;
							}
						}
						break;

					//PlaceableItem
					case ItemType.PlaceableItem:
						for (int j = 0; j < autoVidLists.autoPlaceableItem.Count; j++)
						{
							if (autoVidLists.autoPlaceableItem[j].itemID == autoVidLists.usedIDs[i])
							{
								removeKey = false;
								break;
							}
						}
						break;

					//
					case ItemType.GridItem:
					for (int j = 0; j < autoVidLists.autoGridItem.Count; j++)
						{
						if (autoVidLists.autoGridItem[j].itemID == autoVidLists.usedIDs[i])
							{
								removeKey = false;
								break;
							}
						}
						break;

					//Material
					case ItemType.Material:
						for (int j = 0; j < autoVidLists.autoMaterial.Count; j++)
						{
							if (autoVidLists.autoMaterial[j].itemID == autoVidLists.usedIDs[i])
							{
								removeKey = false;
								break;
							}
						}
						break;

					//Building
					case ItemType.Building:
						for (int j = 0; j < autoVidLists.autoBuilding.Count; j++)
						{
							if (autoVidLists.autoBuilding[j].itemID == autoVidLists.usedIDs[i])
							{
								removeKey = false;
								break;
							}
						}
						break;

					//Recipe
					case ItemType.Recipe:
						for (int j = 0; j < autoVidLists.autoRecipe.Count; j++)
						{
							if (autoVidLists.autoRecipe[j].itemID == autoVidLists.usedIDs[i])
							{
								removeKey = false;
								break;
							}
						}
						break;
                }//#VID-REIDE

                //If the key isn't used in its respective list then remove it from our list of keys
                if (removeKey)
                {
                    autoVidLists.usedIDs.RemoveAt(i);
                    autoVidLists.typesOfUsedIDs.RemoveAt(i);
                }
            }
        }
    }
}
