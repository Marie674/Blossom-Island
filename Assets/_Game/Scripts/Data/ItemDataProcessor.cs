using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ES3Internal;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.AddressableAssets;
using System.Linq;
using UnityEngine.U2D;

namespace Game.Items
{

    [CreateAssetMenu(fileName = "ItemDataProcessor", menuName = "Data/ItemDataProcessor")]

    public class ItemDataProcessor : ScriptableObject
    {
        ES3Spreadsheet Spreadsheet;

        [FilePath]
        public string SpreadSheetPath;
        [FolderPath]
        public string ObjectPath;
        [FilePath]
        public string AtlasPath;

        [FolderPath]
        public string[] IconPaths = new string[1];


        private int IDIndex;
        private int NameIndex;
        private int DescriptionIndex;
        private int IconNameIndex;
        private int SpriteNameIndex;
        private int ValueIndex;
        private int MarkupIndex;
        private int WeightIndex;
        private int TypeIndex;
        private int SellableIndex;
        private int ConsumableIndex;
        private int ToolbarIndex;
        private int EnergyRegenIndex;
        private int FoodEdibleIndex;
        private int BlossomFeedIndex;
        private int PowerIndex;
        private int LevelIndex;
        private int IntervalIndex;
        private int EnergyCostIndex;
        private int PlayerTriggerIndex;
        private int ToolTriggerIndex;
        private int ToolMaxChargeIndex;
        private int CropNameIndex;
        private int BottleMaxChargeIndex;
        private int HeldLiquidIndex;
        private int BurnTimeIndex;

        [SerializeField]
        [ShowInInspector]
        public List<Game.Items.ItemBase> Items = new List<Game.Items.ItemBase>();

#if (UNITY_EDITOR)


