using UnityEngine;
using ItemSystem.Database;
using System.Collections.Generic;

namespace ItemSystem
{
    [DisallowMultipleComponent]
    public partial class ItemSystemUtility : MonoBehaviour
    {
        public static ItemDatabaseV3 itemDatabase { get; private set; }
        public static VIDItemListsV3 vidLists { get; private set; }

        void Awake()
        {
            LoadItemDatabase();
        }

        /// <summary>
        /// Loads the item database if it isn't already loaded (Called automatically on Awake)
        /// </summary>
        public static void LoadItemDatabase()
        {
            if (!itemDatabase)
                itemDatabase = Resources.Load<ItemDatabaseV3>(ItemDatabaseV3.dbName);

            if (!vidLists)
                vidLists = Resources.Load<VIDItemListsV3>(VIDItemListsV3.itemListsName);
        }

        /// <summary>
        /// Returns a copy of the wanted item(so the one in the database doesn't change). Returns NULL if the item isn't found
        /// </summary>
        /// <param name="id">ID of the wanted item</param>
        /// <param name="type">Type of the wanted item</param>
        public static ItemBase GetItemCopy(int id, ItemType type)
        {
            ItemBase item = null;

            //Instantiate item of proper type
            switch (type)
            {//#VID-GICB
				case ItemType.Generic:
					item = new ItemBase();
					break;
				case ItemType.Tool:
					item = new ItemTool();
					break;
				case ItemType.Bottle:
					item = new ItemBottle();
					break;
				case ItemType.Food:
					item = new ItemFood();
					break;
				case ItemType.PlaceableItem:
					item = new ItemPlaceable();
					break;
				case ItemType.GridItem:
					item = new ItemGrid();
					break;
				case ItemType.Material:
					item = new ItemMaterial();
					break;
				case ItemType.Building:
					item = new ItemBuilding();
					break;
				case ItemType.Recipe:
					item = new ItemRecipe();
					break;
            }//#VID-GICE

            //This is because in game we don't need this check, since we assume that the ItemSystemUtility is attached to a gameobject
#if UNITY_EDITOR
            if (!itemDatabase)
                LoadItemDatabase();
#endif

            //Get the item we want to become
            ItemBase itemToCopy = itemDatabase.GetItem(id, type);

            if (itemToCopy == null)
                return null;

            //Become that item
            item.UpdateGenericProperties(itemToCopy);
            item.UpdateUniqueProperties(itemToCopy);
            return item;
        }

        /// <summary>
        /// Returns a copy of the wanted item(so the one in the database doesn't change). Returns NULL if the item isn't found
        /// </summary>
        /// <param name="itemName">Name of the wanted item</param>
        /// <param name="type">Type of the wanted item</param>
        public static ItemBase GetItemCopy(string itemName, ItemType type)
        {
            ItemBase item = null;

            //Instantiate item of proper type
            switch (type)
            {//#VID-2GICB
				case ItemType.Generic:
					item = new ItemBase();
					break;
				case ItemType.Tool:
					item = new ItemTool();
					break;
				case ItemType.Bottle:
					item = new ItemBottle();
					break;
				case ItemType.Food:
					item = new ItemFood();
					break;
				case ItemType.PlaceableItem:
					item = new ItemPlaceable();
					break;
				case ItemType.GridItem:
					item = new ItemGrid();
					break;
				case ItemType.Material:
					item = new ItemMaterial();
					break;
				case ItemType.Building:
					item = new ItemBuilding();
					break;
				case ItemType.Recipe:
					item = new ItemRecipe();
					break;
            }//#VID-2GICE

#if UNITY_EDITOR
            if (!itemDatabase)
                LoadItemDatabase();
#endif

            //Get the item we want to become
            ItemBase itemToCopy = itemDatabase.GetItem(itemName, type);

            if (itemToCopy == null)
                return null;

            item.UpdateGenericProperties(itemToCopy);
            item.UpdateUniqueProperties(itemToCopy);

            return item;
        }

