using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Blossoms
{

    public class BlossomStore : MonoBehaviour
    {
        public List<BlossomSaleSlot> Slots;

        public string LevelName;

        public int Fee;

        void OnEnable()
        {
            BlossomSaleSlot[] allSlots = GetComponentsInChildren<BlossomSaleSlot>();
            foreach (BlossomSaleSlot slot in allSlots)
            {
                Slots.Add(slot);
                slot.LevelName = LevelName;
                slot.Fee = Fee;
            }
            TimeManager.OnDayChanged += NewDay;

        }

        void OnDisable()
        {
            TimeManager.OnDayChanged -= NewDay;
        }

        void NewDay(int pCurrentDay)
        {

            foreach (BlossomSaleSlot slot in Slots)
            {
                slot.NewDay(pCurrentDay);
            }
            BlossomManager.Instance.SpawnLevelBlossoms();
        }

    }

}

