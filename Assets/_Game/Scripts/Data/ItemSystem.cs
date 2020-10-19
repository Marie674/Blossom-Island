using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Items
{


    public class ItemSystem : Singleton<ItemSystem>
    {
        public ItemDataProcessor Processor;
        public enum ItemTypes
        {
            Generic,
            Food,
            Tool,
            Bottle,
            Material,
            Prop,
            Recipe
        }

        public enum ItemTags
        {
            FireStarter,
        }

        public ItemBase GetItemClone(int pID)
        {
            foreach (ItemBase item in Processor.Items)
            {
                if (pID == item.ID)
                {
                    return item;//.Clone(item);
                }
            }
            return null;
        }

        public ItemBase GetItemClone(string pName)
        {
            foreach (ItemBase item in Processor.Items)
            {
                if (pName == item.Name)
                {
                    return item;//.Clone(item);
                }
            }
            return null;
        }
    }
}