        /// <summary>
        /// Returns a copy of a random item of the passed type
        /// </summary>
        /// <param name="type">Type of the random item</param>
        /// <returns></returns>
        public static ItemBase GetRandomItemCopy(ItemType type)
        {
            ItemBase item = null;

            //Instantiate item of proper type
            switch (type)
            {//#VID-GRICB
				case ItemType.Generic:
					item = new ItemBase();
					break;
				case ItemType.Tool:
					item = new ItemTool();
					break;
				case ItemType.Bottle:
					item = new ItemBottle();
					break;
				case ItemType.Food:
					item = new ItemFood();
					break;
				case ItemType.PlaceableItem:
					item = new ItemPlaceable();
					break;
				case ItemType.GridItem:
					item = new ItemGrid();
					break;
				case ItemType.Material:
					item = new ItemMaterial();
					break;
				case ItemType.Building:
					item = new ItemBuilding();
					break;
				case ItemType.Recipe:
					item = new ItemRecipe();
					break;
            }//#VID-GRICE

#if UNITY_EDITOR
            if (!itemDatabase)
                LoadItemDatabase();
#endif

            //Get the item we want to become
            ItemBase itemToCopy = itemDatabase.GetRandomItem(type);

            if (itemToCopy == null)
                return null;

            item.UpdateGenericProperties(itemToCopy);
            item.UpdateUniqueProperties(itemToCopy);

            return item;
        }

        /// <summary>
        /// Returns a copy of a random item from the passed subtype(use the subtype enums and convert the wanted subtype to a string)
        /// </summary>
        /// <param name="subtypeName">Name of the subtype</param>
        /// <returns></returns>
        public static ItemBase GetRandomItemCopy(string subtypeName, ItemType mainType)
        {
            ItemBase item = null;

            switch (mainType)
            {//#VID-2GRICB
				case ItemType.Generic:
					item = new ItemBase();
					break;
				case ItemType.Tool:
					item = new ItemTool();
					break;
				case ItemType.Bottle:
					item = new ItemBottle();
					break;
				case ItemType.Food:
					item = new ItemFood();
					break;
				case ItemType.PlaceableItem:
					item = new ItemPlaceable();
					break;
				case ItemType.GridItem:
					item = new ItemGrid();
					break;
				case ItemType.Material:
					item = new ItemMaterial();
					break;
				case ItemType.Building:
					item = new ItemBuilding();
					break;
				case ItemType.Recipe:
					item = new ItemRecipe();
					break;
            }//#VID-2GRICE

#if UNITY_EDITOR
            if (!itemDatabase)
                LoadItemDatabase();
#endif

            for (int i = 0; i < vidLists.subtypes.Count; i++)
            {
                //Find the subType and get a random item id from that subtype
                if (vidLists.subtypes[i].type == mainType && vidLists.subtypes[i].name == subtypeName)
                {
                    ItemBase c = itemDatabase.GetItem(vidLists.subtypes[i].itemIDs[Random.Range(0, vidLists.subtypes[i].itemIDs.Count)], vidLists.subtypes[i].type);

                    item.UpdateGenericProperties(c);
                    item.UpdateUniqueProperties(c);
                    return item;
                }
            }

            return item;
        }

        /// <summary>
        /// Returns a copy of the wanted item(so the one in the database doesn't change). Returns NULL if the item isn't found
        /// </summary>
        /// <param name="id">ID of the wanted item</param>
        /// <param name="type">Type of the wanted item</param>
        public static T GetItemCopy<T>(int id, ItemType type) where T : ItemBase, new()
        {
            T item = new T();

#if UNITY_EDITOR
            if (!itemDatabase)
                LoadItemDatabase();
#endif
            ItemBase itemToCopy = itemDatabase.GetItem(id, type);

            item.UpdateGenericProperties(itemToCopy);
            item.UpdateUniqueProperties(itemToCopy);

            return item;
        }

        /// <summary>
        /// Returns a copy of the wanted item(so the one in the database doesn't change). Returns NULL if the item isn't found
        /// </summary>
        /// <param name="itemName">Name of the wanted item</param>
        /// <param name="type">Type of the wanted item</param>
        public static T GetItemCopy<T>(string itemName, ItemType type) where T : ItemBase, new()
        {
            T item = new T();

#if UNITY_EDITOR
            if (!itemDatabase)
                LoadItemDatabase();
#endif
            ItemBase itemToCopy = itemDatabase.GetItem(itemName, type);

            item.UpdateGenericProperties(itemToCopy);
            item.UpdateUniqueProperties(itemToCopy);

            return item;
        }

