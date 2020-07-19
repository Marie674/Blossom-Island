using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;

namespace Game.Blossoms
{
    public class BlossomData : MonoBehaviour
    {

        public enum BlossomGrowth
        {
            Unborn,
            Baby,
            Adult,
            Elder
        }
        public string ID;

        [SerializeField]
        public Stat Agility;
        [SerializeField]
        public Stat Strength;
        [SerializeField]
        public Stat Intellect;
        [SerializeField]
        public Stat Charm;

        public string Parent1;
        public string Parent2;
        public List<string> Children;
        public int ChildAmount;
        public string Name;
        public int Age;
        public BlossomGrowth Growth = BlossomGrowth.Unborn;
        public bool Hungry;
        public int Happiness;
        public int Affection;
        public int Energy;
        public bool Pregnant;
        public int DaysPregnant;
        public string BabyID;
        public bool PetToday;
        public bool TalkedToday;
        public bool FedToday;
        public List<Trait> Traits = new List<Trait>();
        public int TraitAmount;
        public string CurrentLevel;
        public float CurrentX;
        public float CurrentY;

        public Vector2 HutPosition;
        public string HutName;
        public Hut Hut;

        public string Color;

        public bool ForSale;

    }
}