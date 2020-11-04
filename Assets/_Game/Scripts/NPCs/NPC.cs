using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using System.Linq;
namespace Game.NPCs
{


    public class NPC : MonoBehaviour
    {
        public string Name;
        public Actor Actor;
        public float CurrentAffection = 0;
        public float CurrentAcquaintance = 0;

        public float AcquaintanceRaiseMultiplier = 1;
        public float AcquaintanceLowerMultiplier = 1;

        public float GreetingMaxAffectionGain = 5;
        public float GreetingMaxAffectionLoss = 0.1f;

        public float CompatibilityMultiplier;

        public float AffectionRaiseMultiplier = 1;
        public float AffectionLowerMultiplier = 1;

        public float MinAffection = 0;
        public float MaxAffection = 1000;
        public float MaxAcquaintance = 1000;

        public float minCompatibility = 0;
        public float maxCompatibility = 3;

        public NPCData Data;
        public NPConversationData ConversationData;

        [SerializeField]
        public List<NPCTraitScriptableObject> Traits = new List<NPCTraitScriptableObject>();

        void Start()
        {
            Data = GetComponent<NPCData>();
        }
        void OnEnable()
        {
            TimeManager.OnDayChanged += NewDay;
        }

        void OnDisable()
        {
            TimeManager.OnDayChanged -= NewDay;
        }

        void NewDay(int pDay)
        {
            if (Data != null)
            {
                Data.GreetedToday = false;
            }
        }

        public void Interact()
        {
            if (Data.Met == false)
            {
                Meet();
                Data.Met = true;
            }
            else
            {
                if (Data.GreetedToday == false)
                {
                    Greet();
                }
                SpeakTo();
            }
        }
        void SpeakTo()
        {
            Data.CurrentEligibleConversations = ConversationData.CheckEligibleConversations();
            if (Data.CurrentEligibleConversations.Count < 1)
            {
                Debug.Log("NOTHING IS ELIGIBLE");
                return;
            }
            foreach (Conversation conversation in Data.SaidToday)
            {
                if (Data.CurrentEligibleConversations.Contains(conversation))
                {
                    Data.CurrentEligibleConversations.Remove(conversation);
                }
            }
            if (Data.CurrentEligibleConversations.Count < 1)
            {
                Data.SaidToday.Clear();
                Data.CurrentEligibleConversations = ConversationData.CheckEligibleConversations();
            }
            Data.CurrentEligibleConversations = ListExtension.Shuffle<Conversation>(Data.CurrentEligibleConversations);
            Data.CurrentEligibleConversations = Data.CurrentEligibleConversations.OrderByDescending(c => c.LookupInt("Priority")).ToList<Conversation>();
            Conversation picked = Data.CurrentEligibleConversations[0];

            Field field = picked.fields.Find(f => string.Equals(f.title, "SaidToday"));
            if (field != null)
            {
                field.value = "True";
            }

            field = picked.fields.Find(f => string.Equals(f.title, "SaidThisGame"));
            if (field != null)
            {
                field.value = "True";
            }

            Data.SaidToday.Add(picked);
            DialogueManager.StartConversation(picked.Title);
        }
        void Meet()
        {
            if (ConversationData == null)
            {
                return;
            }

            Conversation negativeMeeting = null;
            Conversation neutralMeeting = null;
            Conversation positiveMeeting = null;

            foreach (Conversation meeting in ConversationData.Meetings)
            {
                float minAffection = meeting.LookupFloat("MinAffection");
                if (minAffection >= 2)
                {
                    positiveMeeting = meeting;
                }
                else if (minAffection < 1)
                {
                    negativeMeeting = meeting;
                }
                else
                {
                    neutralMeeting = meeting;
                }
            }
            float compatibility = CheckCompatibility();
            if (compatibility < 1)
            {
                GetComponent<NPC>().ChangeAffection(-5);
                DialogueManager.StartConversation(negativeMeeting.Title);
            }
            else if (compatibility >= 2)
            {
                GetComponent<NPC>().ChangeAffection(5);
                DialogueManager.StartConversation(positiveMeeting.Title);

            }
            else
            {
                DialogueManager.StartConversation(neutralMeeting.Title);
            }
        }

        public void Greet()
        {
            Data.GreetedToday = true;
            GreetingChangeAffection(1);
            GreetingChangeAcquaintance(1);
            print(Data.CurrentAcquaintance);
            print(Data.CurrentAffection);
        }

        void GreetingChangeAffection(int pAmount)
        {
            float amount = pAmount;
            CheckCompatibility();
            amount *= CompatibilityMultiplier;
            if (amount >= 0)
            {
                amount *= AffectionRaiseMultiplier;
            }
            else
            {
                amount *= AffectionLowerMultiplier;
            }

            amount = Mathf.Clamp(amount, GreetingMaxAffectionLoss, GreetingMaxAffectionGain);
            ChangeAffection(amount);
        }

        public void ChangeAffection(float pAmount)
        {
            float amt = pAmount;
            Data.CurrentAffection = Mathf.Clamp(Data.CurrentAffection + amt, MinAffection, MaxAffection);
        }

        public void ChangeAcquaintance(float pAmount)
        {
            float amt = pAmount;
            Data.CurrentAcquaintance = Mathf.Clamp(Data.CurrentAcquaintance + amt, 0f, MaxAcquaintance);
        }

        void GreetingChangeAcquaintance(int pAmount)
        {
            float amount = pAmount;

            if (amount >= 0)
            {
                amount *= AcquaintanceRaiseMultiplier;
            }
            else
            {
                amount *= AcquaintanceLowerMultiplier;
            }
            // print("changing acquaintance: " + amount);
            Data.CurrentAcquaintance += amount;
        }

        public float CheckCompatibility()
        {
            PlayerCharacter player = GameManager.Instance.Player;
            float compatibility = 1;
            foreach (NPCTraitScriptableObject trait in Traits)
            {
                float traitCompatibility = 0;
                foreach (PlayerTraitScriptableObject playerTrait in player.Traits)
                {
                    if (trait.Compatibilities != null)
                    {
                        foreach (NPCTraitCompatibility traitCompat in trait.Compatibilities)
                        {
                            if (traitCompat.Trait.Name == playerTrait.Name)
                            {
                                traitCompatibility += traitCompat.Modifier;
                            }
                        }
                    }

                    // print(trait.Name + " " + traitCompatibility);
                }
                compatibility += traitCompatibility;
            }
            compatibility = Mathf.Clamp(compatibility, minCompatibility, maxCompatibility);
            CompatibilityMultiplier = compatibility;

            return compatibility;

        }

    }
}