        /// <summary>
        /// Returns a copy of a random item of the passed type
        /// </summary>
        /// <param name="type">Type of the random item</param>
        /// <returns></returns>
        public static T GetRandomItemCopy<T>(ItemType type) where T : ItemBase, new()
        {
            T item = new T();

#if UNITY_EDITOR
            if (!itemDatabase)
                LoadItemDatabase();
#endif

            //Get the item we want to become
            ItemBase itemToCopy = itemDatabase.GetRandomItem(type);

            if (itemToCopy == null)
                return null;

            item.UpdateGenericProperties(itemToCopy);
            item.UpdateUniqueProperties(itemToCopy);

            return item;
        }

        /// <summary>
        /// Returns a copy of a random item from the passed subtype(use the subtype enums and convert the wanted subtype to a string)
        /// </summary>
        /// <param name="subtypeName">Name of the subtype</param>
        /// <returns></returns>
        public static T GetRandomItemCopy<T>(string subtypeName, ItemType mainType) where T : ItemBase, new()
        {
            T item = new T();

#if UNITY_EDITOR
            if (!itemDatabase)
                LoadItemDatabase();
#endif
            for (int i = 0; i < vidLists.subtypes.Count; i++)
            {
                //Find the subType and get a random item id from that subtype
                if (vidLists.subtypes[i].type == mainType && vidLists.subtypes[i].name == subtypeName)
                {
                    ItemBase c = itemDatabase.GetItem(vidLists.subtypes[i].itemIDs[Random.Range(0, vidLists.subtypes[i].itemIDs.Count)], vidLists.subtypes[i].type);

                    item.UpdateGenericProperties(c);
                    item.UpdateUniqueProperties(c);
                    return item;
                }
            }

            return item;
        }

        /// <summary>
        /// Returns a list containing instances of all the items in the wanted type. (This is possibly a slow operation, take care when using)
        /// </summary>
        /// <typeparam name="T">Class used by the wanted type</typeparam>
        /// <param name="type">The item type</param>
        /// <returns></returns>
        public static List<T> GetAllTypeItems<T>(ItemType type) where T : ItemBase, new()
        {
            List<T> items = new List<T>();
            T instance = null;

#if UNITY_EDITOR
            if (!itemDatabase)
                LoadItemDatabase();
#endif

            //Iterate directly over all the items of the wanted type, make instances of them and add them to the list
            switch (type)
            {//#VID-GATIB
				case ItemType.Generic:
					for (int i = 0; i < vidLists.autoGeneric.Count; i++)
					{
						instance = new T();
						instance.UpdateGenericProperties(vidLists.autoGeneric[i]);
						instance.UpdateUniqueProperties(vidLists.autoGeneric[i]);
						items.Add(instance);
					}
					break;
				case ItemType.Tool:
					for (int i = 0; i < vidLists.autoTool.Count; i++)
					{
						instance = new T();
						instance.UpdateGenericProperties(vidLists.autoTool[i]);
						instance.UpdateUniqueProperties(vidLists.autoTool[i]);
						items.Add(instance);
					}
					break;
				case ItemType.Bottle:
					for (int i = 0; i < vidLists.autoBottle.Count; i++)
					{
						instance = new T();
						instance.UpdateGenericProperties(vidLists.autoBottle[i]);
						instance.UpdateUniqueProperties(vidLists.autoBottle[i]);
						items.Add(instance);
					}
					break;
				case ItemType.Food:
					for (int i = 0; i < vidLists.autoFood.Count; i++)
					{
						instance = new T();
						instance.UpdateGenericProperties(vidLists.autoFood[i]);
						instance.UpdateUniqueProperties(vidLists.autoFood[i]);
						items.Add(instance);
					}
					break;
				case ItemType.PlaceableItem:
					for (int i = 0; i < vidLists.autoPlaceableItem.Count; i++)
					{
						instance = new T();
						instance.UpdateGenericProperties(vidLists.autoPlaceableItem[i]);
						instance.UpdateUniqueProperties(vidLists.autoPlaceableItem[i]);
						items.Add(instance);
					}
					break;
			case ItemType.GridItem:
					for (int i = 0; i < vidLists.autoGridItem.Count; i++)
					{
						instance = new T();
					instance.UpdateGenericProperties(vidLists.autoGridItem[i]);
					instance.UpdateUniqueProperties(vidLists.autoGridItem[i]);
						items.Add(instance);
					}
					break;
				case ItemType.Material:
					for (int i = 0; i < vidLists.autoMaterial.Count; i++)
					{
						instance = new T();
						instance.UpdateGenericProperties(vidLists.autoMaterial[i]);
						instance.UpdateUniqueProperties(vidLists.autoMaterial[i]);
						items.Add(instance);
					}
					break;
				case ItemType.Building:
					for (int i = 0; i < vidLists.autoBuilding.Count; i++)
					{
						instance = new T();
						instance.UpdateGenericProperties(vidLists.autoBuilding[i]);
						instance.UpdateUniqueProperties(vidLists.autoBuilding[i]);
						items.Add(instance);
					}
					break;
				case ItemType.Recipe:
					for (int i = 0; i < vidLists.autoRecipe.Count; i++)
					{
						instance = new T();
						instance.UpdateGenericProperties(vidLists.autoRecipe[i]);
						instance.UpdateUniqueProperties(vidLists.autoRecipe[i]);
						items.Add(instance);
					}
					break;
            }//#VID-GATIE

            //Finally we return the wanted list, no casting required ;)
            return items;
        }

