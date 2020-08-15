using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

namespace Game.NPCs
{


    public class NPCConversationPicker : MonoBehaviour
    {
        public string Name;
        public Actor Actor;
        private bool Met = false;
        private bool GreetedToday = false;

        public float Affection = 0;
        public float Acquaintance = 0;

        [SerializeField]
        public List<NPCTraitScriptableObject> Traits;

        //CONVERSATIONS

        [SerializeField]
        public List<Conversation> AllConversations = new List<Conversation>();

        [SerializeField]
        public List<Conversation> DayGenericConversations;
        [SerializeField]
        public List<Conversation> DayPriorityGenericConversations;
        List<Conversation> SituationPool;

        [SerializeField]
        public List<Conversation> SituationConversations;

        [SerializeField]
        public List<Conversation> SituationPriorityConversations;
        Dictionary<Conversation, int> CurrentConversations;
        public List<Conversation> SaidToday;

        [SerializeField]
        public List<Conversation> CycleList;
        // END CONVERSATIONS
        private DialogueDatabase DialogueDB;
        void Start()
        {
            DialogueDB = DialogueManager.Instance.masterDatabase;
            Actor = DialogueDB.GetActor(Name);
            GetCharacterConversations();
            GetDailyPool();
        }

        void OnEnable()
        {
            TimeManager.OnDayChanged += DayChange;
        }

        void OnDisable()
        {
            TimeManager.OnDayChanged -= DayChange;
        }

        public void DayChange(int pDayIndex)
        {
            GreetedToday = false;
            SaidToday.Clear();
            CycleList.Clear();
            AllConversations.Clear();
            GetCharacterConversations();
            AllConversations = ValidateConversations(AllConversations);
            GetDailyPool();
        }


