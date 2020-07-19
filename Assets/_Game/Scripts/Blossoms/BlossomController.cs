using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;
using PixelCrushers.DialogueSystem;
using BehaviorDesigner.Runtime;
namespace Game.Blossoms
{
    public class BlossomController : MonoBehaviour
    {
        BlossomData Data;
        public BehaviorTree Behavior;

        void Start()
        {
            Data = GetComponent<BlossomData>();
            if (DialogueLua.DoesVariableExist(Data.ID + "HutX"))
            {
                Data.HutPosition = new Vector2(DialogueLua.GetVariable(Data.ID + "HutX").asFloat, DialogueLua.GetVariable(Data.ID + "HutY").asFloat);
                Data.HutName = DialogueLua.GetVariable(Data.ID + "HutName").asString;
                Data.Hut = BlossomManager.Instance.GetHutObject(Data.HutName);
            }

        }

        void OnEnable()
        {
            TimeManager.OnDayChanged += NewDay;
            DayPhaseManager.OnPhaseChange += DayPhaseChange;
        }

        void OnDisable()
        {
            TimeManager.OnDayChanged -= NewDay;
            DayPhaseManager.OnPhaseChange -= DayPhaseChange;
        }

        void NewDay(int pCurrentDay)
        {
            Data.PetToday = false;
            Data.TalkedToday = false;
            Data.FedToday = false;
            if (Data.Growth != BlossomData.BlossomGrowth.Unborn)
            {
                Data.Age += 1;
                if (Data.Age >= 28 && (Data.Growth.ToString() != BlossomData.BlossomGrowth.Adult.ToString() && Data.Growth.ToString() != BlossomData.BlossomGrowth.Elder.ToString()))
                {

                    Data.Growth = BlossomData.BlossomGrowth.Adult;
                    print(Data.ID + "" + Data.Color);
                    GetComponent<BlossomAppearance>().SetAppearance(Data.Growth, Data.Color);
                }
                if (Data.Age >= 336 && (Data.Growth != BlossomData.BlossomGrowth.Elder))
                {
                    Data.Growth = BlossomData.BlossomGrowth.Elder;
                }
            }

            if (Data.Pregnant == true)
            {
                Data.DaysPregnant += 1;
            }

            SetDayEnergy();
            SetAffection();
            CheckPregnancy();
            CheckGiveBirth();
            Data.Happiness = 0;
            GetComponent<BlossomDataSaver>().OnRecordPersistentData();
        }

        void SetDayEnergy()
        {
            Data.Energy = 0;
            int val = Data.Happiness;

            foreach (Trait trait in Data.Traits)
            {
                val += trait.EnergyChange;
            }
            Data.Energy = val;
        }

        void SetAffection()
        {
            Data.Affection += Data.Happiness;
        }

        //Blossoms become hungry twice a day, in the morning and afternoon
        void DayPhaseChange()
        {
            DayPhaseManager.DayPhaseNames prevDayPhase = DayPhaseManager.Instance.PreviousDayPhase.Name;
            DayPhaseManager.DayPhaseNames currentDayPhase = DayPhaseManager.Instance.CurrentDayPhase.Name;
            if (currentDayPhase == DayPhaseManager.DayPhaseNames.Morning && prevDayPhase.ToString() != DayPhaseManager.DayPhaseNames.Morning.ToString())
            {
                Data.Hungry = true;
                if(Behavior!=null)
                Behavior.SetVariableValue("IsHungry", true);

            }
            else if (currentDayPhase == DayPhaseManager.DayPhaseNames.Afternoon && prevDayPhase.ToString() != DayPhaseManager.DayPhaseNames.Afternoon.ToString())
            {
                Data.Hungry = true;
                if(Behavior!=null)
                Behavior.SetVariableValue("IsHungry", true);
            }
        }

        bool CheckBowl()
        {
            if (Data.Hut == null)
            {
                return false;
            }
            if (Data.Hut.BowlFull == true)
            {
                return true;
            }
            return false;

        }

        void EatFromBowl()
        {
            if (CheckBowl() == false)
            {
                return;
            }
            Feed(Data.Hut.BowlFood);
            Data.Hut.EatFrom();
        }

        void Feed(ItemBase pFood)
        {
            if (Data.Hungry == false)
            {
                return;
            }
            //if (Data.FedToday == false){
            //    Data.Happiness += 1;
            //}
            PixelCrushers.MessageSystem.SendMessage(this, "FeedBlossom", Data.Name);
            ParticleSpawner.Instance.SpawnBubble(ParticleSpawner.BubbleTypes.Heart, new Vector2(transform.position.x, transform.position.y + 1f), transform);

            Data.Happiness += 1;
            Data.FedToday = true;
            Data.Hungry = false;
            if(Behavior!=null)
            Behavior.SetVariableValue("IsHungry", false);

        }