        [Button("Load Spreadsheet", ButtonSizes.Gigantic)]
        public void LoadData()
        {
            Spreadsheet = new ES3Spreadsheet();
            ES3Settings settings = new ES3Settings();
            settings.location = ES3.Location.Resources;
            Spreadsheet.Load("Data/ItemData.csv", settings);
            PopulateObjects();

        }
        void PopulateObjects()
        {

            SpriteAtlas atlas = EditorGUIUtility.Load(AtlasPath) as SpriteAtlas;
            SetIndexes();

            for (int row = 2; row < Spreadsheet.RowCount; row++)
            {
                if (Spreadsheet.GetCell<string>(0, row) == string.Empty)
                {
                    continue;
                }
                int ID = -1;
                string itemName = string.Empty;
                string description = string.Empty;
                string iconName = string.Empty;
                string spriteName = string.Empty;
                float value = -1;
                int markup = -1;
                float weight = -1;
                string type = string.Empty;
                bool sellable = false;
                bool consumable = false;
                bool usableFromToolbar = false;
                Sprite itemIcon = null;
                Sprite itemSprite = null;
                bool itemExists = false;

                if (IDIndex != -1)
                {
                    ID = Spreadsheet.GetCell<int>(IDIndex, row);

                }
                if (NameIndex != -1)
                {
                    itemName = Spreadsheet.GetCell<string>(NameIndex, row);
                }
                if (DescriptionIndex != -1)
                {
                    description = Spreadsheet.GetCell<string>(DescriptionIndex, row);
                }
                if (IconNameIndex != -1)
                {
                    iconName = Spreadsheet.GetCell<string>(IconNameIndex, row);

                    string guid = string.Empty;
                    Sprite sprite = null;

                    if (AssetDatabase.FindAssets(iconName, IconPaths).Length > 0)
                    {
                        guid = AssetDatabase.FindAssets(iconName, IconPaths)[0];
                    }

                    if (guid != string.Empty)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(guid);
                        sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                    }

                    if (sprite != null)
                    {
                        itemIcon = sprite;
                    }
                }
                if (ValueIndex != -1)
                {
                    value = Spreadsheet.GetCell<float>(ValueIndex, row);
                }
                if (MarkupIndex != -1)
                {
                    markup = Spreadsheet.GetCell<int>(MarkupIndex, row);
                }
                if (WeightIndex != -1)
                {
                    weight = Spreadsheet.GetCell<float>(WeightIndex, row);
                }
                if (TypeIndex != -1)
                {
                    type = Spreadsheet.GetCell<string>(TypeIndex, row);
                }
                if (SellableIndex != -1)
                {
                    sellable = Spreadsheet.GetCell<bool>(SellableIndex, row);
                }
                if (ConsumableIndex != -1)
                {
                    consumable = Spreadsheet.GetCell<bool>(ConsumableIndex, row);
                }
                if (ToolbarIndex != -1)
                {
                    usableFromToolbar = Spreadsheet.GetCell<bool>(ToolbarIndex, row);
                }

                if (type != string.Empty)
                {
                    ItemBase item = null;
                    item = Items.Find(x => x.ID == ID);
                    switch (type)
                    {
                        case "Generic":
                        case "Recipe":
                            ItemBase newItem = null;

                            if (item != null)
                            {
                                itemExists = true;
                            }

                            if (itemExists == false)
                            {
                                newItem = ScriptableObject.CreateInstance<ItemBase>();
                            }
                            else
                            {
                                newItem = item;
                            }
                            ApplyGenericProperties(newItem, ID, itemName, description, itemIcon, value, markup, weight, sellable, consumable, usableFromToolbar, type);
                            if (itemExists == false)
                            {
                                Items.Add(newItem);
                                Debug.Log("Creating: " + itemName);
                                AssetDatabase.CreateAsset(newItem, ObjectPath + "/" + type + "/" + itemName + ".asset");
                            }
                            break;
                        case "Prop":
                            ItemProp newItemProp = null;
                            if (SpriteNameIndex != -1)
                            {
                                spriteName = Spreadsheet.GetCell<string>(SpriteNameIndex, row);
                            }
                            if (item != null)
                            {
                                itemExists = true;
                            }
                            if (itemExists == false)
                            {
                                newItemProp = ScriptableObject.CreateInstance<ItemProp>();
                            }
                            else
                            {
                                newItemProp = item as ItemProp;
                            }
                            ApplyGenericProperties(newItemProp, ID, itemName, description, itemIcon, value, markup, weight, sellable, consumable, usableFromToolbar, type);
                            newItemProp.PropSprite = itemSprite;
                            if (itemExists == false)
                            {
                                Items.Add(newItemProp);
                                AssetDatabase.CreateAsset(newItemProp, ObjectPath + "/" + type + "/" + itemName + ".asset");

                            }
                            break;
                        case "Food":
                            float energyRegen = -1;
                            bool edible = false;
                            bool blossomFeed = false;
                            if (EnergyRegenIndex != -1)
                            {
                                energyRegen = Spreadsheet.GetCell<float>(EnergyRegenIndex, row);
                            }
                            if (FoodEdibleIndex != -1)
                            {
                                edible = Spreadsheet.GetCell<bool>(FoodEdibleIndex, row);
                            }
                            if (BlossomFeedIndex != -1)
                            {
                                blossomFeed = Spreadsheet.GetCell<bool>(BlossomFeedIndex, row);
                            }

                            ItemFood newItemFood = null;
                            if (item != null)
                            {
                                itemExists = true;
                            }
                            if (itemExists == false)
                            {
                                newItemFood = ScriptableObject.CreateInstance<ItemFood>();
                            }
                            else
                            {
                                newItemFood = item as ItemFood;
                            }
                            ApplyGenericProperties(newItemFood, ID, itemName, description, itemIcon, value, markup, weight, sellable, consumable, usableFromToolbar, type);
                            newItemFood.EnergyRegen = energyRegen;
                            newItemFood.Edible = edible;
                            newItemFood.BlossomFeed = blossomFeed;

                            if (itemExists == false)
                            {
                                Items.Add(newItemFood);
                                AssetDatabase.CreateAsset(newItemFood, ObjectPath + "/" + type + "/" + itemName + ".asset");

                            }
                            break;
                        case "Tool":
                            float power = -1;
                            int level = -1;
                            float useInterval = -1;
                            float energyCost = -1;
                            string playerTrigger = string.Empty;
                            string toolTrigger = string.Empty;
                            int toolMaxCharge = -1;
                            string cropName = string.Empty;
                            if (PowerIndex != -1)
                            {
                                power = Spreadsheet.GetCell<float>(PowerIndex, row);
                            }
                            if (LevelIndex != -1)
                            {
                                level = Spreadsheet.GetCell<int>(LevelIndex, row);
                            }
                            if (IntervalIndex != -1)
                            {
                                useInterval = Spreadsheet.GetCell<float>(IntervalIndex, row);
                            }
                            if (EnergyCostIndex != -1)
                            {
                                energyCost = Spreadsheet.GetCell<float>(EnergyCostIndex, row);
                            }
                            if (PlayerTriggerIndex != -1)
                            {
                                playerTrigger = Spreadsheet.GetCell<string>(PlayerTriggerIndex, row);
                            }
                            if (ToolTriggerIndex != -1)
                            {
                                toolTrigger = Spreadsheet.GetCell<string>(ToolTriggerIndex, row);
                            }
                            if (ToolMaxChargeIndex != -1)
                            {
                                toolMaxCharge = Spreadsheet.GetCell<int>(ToolMaxChargeIndex, row);
                            }
                            if (CropNameIndex != -1)
                            {
                                cropName = Spreadsheet.GetCell<string>(CropNameIndex, row);
                            }


                            ItemTool newItemTool = null;
                            if (item != null)
                            {
                                itemExists = true;
                            }
                            if (itemExists == false)
                            {
                                newItemTool = ScriptableObject.CreateInstance<ItemTool>();
                            }
                            else
                            {
                                newItemTool = item as ItemTool;
                            }
                            ApplyGenericProperties(newItemTool, ID, itemName, description, itemIcon, value, markup, weight, sellable, consumable, usableFromToolbar, type);

                            newItemTool.Power = power;
                            newItemTool.Level = level;
                            newItemTool.UseInterval = useInterval;
                            newItemTool.EnergyCost = energyCost;
                            newItemTool.PlayerTrigger = playerTrigger;
                            newItemTool.ToolTrigger = toolTrigger;
                            newItemTool.MaxCharge = toolMaxCharge;
                            newItemTool.CurrentCharge = newItemTool.MaxCharge;
                            newItemTool.CropName = cropName;
                            if (itemExists == false)
                            {
                                Items.Add(newItemTool);
                                AssetDatabase.CreateAsset(newItemTool, ObjectPath + "/" + type + "/" + itemName + ".asset");
                            }
                            break;
                        case "Bottle":
                            int bottleMaxCharge = -1;
                            if (BottleMaxChargeIndex != -1)
                            {
                                bottleMaxCharge = Spreadsheet.GetCell<int>(BottleMaxChargeIndex, row);
                            }

                            ItemBottle newItemBottle = null;
                            if (item != null)
                            {
                                itemExists = true;
                            }
                            if (itemExists == false)
                            {
                                newItemBottle = ScriptableObject.CreateInstance<ItemBottle>();
                            }
                            else
                            {
                                newItemBottle = item as ItemBottle;
                            }
                            ApplyGenericProperties(newItemBottle, ID, itemName, description, itemIcon, value, markup, weight, sellable, consumable, usableFromToolbar, type);

                            newItemBottle.MaxCharge = bottleMaxCharge;
                            newItemBottle.CurrentCharge = newItemBottle.MaxCharge;

                            if (itemExists == false)
                            {
                                Items.Add(newItemBottle);
                                AssetDatabase.CreateAsset(newItemBottle, ObjectPath + "/" + type + "/" + itemName + ".asset");
                            }
                            break;
                        case "Material":
                            float burnTime = -1;
                            if (BurnTimeIndex != -1)
                            {
                                burnTime = Spreadsheet.GetCell<float>(BurnTimeIndex, row);
                            }

                            ItemMaterial newItemMat = null;
                            if (item != null)
                            {
                                itemExists = true;
                            }
                            if (itemExists == false)
                            {
                                newItemMat = ScriptableObject.CreateInstance<ItemMaterial>();
                            }
                            else
                            {
                                newItemMat = item as ItemMaterial;
                            }
                            ApplyGenericProperties(newItemMat, ID, itemName, description, itemIcon, value, markup, weight, sellable, consumable, usableFromToolbar, type);

                            newItemMat.BurnTime = burnTime;
                            if (itemExists == false)
                            {
                                Items.Add(newItemMat);
                                AssetDatabase.CreateAsset(newItemMat, ObjectPath + "/" + type + "/" + itemName + ".asset");
                            }
                            break;
                        default:
                            break;
                    }
                }




            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        void ApplyGenericProperties(ItemBase pItem, int pID, string pName, string pDescription, Sprite pIcon, float pVal, int pMarkup, float pWeight, bool pSellable, bool pConsumable, bool pToolbar, string pType)
        {
            pItem.ID = pID;
            pItem.Name = pName;
            pItem.Description = pDescription;
            if (pIcon != null)
            {
                pItem.Icon = pIcon;

            }

            pItem.Value = pVal;
            pItem.Markup = pMarkup;
            pItem.Weight = pWeight;
            pItem.Sellable = pSellable;
            pItem.Consumable = pConsumable;
            pItem.UsableFromToolbar = pToolbar;

            ItemSystem.ItemTypes type = (ItemSystem.ItemTypes)System.Enum.Parse(typeof(ItemSystem.ItemTypes), pType, true);
            if (type.ToString() != string.Empty)
            {
                pItem.Type = type;
            }
        }
        void SetIndexes()
        {

            IDIndex = -1;
            NameIndex = -1;
            DescriptionIndex = -1;
            IconNameIndex = -1;
            SpriteNameIndex = -1;
            ValueIndex = -1;
            MarkupIndex = -1;
            WeightIndex = -1;
            TypeIndex = -1;
            SellableIndex = -1;
            ConsumableIndex = -1;
            ToolbarIndex = -1;
            EnergyRegenIndex = -1;
            FoodEdibleIndex = -1;
            BlossomFeedIndex = -1;
            PowerIndex = -1;
            LevelIndex = -1;
            IntervalIndex = -1;
            EnergyCostIndex = -1;
            PlayerTriggerIndex = -1;
            ToolTriggerIndex = -1;
            ToolMaxChargeIndex = -1;
            CropNameIndex = -1;
            BottleMaxChargeIndex = -1;
            HeldLiquidIndex = -1;
            BurnTimeIndex = -1;

            for (int col = 0; col < Spreadsheet.ColumnCount; col++)
            {
                string text = Spreadsheet.GetCell<string>(col, 1);

                if (text == "ID")
                {
                    IDIndex = col;
                    continue;
                }
                if (text == "Name")
                {
                    NameIndex = col;
                    continue;
                }
                if (text == "Description")
                {
                    DescriptionIndex = col;
                    continue;
                }
                if (text == "Icon Name")
                {
                    IconNameIndex = col;
                    continue;
                }
                if (text == "Sprite Name")
                {
                    SpriteNameIndex = col;
                    continue;
                }
                if (text == "Value")
                {
                    ValueIndex = col;
                    continue;
                }
                if (text == "Markup")
                {
                    MarkupIndex = col;
                    continue;
                }
                if (text == "Weight")
                {
                    WeightIndex = col;
                    continue;
                }
                if (text == "Type")
                {
                    TypeIndex = col;
                    continue;
                }
                if (text == "Sellable")
                {
                    SellableIndex = col;
                    continue;
                }
                if (text == "Consumable")
                {
                    ConsumableIndex = col;
                    continue;
                }
                if (text == "Usable From Toolbar")
                {
                    ToolbarIndex = col;
                    continue;
                }
                if (text == "Energy Regen")
                {
                    EnergyRegenIndex = col;
                    continue;
                }
                if (text == "Food Edible")
                {
                    FoodEdibleIndex = col;
                    continue;
                }
                if (text == "Blossom Feed")
                {
                    BlossomFeedIndex = col;
                    continue;
                }
                if (text == "Power")
                {
                    PowerIndex = col;
                    continue;
                }
                if (text == "Level")
                {
                    LevelIndex = col;
                    continue;
                }
                if (text == "Use Interval")
                {
                    IntervalIndex = col;
                    continue;
                }
                if (text == "Energy Cost")
                {
                    EnergyCostIndex = col;
                    continue;
                }
                if (text == "Player Trigger")
                {
                    PlayerTriggerIndex = col;
                    continue;
                }
                if (text == "Tool Trigger")
                {
                    ToolTriggerIndex = col;
                    continue;
                }
                if (text == "Tool Max Charge")
                {
                    ToolMaxChargeIndex = col;
                    continue;
                }
                if (text == "Crop Name")
                {
                    CropNameIndex = col;
                    continue;
                }
                if (text == "Bottle Max Charge")
                {
                    BottleMaxChargeIndex = col;
                    continue;
                }
                if (text == "Held Liquid")
                {
                    HeldLiquidIndex = col;
                    continue;
                }
                if (text == "Burn Time")
                {
                    BurnTimeIndex = col;
                    continue;
                }
            }

        }

#endif
    }
}