        public void Meet()
        {
            AllConversations.Clear();
            GetCharacterConversations();
            List<Conversation> meetings = new List<Conversation>();
            foreach (Conversation conversation in AllConversations)
            {
                if (conversation.LookupBool("isMeeting") == true)
                {
                    meetings.Add(conversation);
                }
            }
            Conversation negativeMeeting = null;
            Conversation neutralMeeting = null;
            Conversation positiveMeeting = null;
            print("Meetings: " + meetings.Count);
            foreach (Conversation meeting in meetings)
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
            float compatibility = GetComponent<NPC>().CheckCompatibility();
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
        private void GetCharacterConversations()
        {
            List<Conversation> conversations = DialogueDB.conversations;

            foreach (Conversation conversation in conversations)
            {
                Actor conversationActor = DialogueDB.GetActor(conversation.ActorID);

                if (conversationActor != Actor)
                {
                    continue;
                }
                //            print(conversation.Title);
                Field field = conversation.fields.Find(f => string.Equals(f.title, "SaidToday"));
                field.value = "False";

                AllConversations.Add(conversation);

            }
        }

        private bool CheckIfGeneric(Conversation pConversation)
        {
            if (pConversation.LookupValue("Time") != "")
            {
                return false;
            }
            if (pConversation.LookupValue("Weather") != "")
            {
                return false;
            }
            if (pConversation.LookupValue("Season") != "")
            {
                return false;
            }
            if (pConversation.LookupValue("Area") != "")
            {
                return false;
            }
            return true;
        }

        private List<Conversation> ValidateConversations(List<Conversation> pConversations)
        {
            print(pConversations.Count);
            List<Conversation> validConversations = new List<Conversation>();
            foreach (Conversation conversation in pConversations)
            {
                if (conversation.LookupBool("isMeeting") == true || conversation.LookupBool("isGreeting") == true)
                {
                    continue;
                }

                if (conversation.LookupFloat("MinAffection") > Affection || conversation.LookupFloat("MaxAffection") < Affection)
                {
                    continue;
                }

                if (conversation.LookupFloat("MinAcquaintance") > Acquaintance || conversation.LookupFloat("MaxAcquaintance") < Acquaintance)
                {
                    continue;
                }

                if (DialogueManager.ConversationHasValidEntry(conversation.Title) == false)
                {
                    continue;
                }
                if (conversation.LookupBool("SayOncePerGame") == true && conversation.LookupBool("SaidThisGame") == true)
                {
                    continue;
                }
                if (conversation.LookupBool("SayOncePerDay") == true && conversation.LookupBool("SaidToday") == true)
                {
                    continue;
                }
                validConversations.Add(conversation);

            }
            return validConversations;
        }


        private void GetDailyPool()
        {
            //SHUFFLE CONVERSATIONS
            AllConversations = ShuffleConversations(AllConversations);
            //        print("Shuffled " + AllConversations.Count);
            //DRAW GENERIC CONVERSATIONS FOR THE DAY
            DrawDayGenericConversations();

            //SHUFFLE CONVERSATIONS
            AllConversations = ShuffleConversations(AllConversations);
            //        print("Shuffled " + AllConversations.Count);

            //DRAW PRIORITY GENERIC CONVERSATIONS FOR THE DAY
            DrawDayPriorityGenericConversations();
        }

        void RePopulateGeneric()
        {


            List<Conversation> generic = new List<Conversation>();
            List<Conversation> genericP = new List<Conversation>();

            foreach (Conversation conversation in SaidToday)
            {
                if (conversation.LookupValue("Time") != ""
                || conversation.LookupValue("Season") != ""
                || conversation.LookupValue("Weather") != ""
                || conversation.LookupValue("Area") != ""
                )
                {
                    break;
                }
                if (conversation.LookupInt("Priority") == 0)
                {
                    generic.Add(conversation);
                }
                else
                {
                    genericP.Add(conversation);
                }
            }
            DayGenericConversations = generic;
            SituationPriorityConversations = genericP;
        }




        //GETS: ALL generic (non-situational), priority 2 conversations AND, if 4 slots are not filled 4 or less generic (non-situational), priority 1 conversations

        int MaxGenPrioConvos = 4;
        private void DrawDayPriorityGenericConversations()
        {

            //        print(AllConversations.Count);
            DayPriorityGenericConversations = new List<Conversation>();
            Dictionary<Conversation, int> priority1 = new Dictionary<Conversation, int>();
            foreach (Conversation conversation in AllConversations)
            {
                if (CheckIfGeneric(conversation) == false)
                {
                    //               print("Not generic");
                    continue;
                }
                if (conversation.LookupInt("Priority") == 0)
                {
                    //               print("Priority<1");
                    continue;
                }

                if (conversation.LookupInt("Priority") == 2)
                {
                    //               print("Priority 2. Adding.");
                    DayPriorityGenericConversations.Add(conversation);
                    continue;
                }
                else if (conversation.LookupInt("Priority") == 1)
                {
                    //              print("Adding to priority 1 list");
                    priority1.Add(conversation, conversation.LookupInt("Weight"));
                    continue;
                }
            }

            int amt = MaxGenPrioConvos - DayPriorityGenericConversations.Count;
            if (amt < 0) { amt = 0; }
            //      print("Getting priority 1: " + amt);
            for (int i = 0; i < amt; i++)
            {
                if (priority1.Count > 0)
                {
                    Conversation conversation = WeightedRandomizer.From(priority1).TakeOne();
                    DayPriorityGenericConversations.Add(conversation);
                    priority1.Remove(conversation);
                    //               print("Adding conversation to priority generic list.");
                }
                else
                {
                    //               print("No more eligible conversations");
                    break;
                }
            }
            //        print("Generic priority conversations: " + DayPriorityGenericConversations.Count);
        }

        //GETS: 4 generic (non-situational), non-priority conversations
        int maxGenConvos = 4;

        private void DrawDayGenericConversations()
        {
            DayGenericConversations = new List<Conversation>();
            Dictionary<Conversation, int> generic = new Dictionary<Conversation, int>();

            foreach (Conversation conversation in AllConversations)
            {
                //            print(conversation.Title);
                if (CheckIfGeneric(conversation) == false)
                {
                    //                print("Not generic");
                    continue;
                }
                if (conversation.LookupInt("Priority") != 0)
                {
                    //                print("Priority>0");
                    continue;
                }
                generic.Add(conversation, conversation.LookupInt("Weight"));
                //           print("Add to generic: " + conversation.Title);
            }

            int amt = maxGenConvos - DayGenericConversations.Count;
            if (amt < 0) { amt = 0; }
            //       print("Adding conversations: " + amt);
            for (int i = 0; i < amt; i++)
            {
                if (generic.Count < 1)
                {
                    //               print("No more eligible conversations");
                    break;
                }
                Conversation conversation = WeightedRandomizer.From(generic).TakeOne();
                DayGenericConversations.Add(conversation);
                // AllConversations.Remove(conversation);
                generic.Remove(conversation);
                //            print("Adding to list");
            }
            //       print("Generated generic conversations for the day: " + DayGenericConversations.Count);
        }

        private void GetSituationPool()
        {
            SituationPool = new List<Conversation>();
            //SHUFFLE CONVERSATIONS
            AllConversations = ShuffleConversations(AllConversations);

            //DRAW SITUATIONAL CONVERSATIONS
            DrawSituationConversations();

            // //SHUFFLE CONVERSATIONS
            AllConversations = ShuffleConversations(AllConversations);

            // //DRAW PRIORITY SITUATIONAL CONVERSATIONS
            DrawPrioritySituationConversations();

        }

        private List<Conversation> ShuffleConversations(List<Conversation> pList)
        {
            List<Conversation> conversationList = ListExtension.Shuffle<Conversation>(pList);
            return conversationList;
        }


        //GETS: 4 situational, non-priority conversations: criterias: area, season, weather, time. Gets: 1 match 4 (if any exist), 1 match 3 (if any exists), 1 match 2 (if any exists)
        //Fills any empty slots with match 1 (if any)

        int MaxSitConvos = 4;

        Dictionary<Conversation, int> areaList = new Dictionary<Conversation, int>();
        Dictionary<Conversation, int> timeList = new Dictionary<Conversation, int>();
        Dictionary<Conversation, int> seasonList = new Dictionary<Conversation, int>();
        Dictionary<Conversation, int> weatherList = new Dictionary<Conversation, int>();

        Dictionary<Conversation, int> combo2 = new Dictionary<Conversation, int>();
        Dictionary<Conversation, int> combo3 = new Dictionary<Conversation, int>();
        Dictionary<Conversation, int> combo4 = new Dictionary<Conversation, int>();
        private void DrawSituationConversations()
        {

            //       print("Drawing situation");
            SituationConversations = new List<Conversation>();

            PopulateSituations(SaidToday);

            ////////////////////////////////
            //Draw desired amount from the sorted lists, based on weighted priorities
            for (int i = 0; i < MaxSitConvos; i++)
            {
                if (combo4.Count > 0)
                {
                    Conversation conversation = WeightedRandomizer.From(combo4).TakeOne();
                    SituationConversations.Add(conversation);
                    combo4.Remove(conversation);
                    if (SituationConversations.Count > MaxSitConvos)
                    {
                        return;
                    }
                }
                if (combo3.Count > 0)
                {
                    Conversation conversation = WeightedRandomizer.From(combo3).TakeOne();
                    SituationConversations.Add(conversation);
                    combo3.Remove(conversation);
                    if (SituationConversations.Count > MaxSitConvos)
                    {
                        return;
                    }
                }
                if (combo2.Count > 0)
                {
                    Conversation conversation = WeightedRandomizer.From(combo2).TakeOne();
                    SituationConversations.Add(conversation);
                    combo2.Remove(conversation);

                    if (SituationConversations.Count > MaxSitConvos)
                    {
                        return;
                    }
                }
                if (areaList.Count > 0)
                {
                    Conversation conversation = WeightedRandomizer.From(areaList).TakeOne();
                    SituationConversations.Add(conversation);
                    areaList.Remove(conversation);

                    if (SituationConversations.Count > MaxSitConvos)
                    {
                        return;
                    }
                }
                if (weatherList.Count > 0)
                {
                    Conversation conversation = WeightedRandomizer.From(weatherList).TakeOne();
                    SituationConversations.Add(conversation);
                    weatherList.Remove(conversation);

                    if (SituationConversations.Count > MaxSitConvos)
                    {
                        return;
                    }
                }
                if (timeList.Count > 0)
                {
                    Conversation conversation = WeightedRandomizer.From(timeList).TakeOne();
                    SituationConversations.Add(conversation);
                    timeList.Remove(conversation);

                    if (SituationConversations.Count > MaxSitConvos)
                    {
                        return;
                    }
                }
                if (seasonList.Count > 0)
                {
                    Conversation conversation = WeightedRandomizer.From(seasonList).TakeOne();
                    SituationConversations.Add(conversation);
                    seasonList.Remove(conversation);

                    if (SituationConversations.Count > MaxSitConvos)
                    {
                        return;
                    }
                }
            }
            PopulateSituations(AllConversations);

            ////////////////////////////////
            //Draw desired amount from the sorted lists, based on weighted priorities

            //       print("Adding from all");

            for (int i = 0; i < MaxSitConvos; i++)
            {
                if (combo4.Count > 0)
                {
                    Conversation conversation = WeightedRandomizer.From(combo4).TakeOne();
                    SituationConversations.Add(conversation);
                    combo4.Remove(conversation);
                    if (SituationConversations.Count > MaxSitConvos)
                    {
                        return;
                    }
                }
                if (combo3.Count > 0)
                {
                    Conversation conversation = WeightedRandomizer.From(combo3).TakeOne();
                    SituationConversations.Add(conversation);
                    combo3.Remove(conversation);
                    if (SituationConversations.Count > MaxSitConvos)
                    {
                        return;
                    }
                }
                if (combo2.Count > 0)
                {
                    Conversation conversation = WeightedRandomizer.From(combo2).TakeOne();
                    SituationConversations.Add(conversation);
                    combo2.Remove(conversation);

                    if (SituationConversations.Count > MaxSitConvos)
                    {
                        return;
                    }
                }
                if (areaList.Count > 0)
                {
                    Conversation conversation = WeightedRandomizer.From(areaList).TakeOne();
                    SituationConversations.Add(conversation);
                    areaList.Remove(conversation);

                    if (SituationConversations.Count > MaxSitConvos)
                    {
                        return;
                    }
                }
                if (weatherList.Count > 0)
                {
                    Conversation conversation = WeightedRandomizer.From(weatherList).TakeOne();
                    SituationConversations.Add(conversation);
                    weatherList.Remove(conversation);

                    if (SituationConversations.Count > MaxSitConvos)
                    {
                        return;
                    }
                }
                if (timeList.Count > 0)
                {
                    Conversation conversation = WeightedRandomizer.From(timeList).TakeOne();
                    SituationConversations.Add(conversation);
                    timeList.Remove(conversation);

                    if (SituationConversations.Count > MaxSitConvos)
                    {
                        return;
                    }
                }
                if (seasonList.Count > 0)
                {
                    Conversation conversation = WeightedRandomizer.From(seasonList).TakeOne();
                    SituationConversations.Add(conversation);
                    seasonList.Remove(conversation);

                    if (SituationConversations.Count > MaxSitConvos)
                    {
                        return;
                    }
                }
            }

            //        print(SituationConversations.Count);
        }

        void PopulateSituations(List<Conversation> conversations)
        {

            combo2.Clear();
            combo3.Clear();
            combo4.Clear();
            timeList.Clear();
            weatherList.Clear();
            seasonList.Clear();
            areaList.Clear();
            foreach (Conversation conversation in conversations)
            {

                //           print(conversation.Title);
                string area = conversation.LookupValue("Area");
                //           print("Area: " + area);

                string timeString = conversation.LookupValue("Time");
                //            print("Time: " + timeString);
                string[] split = timeString.Split(char.Parse(","));
                List<string> times = new List<string>();
                foreach (string sub in split)
                {
                    times.Add(sub);
                    //                print(sub);
                }

                string season = conversation.LookupValue("Season");
                //            print("Season: " + season);

                string weather = conversation.LookupValue("Weather");
                //           print("Weather: " + weather);

                //Don't handle priority conversations
                if (conversation.LookupInt("Priority") != 0)
                {
                    //               print("priority>0");
                    continue;
                }
                //Don't handle generic conversations
                if (CheckIfGeneric(conversation) == true)
                {
                    //                print("Generic");
                    continue;
                }

                //Look for matches
                int matches = 0;
                if (area == FindObjectOfType<PlayerCharacter>().GetCurrentArea())
                {
                    //                print("Area match: " + FindObjectOfType<PlayerCharacter>().GetCurrentArea());
                    matches += 1;
                }
                if (times.Contains(DayPhaseManager.Instance.CurrentDayPhase.Name.ToString()))
                {
                    //                print("Time match: " + DayPhaseManager.Instance.CurrentDayPhase.Name.ToString());
                    matches += 1;

                }
                if (season == TimeManager.Instance.CurrentMonth.Name.ToString())
                {
                    //                print("Season match: " + TimeManager.Instance.CurrentMonth.Name.ToString());
                    matches += 1;
                }
                if (weather == WeatherManager.Instance.CurrentWeather.Name.ToString())
                {
                    //                print("Weather match: " + WeatherManager.Instance.CurrentWeather.Name.ToString());
                    matches += 1;
                }
                //If conversation doesn't match any situation params, don't add
                if (matches == 0)
                {
                    //                print("No matches");
                    continue;
                }
                //get all situations that match 1 parameter, sort them by param
                else if (matches == 1)
                {
                    if (area == FindObjectOfType<PlayerCharacter>().GetCurrentArea())
                    {
                        areaList.Add(conversation, conversation.LookupInt("Weight"));
                        continue;
                    }
                    if (times.Contains(DayPhaseManager.Instance.CurrentDayPhase.Name.ToString()))
                    {
                        timeList.Add(conversation, conversation.LookupInt("Weight"));
                        continue;

                    }
                    if (season == TimeManager.Instance.CurrentMonth.Name.ToString())
                    {
                        seasonList.Add(conversation, conversation.LookupInt("Weight"));
                        continue;
                    }
                    if (weather == WeatherManager.Instance.CurrentWeather.Name.ToString())
                    {
                        weatherList.Add(conversation, conversation.LookupInt("Weight"));
                        continue;
                    }
                }
                //Get all conversations that match 2,3 or 4 params, add them to separate lists
                else
                {
                    if (matches == 2)
                    {
                        combo2.Add(conversation, conversation.LookupInt("Weight"));
                        continue;
                    }
                    else if (matches == 3)
                    {
                        combo3.Add(conversation, conversation.LookupInt("Weight"));
                        continue;
                    }
                    else if (matches == 4)
                    {
                        combo4.Add(conversation, conversation.LookupInt("Weight"));
                        continue;
                    }
                }
            }
        }

        //GETS: 4 priority, situational conversations
        int MaxSitPrioConvos = 4;
        private void DrawPrioritySituationConversations()
        {
            SituationPriorityConversations = new List<Conversation>();

            int amt = MaxSitPrioConvos - SituationPriorityConversations.Count;
            if (amt < 0) { amt = 0; }

            PopulatePrioritySituations(SaidToday);

            for (int i = 0; i < amt; i++)
            {
                if (combo4.Count > 0 && combo4.Count < 1)
                {
                    Conversation conversation = WeightedRandomizer.From(combo4).TakeOne();
                    SituationPriorityConversations.Add(conversation);
                    if (SituationPriorityConversations.Count > MaxSitConvos)
                    {
                        return;
                    }
                }
                if (combo3.Count > 0 && combo3.Count < 1)
                {
                    Conversation conversation = WeightedRandomizer.From(combo3).TakeOne();
                    SituationPriorityConversations.Add(conversation);
                    if (SituationPriorityConversations.Count > MaxSitConvos)
                    {
                        return;
                    }
                }
                if (combo2.Count > 0 && combo2.Count < 1)
                {
                    Conversation conversation = WeightedRandomizer.From(combo2).TakeOne();
                    SituationPriorityConversations.Add(conversation);
                    if (SituationPriorityConversations.Count > MaxSitConvos)
                    {
                        return;
                    }
                }
                if (areaList.Count > 0 && areaList.Count < 1)
                {
                    Conversation conversation = WeightedRandomizer.From(areaList).TakeOne();
                    SituationPriorityConversations.Add(conversation);
                    if (SituationPriorityConversations.Count > MaxSitConvos)
                    {
                        return;
                    }
                }
                if (weatherList.Count > 0 && weatherList.Count < 1)
                {
                    Conversation conversation = WeightedRandomizer.From(weatherList).TakeOne();
                    SituationPriorityConversations.Add(conversation);
                    if (SituationPriorityConversations.Count > MaxSitConvos)
                    {
                        return;
                    }
                }
                if (timeList.Count > 0 && timeList.Count < 1)
                {
                    Conversation conversation = WeightedRandomizer.From(timeList).TakeOne();
                    SituationPriorityConversations.Add(conversation);
                    if (SituationPriorityConversations.Count > MaxSitConvos)
                    {
                        return;
                    }
                }
                if (seasonList.Count > 0 && seasonList.Count < 1)
                {
                    Conversation conversation = WeightedRandomizer.From(seasonList).TakeOne();
                    SituationPriorityConversations.Add(conversation);
                    if (SituationPriorityConversations.Count > MaxSitConvos)
                    {
                        return;
                    }
                }

            }

            PopulatePrioritySituations(AllConversations);

            for (int i = 0; i < amt; i++)
            {
                if (combo4.Count > 0 && combo4.Count < 1)
                {
                    Conversation conversation = WeightedRandomizer.From(combo4).TakeOne();
                    SituationPriorityConversations.Add(conversation);
                    if (SituationPriorityConversations.Count > MaxSitConvos)
                    {
                        return;
                    }
                }
                if (combo3.Count > 0 && combo3.Count < 1)
                {
                    Conversation conversation = WeightedRandomizer.From(combo3).TakeOne();
                    SituationPriorityConversations.Add(conversation);
                    if (SituationPriorityConversations.Count > MaxSitConvos)
                    {
                        return;
                    }
                }
                if (combo2.Count > 0 && combo2.Count < 1)
                {
                    Conversation conversation = WeightedRandomizer.From(combo2).TakeOne();
                    SituationPriorityConversations.Add(conversation);
                    if (SituationPriorityConversations.Count > MaxSitConvos)
                    {
                        return;
                    }
                }
                if (areaList.Count > 0 && areaList.Count < 1)
                {
                    Conversation conversation = WeightedRandomizer.From(areaList).TakeOne();
                    SituationPriorityConversations.Add(conversation);
                    if (SituationPriorityConversations.Count > MaxSitConvos)
                    {
                        return;
                    }
                }
                if (weatherList.Count > 0 && weatherList.Count < 1)
                {
                    Conversation conversation = WeightedRandomizer.From(weatherList).TakeOne();
                    SituationPriorityConversations.Add(conversation);
                    if (SituationPriorityConversations.Count > MaxSitConvos)
                    {
                        return;
                    }
                }
                if (timeList.Count > 0 && timeList.Count < 1)
                {
                    Conversation conversation = WeightedRandomizer.From(timeList).TakeOne();
                    SituationPriorityConversations.Add(conversation);
                    if (SituationPriorityConversations.Count > MaxSitConvos)
                    {
                        return;
                    }
                }
                if (seasonList.Count > 0 && seasonList.Count < 1)
                {
                    Conversation conversation = WeightedRandomizer.From(seasonList).TakeOne();
                    SituationPriorityConversations.Add(conversation);
                    if (SituationPriorityConversations.Count > MaxSitConvos)
                    {
                        return;
                    }
                }

            }
        }


        void PopulatePrioritySituations(List<Conversation> conversations)
        {

            combo2.Clear();
            combo3.Clear();
            combo4.Clear();
            timeList.Clear();
            weatherList.Clear();
            seasonList.Clear();
            areaList.Clear();

            foreach (Conversation conversation in conversations)
            {
                if (conversation.LookupInt("Priority") == 0)
                {
                    continue;
                }
                if (
                    CheckIfGeneric(conversation) == true
                )
                {
                    continue;
                }

                if (conversation.LookupInt("Priority") == 2)
                {
                    SituationPriorityConversations.Add(conversation);
                    continue;
                }
                else if (conversation.LookupInt("Priority") == 1)
                {
                    string area = conversation.LookupValue("Area");

                    string timeString = conversation.LookupValue("Time");

                    List<string> times = new List<string>();
                    if (timeString != "")
                    {
                        string[] split = timeString.Split(char.Parse(","));
                        foreach (string sub in split)
                        {
                            times.Add(sub);
                        }
                    }



                    string season = conversation.LookupValue("Season");
                    string weather = conversation.LookupValue("Weather");

                    //priority1.Add(conversation, conversation.LookupInt("Weight"));

                    int matches = 0;
                    if (area == FindObjectOfType<PlayerCharacter>().GetCurrentArea())
                    {
                        matches += 1;
                    }
                    if (times.Contains(DayPhaseManager.Instance.CurrentDayPhase.Name.ToString()))
                    {
                        matches += 1;

                    }
                    if (season == TimeManager.Instance.CurrentMonth.Name.ToString())
                    {
                        matches += 1;
                    }
                    if (weather == WeatherManager.Instance.CurrentWeather.Name.ToString())
                    {
                        matches += 1;
                    }

                    if (matches == 0)
                    {
                        continue;
                    }
                    else if (matches == 1)
                    {
                        if (area == FindObjectOfType<PlayerCharacter>().GetCurrentArea())
                        {
                            areaList.Add(conversation, conversation.LookupInt("Weight"));
                            continue;
                        }
                        if (times.Contains(DayPhaseManager.Instance.CurrentDayPhase.Name.ToString()))
                        {
                            timeList.Add(conversation, conversation.LookupInt("Weight"));
                            continue;

                        }
                        if (season == TimeManager.Instance.CurrentMonth.Name.ToString())
                        {
                            seasonList.Add(conversation, conversation.LookupInt("Weight"));
                            continue;
                        }
                        if (weather == WeatherManager.Instance.CurrentWeather.Name.ToString())
                        {
                            weatherList.Add(conversation, conversation.LookupInt("Weight"));
                            continue;
                        }
                    }
                    else
                    {
                        if (matches == 2)
                        {
                            combo2.Add(conversation, conversation.LookupInt("Weight"));
                            continue;
                        }
                        else if (matches == 3)
                        {
                            combo3.Add(conversation, conversation.LookupInt("Weight"));
                            continue;
                        }
                        else if (matches == 4)
                        {
                            combo4.Add(conversation, conversation.LookupInt("Weight"));
                            continue;
                        }
                    }
                }
            }
        }


        void ValidateCycleList()
        {
            List<Conversation> valid = new List<Conversation>();

            foreach (Conversation conversation in CycleList)
            {
                valid.Add(conversation);
            }

            foreach (Conversation conversation in CycleList)
            {
                if (!SituationPriorityConversations.Contains(conversation) && !SituationConversations.Contains(conversation)
                && !DayPriorityGenericConversations.Contains(conversation) && !DayGenericConversations.Contains(conversation)
                )
                {
                    valid.Remove(conversation);
                    continue;
                }
            }
            CycleList = valid;
        }

        public void SpeakTo()
        {
            DialogueManager.StopConversation();
            GetSituationPool();

            ValidateCycleList();

            Conversation chosenConversation = null;

            int tries = SituationPriorityConversations.Count + SituationConversations.Count + DayPriorityGenericConversations.Count + DayGenericConversations.Count;

            for (int i = tries; i > 0; i--)
            {
                //            print("try draaaw");
                //draw a conversation based on priorities
                if (SituationPriorityConversations.Count > 0)
                {
                    int rand = Random.Range(0, SituationPriorityConversations.Count);
                    if (!CycleList.Contains(SituationPriorityConversations[rand]))
                    {
                        chosenConversation = SituationPriorityConversations[rand];
                        SituationPriorityConversations.Remove(chosenConversation);
                    }

                }
                if (chosenConversation == null && DayPriorityGenericConversations.Count > 0)
                {
                    int rand = Random.Range(0, DayPriorityGenericConversations.Count);
                    if (!CycleList.Contains(DayPriorityGenericConversations[rand]))
                    {
                        chosenConversation = DayPriorityGenericConversations[rand];
                        DayPriorityGenericConversations.Remove(chosenConversation);
                    }

                }
                if (chosenConversation == null && SituationConversations.Count > 0)
                {
                    int rand = Random.Range(0, SituationConversations.Count);
                    if (!CycleList.Contains(SituationConversations[rand]))
                    {
                        chosenConversation = SituationConversations[rand];
                        SituationConversations.Remove(chosenConversation);
                    }
                }
                if (chosenConversation == null && DayGenericConversations.Count > 0)
                {
                    int rand = Random.Range(0, DayGenericConversations.Count);
                    if (!CycleList.Contains(DayGenericConversations[rand]))
                    {
                        chosenConversation = DayGenericConversations[rand];
                        DayGenericConversations.Remove(chosenConversation);
                    }

                }

                if (chosenConversation == null)
                {
                    //                print("ohno, null. Tries left: " + i);

                }
                else
                {
                    break;
                }
            }

            if (chosenConversation == null)
            {

                //            print("ayyyy");
                CycleList.Clear();
                GetSituationPool();
                RePopulateGeneric();

                ValidateConversations(SituationPriorityConversations);
                ValidateConversations(SituationConversations);
                ValidateConversations(DayGenericConversations);
                ValidateConversations(DayPriorityGenericConversations);

                tries = SituationPriorityConversations.Count + SituationConversations.Count + DayPriorityGenericConversations.Count + DayGenericConversations.Count;
                print(tries);
                for (int i = tries; i > 0; i--)
                {
                    //draw a conversation based on priorities
                    if (SituationPriorityConversations.Count > 0)
                    {
                        int rand = Random.Range(0, SituationPriorityConversations.Count);
                        chosenConversation = SituationPriorityConversations[rand];
                        SituationPriorityConversations.Remove(chosenConversation);

                    }
                    if (chosenConversation == null && DayPriorityGenericConversations.Count > 0)
                    {
                        int rand = Random.Range(0, DayPriorityGenericConversations.Count);
                        chosenConversation = DayPriorityGenericConversations[rand];
                        DayPriorityGenericConversations.Remove(chosenConversation);

                    }
                    if (chosenConversation == null && SituationConversations.Count > 0)
                    {
                        int rand = Random.Range(0, SituationConversations.Count);
                        chosenConversation = SituationConversations[rand];
                        SituationConversations.Remove(chosenConversation);

                    }
                    if (chosenConversation == null && DayGenericConversations.Count > 0)
                    {
                        int rand = Random.Range(0, DayGenericConversations.Count);
                        chosenConversation = DayGenericConversations[rand];
                        DayGenericConversations.Remove(chosenConversation);

                    }
                    if (chosenConversation == null)
                    {
                        //                    print("ohno, null. Tries left: " + i);

                    }
                    else { break; }

                }
            }

            if (chosenConversation == null)
            {
                Debug.LogWarning("NO ELIGIBLE CONVERSATION");
                return;
            }

            CycleList.Add(chosenConversation);

            Field field = chosenConversation.fields.Find(f => string.Equals(f.title, "SaidToday"));
            field.value = "True";

            field = chosenConversation.fields.Find(f => string.Equals(f.title, "SaidThisGame"));
            field.value = "True";

            SaidToday.Add(chosenConversation);

            DialogueManager.Instance.StartConversation(chosenConversation.Title);

        }
    }
}