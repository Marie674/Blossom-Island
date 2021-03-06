﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Game.Items;

namespace Game.NPCs.Blossoms
{
    public class Hut : MonoBehaviour
    {
        public string ContainedBlossom = string.Empty;
        public string Name;
        public SpriteRenderer Bowl;
        public bool BowlFull = false;
        public ItemFood BowlFood;

        public Sprite EmptyBowlSprite;
        public Sprite FullBowlSprite;

        void Start()
        {
            Init();
        }
        public void Init()

        {
            Name = "Hut" + transform.position.x.ToString("F1") + transform.position.y.ToString("F1");
            gameObject.name = Name;
        }
        void OnDestroy()
        {
            //  Game.Blossoms.BlossomManager.Instance.RemoveHut(Name);
        }

        public void Interact()
        {
            InventoryItemStack stack = Toolbar.Instance.SelectedSlot.ReferencedItemStack;
            if (stack != null)
            {
                ItemBase item = stack.ContainedItem;
                if (item.Type == ItemSystem.ItemTypes.Food && (item as ItemFood).BlossomFeed == true)
                {

                    FillBowl(item as ItemFood);
                }
            }
        }

        void FillBowl(ItemFood pFood)
        {
            if (BowlFull)
            {
                return;
            }
            BowlFull = true;
            BowlFood = pFood;
            Bowl.sprite = FullBowlSprite;
        }

        public void EatFrom()
        {
            EmptyBowl();
        }

        void EmptyBowl()
        {
            BowlFull = false;
            BowlFood = null;
            Bowl.sprite = EmptyBowlSprite;
        }


    }
}

