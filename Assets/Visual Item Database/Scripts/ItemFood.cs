using UnityEngine;

namespace ItemSystem
{
    [System.Serializable]
	public class ItemFood : ItemBase, IConsumable
    {

		[SerializeField, Range(0, 100), Header("Unique properties")]

		public int energyRegen = 0;

		[SerializeField]
		public bool Edible=true;

		[SerializeField]
		public bool BlossomFeed=false;


		public override void UpdateUniqueProperties(ItemBase itemToChangeTo)
		{
			ItemFood food = (ItemFood)itemToChangeTo;
			energyRegen = food.energyRegen;
			BlossomFeed = food.BlossomFeed;
			Edible = food.Edible;
		}

        public void Consume()
        {
        }
    }
}