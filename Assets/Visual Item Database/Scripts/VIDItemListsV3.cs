using UnityEngine;
using System.Collections.Generic;

namespace ItemSystem
{//#VID-ISNB
	public enum GenericSubtypes
	{
		Liquid,
		Seed,
		Material,
		FireStarter,
	}
	public enum ToolSubtypes
	{
		GridTool,
		FireStarter,
	}
	public enum MaterialSubtypes
	{
		Fuel,
	}
}//#VID-ISNE

namespace ItemSystem
{//#VID-2ISNB
	public enum RecipeItems
	{
		None = 0,
		RecipeCampfire = -1987630773,
	}
	public enum BuildingItems
	{
		None = 0,
		CommunityCenterBlueprint = -1445070677,
	}
	public enum GridItemItems
	{
		None = 0,
	}
	public enum MaterialItems
	{
		None = 0,
		Nails = 1963343688,
		MythrilOre = -1725266136,
		GoldOre = 1686118066,
		SilverOre = -179944841,
		IronOre = -1785934404,
		CopperOre = -357021540,
		Resin = -709983876,
		Clay = -1601692955,
		Coal = 1975587544,
		Stone = -1296374755,
		Plank = 1899089605,
		Log = 86548969,
		Rope = -141889569,
		Rod = -527083629,
		Stick = 774157046,
		PalmFrond = 1133339886,
		Leaf = -100048617,
	}
	public enum PlaceableItemItems
	{
		None = 0,
		Shelter = 1266421838,
		Bed = -1891246765,
		FirePit = 217913788,
		Campfire = -930138330,
		CommunityCenterBlueprint = 650235461,
		AppleTreeSappling = 1536007656,
		Clivia = -1468399537,
		RedRoses = 1972022181,
		StoneWell = -1884038361,
		CraftingStation = -1517540095,
		ChoppingBlock = -675898214,
		ShippingChest = -500200157,
		StorageChest = 1407659378,
		GroomingStation = -594390566,
		BlockPuzzle = 142429970,
		TrainingWeights = 362950378,
		AgilityPoles = 484009408,
		GreenBlossomHut = -1708581655,
		Torch = 1368452359,
	}
	public enum BottleItems
	{
		None = 0,
		WoodenBucket = -396583635,
		EmptyBottle = 1535208075,
		WaterBottle = -1547906302,
	}
	public enum ToolItems
	{
		None = 0,
		StoneAxe = 1967077367,
		StoneHoe = 1231368647,
		StonePickaxe = 1573273775,
		StoneSickle = -1692448748,
		WoodenWateringCan = -16136519,
		CopperAxe = 1485223989,
		CopperHoe = 754714141,
		CopperPickaxe = -851498277,
		CopperSickle = -1047571718,
		CopperWateringCan = 1671772509,
		SteelAxe = -1124638208,
		SteelHoe = 787305248,
		SteelPickaxe = -243631977,
		SteelSickle = -1856322548,
		SteelWateringCan = 372917061,
		TurfRoll = -879960559,
		SandBag = -978429217,
		DirtBag = -1524919010,
		WheatSeeds = -73572827,
		StrawberrySeeds = 115720983,
	}
	public enum FoodItems
	{
		None = 0,
		Wheat = -1912533913,
		Cranberries = -1307152578,
		Strawberry = 783439596,
		Coconut = -550175815,
		Cherry = 1971793235,
		Apple = -1786054029,
	}
	public enum GenericItems
	{
		None = 0,
		FireStarter = -1612136365,
		HalfCoin = -1296926749,
		Coin = -1507328625,
		NULL = -934306571,
	}
}//#VID-2ISNE

namespace ItemSystem.Database
{
    public class VIDItemListsV3 : ScriptableObject
    {
        /*Do NOT change the formatting of anything between comments starting with '#VID-'*/

        public static readonly string itemListsName = "VIDItemListsV3";

        //#VID-ICB
		public List<ItemBase> autoGeneric = new List<ItemBase>();
		public List<ItemTool> autoTool = new List<ItemTool>();
		public List<ItemBottle> autoBottle = new List<ItemBottle>();
		public List<ItemFood> autoFood = new List<ItemFood>();
		public List<ItemPlaceable> autoPlaceableItem = new List<ItemPlaceable>();
		public List<ItemGrid> autoGridItem = new List<ItemGrid>();
		public List<ItemMaterial> autoMaterial = new List<ItemMaterial>();
		public List<ItemBuilding> autoBuilding = new List<ItemBuilding>();
		public List<ItemRecipe> autoRecipe = new List<ItemRecipe>();
        //#VID-ICE

        /*Those two lists are 'parallel', one shouldn't be changed without the other*/
        /// <summary>Stores taken IDs</summary>
        [HideInInspector]
        public List<int> usedIDs = new List<int>();

        /// <summary>Stores the types of taken IDs</summary>
        [HideInInspector]
        public List<ItemType> typesOfUsedIDs = new List<ItemType>();

        [HideInInspector]
        public List<ItemSubtypeV25> subtypes = new List<ItemSubtypeV25>();
        [HideInInspector]
        public List<ItemTypeGroup> typeGroups = new List<ItemTypeGroup>();
    }
}