        void Pet()
        {
            if (Data.PetToday == false)
            {
                Data.Happiness += 1;
                PixelCrushers.MessageSystem.SendMessage(this, "PetBlossom", Data.Name);

            }
            ParticleSpawner.Instance.SpawnBubble(ParticleSpawner.BubbleTypes.Heart, new Vector2(transform.position.x, transform.position.y + 1f), transform);
            GameManager.Instance.Player.DoAction("Pet", GameManager.Instance.Player.PettingTime, transform.position);
            Data.PetToday = true;
        }
        void Talk()
        {
            if (Data.TalkedToday == false)
            {
                Data.Happiness += 1;
            }
            Data.TalkedToday = true;
            DialogueManager.instance.masterDatabase.GetActor(5).Name = Data.Name;
            GetComponent<DialogueActor>().actor = DialogueManager.instance.masterDatabase.GetActor(5).Name;
            DialogueManager.StopConversation();
            DialogueManager.StartConversation("Blossom Hug", transform);
        }

        public void Train(TrainingProp pProp)
        {
            if (Data.Hungry == true)
            {
                return;
            }
            if (Data.Pregnant == true)
            {
                return;
            }
            if (Data.Energy < 1)
            {
                return;
            }
            PixelCrushers.MessageSystem.SendMessage(this, "TrainBlossom", Data.name);
            Data.Energy -= 1;

            float statIncrease = pProp.Training;
            float affectionModifier = Data.Affection / 1000f;

            affectionModifier = Mathf.Clamp(affectionModifier, 0, 1f);
            float statAffectionBonus = affectionModifier + statIncrease;
            statIncrease = Random.Range(statIncrease, statAffectionBonus);

            switch (pProp.TargetStat)
            {
                case Stat.StatName.Agility:
                    statIncrease = statIncrease * GetTrueLearningSpeed(Stat.StatName.Agility);
                    ChangeStat(Stat.StatName.Agility, statIncrease);
                    break;
                case Stat.StatName.Charm:
                    statIncrease = statIncrease * GetTrueLearningSpeed(Stat.StatName.Charm);
                    ChangeStat(Stat.StatName.Agility, statIncrease);
                    break;
                case Stat.StatName.Intellect:
                    statIncrease = statIncrease * GetTrueLearningSpeed(Stat.StatName.Intellect);
                    ChangeStat(Stat.StatName.Agility, statIncrease);
                    break;
                case Stat.StatName.Strength:
                    statIncrease = statIncrease * GetTrueLearningSpeed(Stat.StatName.Strength);
                    ChangeStat(Stat.StatName.Agility, statIncrease);
                    break;
                default:
                    break;
            }
            GetComponent<BlossomDataSaver>().OnRecordPersistentData();


        }

        void ChangeStat(Stat.StatName pStat, float pAmount)
        {
            switch (pStat)
            {
                case Stat.StatName.Agility:
                    Data.Agility.Value = Mathf.Clamp(Data.Agility.Value + pAmount, 0, GetTruePotential(Stat.StatName.Agility));
                    break;
                case Stat.StatName.Charm:
                    Data.Charm.Value = Mathf.Clamp(Data.Charm.Value + pAmount, 0, GetTruePotential(Stat.StatName.Charm));
                    break;
                case Stat.StatName.Intellect:
                    Data.Intellect.Value = Mathf.Clamp(Data.Intellect.Value + pAmount, 0, GetTruePotential(Stat.StatName.Intellect));
                    break;
                case Stat.StatName.Strength:
                    Data.Strength.Value = Mathf.Clamp(Data.Strength.Value + pAmount, 0, GetTruePotential(Stat.StatName.Strength));
                    break;
                default:
                    break;
            }
            GetComponent<BlossomDataSaver>().OnRecordPersistentData();

        }

        public void Interact()
        {
            ItemBase heldItem = null;
            if (Toolbar.Instance.SelectedSlot.ReferencedItemStack != null)
            {
                heldItem = Toolbar.Instance.SelectedSlot.ReferencedItemStack.ContainedItem;
            }
            if (heldItem != null)
            {

                if (heldItem.itemType == ItemType.Food && (heldItem as ItemFood).BlossomFeed == true)
                {
                    if (Data.Hungry == true)
                    {

                        Feed(heldItem as ItemFood);
                        return;
                    }
                }
            }
            if (Data.PetToday == false)
            {
                Pet();
                return;
            }
            Talk();

        }

