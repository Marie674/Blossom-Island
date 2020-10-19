using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace Game.Items
{
    [System.Serializable]
    public class ItemProp : ItemBase
    {
        [Header("Specific Properties")]

        [PreviewField]
        public Sprite PropSprite;
        public List<GameObject> Props = new List<GameObject>();
        public override ItemBase Clone(ItemBase itemToClone)
        {
            ItemProp itemProp = itemToClone as ItemProp;
            ItemProp newItem = base.Clone(itemToClone) as ItemProp;
            if (newItem == null)
            {
                return null;
            }
            if (itemProp.Props != null)
            {
                newItem.PropSprite = itemProp.PropSprite;
                newItem.Props = itemProp.Props;
            }
            return newItem;
        }
        public override void Use()
        {

        }
    }
}