        /// <summary>
        /// Returns a list containing instances of all the items in the wanted subtype. (This is possibly a slow operation, take care when using)
        /// </summary>
        /// <typeparam name="T">Class used by the wanted type</typeparam>
        /// <param name="subtypeName">Name of the subtype to use</param>
        /// <param name="mainType">The main type of the wanted subtype</param>
        /// <returns></returns>
        public static List<T> GetAllSubtypeItems<T>(string subtypeName, ItemType mainType) where T : ItemBase, new()
        {
            List<T> items = new List<T>();
            int subtypeIndex = -1;
            T instance = null;

#if UNITY_EDITOR
            if (!itemDatabase)
                LoadItemDatabase();
#endif

            //Get the index of the wanted subtype
            for (int i = 0; i < vidLists.subtypes.Count; i++)
            {
                if (vidLists.subtypes[i].type == mainType && vidLists.subtypes[i].name == subtypeName)
                {
                    subtypeIndex = i;
                    break;
                }
            }

            //If didn't find subtype or subtype is empty then stop here and return an empty list
            if (subtypeIndex == -1 || vidLists.subtypes[subtypeIndex].itemIDs.Count == 0)
                return items;

            //Check each item to see whether it is wanted, if it is make an instance of it and add it to item list.
            //Each time an item is added check to see if we got all the items we wanted, if we have everything return since there is no point in further iteration.
            switch (mainType)
            {//#VID-GASIB
				case ItemType.Generic:
					for (int i = 0; i < vidLists.autoGeneric.Count; i++)
					{
						if (vidLists.subtypes[subtypeIndex].itemIDs.Contains(vidLists.autoGeneric[i].itemID))
						{
							instance = new T();
							instance.UpdateGenericProperties(vidLists.autoGeneric[i]);
							instance.UpdateUniqueProperties(vidLists.autoGeneric[i]);
							items.Add(instance);
							
							if (items.Count == vidLists.subtypes[subtypeIndex].itemIDs.Count)
								return items;
						}
					}
					break;
				case ItemType.Tool:
					for (int i = 0; i < vidLists.autoTool.Count; i++)
					{
						if (vidLists.subtypes[subtypeIndex].itemIDs.Contains(vidLists.autoTool[i].itemID))
						{
							instance = new T();
							instance.UpdateGenericProperties(vidLists.autoTool[i]);
							instance.UpdateUniqueProperties(vidLists.autoTool[i]);
							items.Add(instance);
							
							if (items.Count == vidLists.subtypes[subtypeIndex].itemIDs.Count)
								return items;
						}
					}
					break;
				case ItemType.Bottle:
					for (int i = 0; i < vidLists.autoBottle.Count; i++)
					{
						if (vidLists.subtypes[subtypeIndex].itemIDs.Contains(vidLists.autoBottle[i].itemID))
						{
							instance = new T();
							instance.UpdateGenericProperties(vidLists.autoBottle[i]);
							instance.UpdateUniqueProperties(vidLists.autoBottle[i]);
							items.Add(instance);
							
							if (items.Count == vidLists.subtypes[subtypeIndex].itemIDs.Count)
								return items;
						}
					}
					break;
				case ItemType.Food:
					for (int i = 0; i < vidLists.autoFood.Count; i++)
					{
						if (vidLists.subtypes[subtypeIndex].itemIDs.Contains(vidLists.autoFood[i].itemID))
						{
							instance = new T();
							instance.UpdateGenericProperties(vidLists.autoFood[i]);
							instance.UpdateUniqueProperties(vidLists.autoFood[i]);
							items.Add(instance);
							
							if (items.Count == vidLists.subtypes[subtypeIndex].itemIDs.Count)
								return items;
						}
					}
					break;
				case ItemType.PlaceableItem:
					for (int i = 0; i < vidLists.autoPlaceableItem.Count; i++)
					{
						if (vidLists.subtypes[subtypeIndex].itemIDs.Contains(vidLists.autoPlaceableItem[i].itemID))
						{
							instance = new T();
							instance.UpdateGenericProperties(vidLists.autoPlaceableItem[i]);
							instance.UpdateUniqueProperties(vidLists.autoPlaceableItem[i]);
							items.Add(instance);
							
							if (items.Count == vidLists.subtypes[subtypeIndex].itemIDs.Count)
								return items;
						}
					}
					break;
			case ItemType.GridItem:
				for (int i = 0; i < vidLists.autoGridItem.Count; i++)
					{
					if (vidLists.subtypes[subtypeIndex].itemIDs.Contains(vidLists.autoGridItem[i].itemID))
						{
							instance = new T();
						instance.UpdateGenericProperties(vidLists.autoGridItem[i]);
						instance.UpdateUniqueProperties(vidLists.autoGridItem[i]);
							items.Add(instance);
							
							if (items.Count == vidLists.subtypes[subtypeIndex].itemIDs.Count)
								return items;
						}
					}
					break;
				case ItemType.Material:
					for (int i = 0; i < vidLists.autoMaterial.Count; i++)
					{
						if (vidLists.subtypes[subtypeIndex].itemIDs.Contains(vidLists.autoMaterial[i].itemID))
						{
							instance = new T();
							instance.UpdateGenericProperties(vidLists.autoMaterial[i]);
							instance.UpdateUniqueProperties(vidLists.autoMaterial[i]);
							items.Add(instance);
							
							if (items.Count == vidLists.subtypes[subtypeIndex].itemIDs.Count)
								return items;
						}
					}
					break;
				case ItemType.Building:
					for (int i = 0; i < vidLists.autoBuilding.Count; i++)
					{
						if (vidLists.subtypes[subtypeIndex].itemIDs.Contains(vidLists.autoBuilding[i].itemID))
						{
							instance = new T();
							instance.UpdateGenericProperties(vidLists.autoBuilding[i]);
							instance.UpdateUniqueProperties(vidLists.autoBuilding[i]);
							items.Add(instance);
							
							if (items.Count == vidLists.subtypes[subtypeIndex].itemIDs.Count)
								return items;
						}
					}
					break;
				case ItemType.Recipe:
					for (int i = 0; i < vidLists.autoRecipe.Count; i++)
					{
						if (vidLists.subtypes[subtypeIndex].itemIDs.Contains(vidLists.autoRecipe[i].itemID))
						{
							instance = new T();
							instance.UpdateGenericProperties(vidLists.autoRecipe[i]);
							instance.UpdateUniqueProperties(vidLists.autoRecipe[i]);
							items.Add(instance);
							
							if (items.Count == vidLists.subtypes[subtypeIndex].itemIDs.Count)
								return items;
						}
					}
					break;
            }//#VID-GASIE

            return items;
        }
    }
}