        void CheckPregnancy()
        {
            if (Data.Growth != BlossomData.BlossomGrowth.Adult)
            {
                return;
            }
            if (Data.Pregnant == true)
            {
                return;
            }
            if (BlossomManager.Instance.HutAmount >= BlossomManager.Instance.MaxHuts)
            {
                return;
            }
            bool pregnantChance = false;

            int rand = Random.Range(0, 101);
            if (rand <= 15)
            {
                pregnantChance = true;
            }

            if (pregnantChance == true)
            {
                //CHECK COMPATIBLE BLOSSOMS
                List<string> compatibleBlossoms = new List<string>();
                string match = string.Empty;
                foreach (string blossom in BlossomManager.Instance.OwnedBlossoms)
                {
                    if (Data.ID == blossom)
                    {
                        continue;
                    }
                    if (Data.Parent1 == blossom)
                    {

                        continue;
                    }
                    if (Data.Parent2 == blossom)
                    {
                        continue;
                    }
                    BlossomData.BlossomGrowth growth = (BlossomData.BlossomGrowth)System.Enum.Parse(typeof(BlossomData.BlossomGrowth), DialogueLua.GetVariable(blossom + "Growth").asString);
                    print(growth.ToString());
                    if (growth != BlossomData.BlossomGrowth.Adult)
                    {
                        continue;
                    }
                    if (DialogueLua.GetVariable(blossom + "Pregnant").asBool == true)
                    {
                        continue;
                    }
                    compatibleBlossoms.Add(blossom);
                }
                if (compatibleBlossoms.Count > 0)
                {
                    rand = Random.Range(0, compatibleBlossoms.Count - 1);
                    match = compatibleBlossoms[rand];
                }
                else
                {
                    print("no compat");
                }
                //IF MATCH FOUND
                if (match != string.Empty)
                {
                    Data.BabyID = BlossomManager.Instance.CreateChildBlossom(Data.ID, match);
                    PixelCrushers.MessageSystem.SendMessage(this, "PregnantBlossom", Data.ID);
                    string babyHut = BlossomManager.Instance.GetEmptyHut();
                    DialogueLua.SetVariable(babyHut + "Blossom", Data.BabyID);
                    DialogueLua.SetVariable(Data.BabyID + "HutX", DialogueLua.GetVariable(babyHut + "X").asFloat);
                    DialogueLua.SetVariable(Data.BabyID + "HutY", DialogueLua.GetVariable(babyHut + "Y").asFloat);
                    DialogueLua.SetVariable(Data.BabyID + "HutName", babyHut);
                    Data.Pregnant = true;
                    Data.DaysPregnant = 0;
                    GetComponent<BlossomDataSaver>().OnRecordPersistentData();


                }
            }



        }

        void CheckGiveBirth()
        {

            if (Data.DaysPregnant >= 14)
            {

                //GIVE BIRTH
                Data.Pregnant = false;
                Data.DaysPregnant = 0;
                DialogueLua.SetVariable(Data.BabyID + "Growth", BlossomData.BlossomGrowth.Baby.ToString());
                DialogueLua.SetVariable(Data.BabyID + "CurrentLevel", "Home");
                DialogueLua.SetVariable(Data.BabyID + "CurrentX", DialogueLua.GetVariable(Data.BabyID + "HutX").asFloat);
                DialogueLua.SetVariable(Data.BabyID + "CurrentY", DialogueLua.GetVariable(Data.BabyID + "HutY").asFloat);

                BlossomManager.Instance.OwnedBlossoms.Add(Data.BabyID);
                BlossomManager.Instance.ExistingBlossoms.Add(Data.BabyID);
                BlossomManager.Instance.SpawnBlossom(Data.BabyID, new Vector2(26, 81));
                PixelCrushers.MessageSystem.SendMessage(this, "BlossomBirth", Data.BabyID);
                Data.BabyID = string.Empty;
                GetComponent<BlossomDataSaver>().OnRecordPersistentData();

            }

        }

        Stat GetStat(Stat.StatName pStat)
        {
            if (pStat == Stat.StatName.Agility)
            {
                return Data.Agility;
            }
            else if (pStat == Stat.StatName.Intellect)
            {
                return Data.Intellect;
            }
            else if (pStat == Stat.StatName.Strength)
            {
                return Data.Strength;
            }
            else if (pStat == Stat.StatName.Charm)
            {
                return Data.Charm;
            }
            return Data.Agility;
        }
        public float GetTruePotential(Stat.StatName pStat)
        {
            Stat stat = GetStat(pStat);
            float val = stat.Potential;
            foreach (Trait trait in Data.Traits)
            {
                foreach (StatInfluence influence in trait.StatInfluences)
                {
                    if (influence.InfluenceType == Trait.StatInfluenceType.Potential)
                    {
                        if (influence.PercentageChange > 0)
                        {
                            float percentage = stat.Potential * influence.PercentageChange;
                            val = Mathf.Clamp(val + percentage, 0, 1000);
                        }
                        if (influence.ValueChange > 0)
                        {
                            val = Mathf.Clamp(val + influence.ValueChange, 0, 1000);
                        }
                    }
                }
            }
            return val;
        }

        public float GetTrueLearningSpeed(Stat.StatName pStat)
        {
            Stat stat = GetStat(pStat);
            float val = stat.LearningSpeed;
            foreach (Trait trait in Data.Traits)
            {
                foreach (StatInfluence influence in trait.StatInfluences)
                {
                    if (influence.InfluenceType == Trait.StatInfluenceType.LearningSpeed)
                    {
                        if (influence.PercentageChange > 0)
                        {
                            float percentage = stat.LearningSpeed * influence.PercentageChange;
                            val = Mathf.Clamp(val + percentage, 0, 1000);
                        }
                        if (influence.ValueChange > 0)
                        {
                            val = Mathf.Clamp(val + influence.ValueChange, 0, 1000);
                        }
                    }
                }
            }
            return val;
        }

        public float GetStatValue(Stat.StatName pStat)
        {
            Stat stat = GetStat(pStat);
            float val = stat.Value;
            return val;
        }

    }
}
