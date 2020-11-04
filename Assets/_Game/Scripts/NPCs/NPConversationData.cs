using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using UnityEditor;
using System.Linq;

namespace Game.NPCs
{

    [CreateAssetMenu(fileName = "NPCConversationData", menuName = "Data/NPCConversationData")]
    public class NPConversationData : ScriptableObject
    {
        public bool ShowDebug = false;
        public DialogueDatabase DialogueDB;
        public List<Conversation> AllConversations = new List<Conversation>();
        [SerializeField]
        public List<LocationConversationData> ConversationLocations = new List<LocationConversationData>();

        public List<LocationConversationData> DailyConversationLocations = new List<LocationConversationData>();
        public List<Conversation> Meetings = new List<Conversation>();
        public int ActorID = -1;

        [Button("Fetch Conversations")]
        void FetchConversations()
        {
            if (ActorID == -1)
            {
                return;
            }
            if (DialogueDB == null)
            {
                return;
            }
            AllConversations.Clear();
            Meetings.Clear();
            List<Conversation> allConversations = DialogueDB.conversations;

            string[] allLocations = System.Enum.GetNames(typeof(GameManager.GameLocations));
            ConversationLocations.Clear();

            foreach (string location in allLocations)
            {
                LocationConversationData data = new LocationConversationData();
                data.Conversations = new List<Conversation>();
                data.Location = (GameManager.GameLocations)System.Enum.Parse(typeof(GameManager.GameLocations), location);
                ConversationLocations.Add(data);
            }


            foreach (Conversation conversation in allConversations)
            {
                if (conversation.ActorID == ActorID)
                {
                    Field said = conversation.fields.Find(f => string.Equals(f.title, "SaidThisGame"));
                    said.value = "False";
                    bool isMeeting = conversation.LookupBool("isMeeting");
                    if (isMeeting)
                    {
                        Meetings.Add(conversation);
                        continue;
                    }
                    if (conversation.LookupBool("Cutscene"))
                    {
                        continue;
                    }
                    AllConversations.Add(conversation);

                    Field field = conversation.fields.Find(f => string.Equals(f.title, "Locations"));

                    if (field != null)
                    {
                        string conversationLocations = field.value;
                        List<string> locationsList = conversationLocations.Split(',').ToList<string>();
                        if (locationsList.Contains(GameManager.GameLocations.All.ToString()) || field.value == string.Empty)
                        {
                            if (ConversationLocations.Find(x => x.Location == GameManager.GameLocations.All) != null)
                            {
                                LocationConversationData locationData = ConversationLocations.Find(x => x.Location == GameManager.GameLocations.All);
                                locationData.Conversations.Add(conversation);
                            }
                            continue;
                        }
                        else
                        {
                            foreach (LocationConversationData locationData in ConversationLocations)
                            {
                                if (locationsList.Contains(locationData.Location.ToString()))
                                {
                                    locationData.Conversations.Add(conversation);
                                }
                            }
                        }

                    }
                }
            }
        }
        public List<LocationConversationData> ConversationDailyFilter(NPCData pNPC)
        {
            if (ShowDebug)
            {
                Debug.Log("Getting Daily filtered conversations");
            }
            List<LocationConversationData> dataList = new List<LocationConversationData>();

            foreach (LocationConversationData locationData in ConversationLocations)
            {

                //Get conversations that are eligible
                List<Conversation> eligibleConversations = new List<Conversation>();
                LocationConversationData newLocationData = new LocationConversationData();
                newLocationData.Location = locationData.Location;
                newLocationData.Conversations = new List<Conversation>();
                foreach (Conversation conversation in locationData.Conversations)
                {
                    if (ShowDebug)
                    {
                        Debug.Log("Conversation: " + conversation.Title);
                    }
                    Field field = conversation.fields.Find(f => string.Equals(f.title, "SaidToday"));
                    if (field != null)
                    {
                        field.value = "False";

                    }

                    if (conversation.LookupBool("SayOncePerGame"))
                    {
                        if (conversation.LookupBool("SaidThisGame"))
                        {
                            if (ShowDebug)
                            {
                                Debug.Log("SaidThisGame, not adding");
                            }
                            continue;
                        }
                    }
                    if (CheckDailyFilter(conversation, pNPC))
                    {
                        eligibleConversations.Add(conversation);
                    }
                }
                newLocationData.Conversations = eligibleConversations;
                dataList.Add(newLocationData);
            }
            return dataList;
        }

        bool CheckDailyFilter(Conversation pConversation, NPCData pNPC)
        {
            List<string> months = new List<string>();
            if (ShowDebug)
            {
                Debug.Log("Months: " + months);
            }
            string monthString = pConversation.LookupValue("Seasons");

            months = monthString.Split(',').ToList<string>();
            if (monthString != string.Empty && !months.Contains(TimeManager.Instance.CurrentMonth.Name.ToString()))
            {
                if (ShowDebug)
                {
                    Debug.Log("Months not compatible");
                }
                return false;
            }

            string dayspanString = pConversation.LookupValue("DaySpans");
            if (ShowDebug)
            {
                Debug.Log("Day Spans: " + dayspanString);
            }
            if (dayspanString != string.Empty)
            {
                List<string> daySpans = dayspanString.Split('/').ToList<string>();
                foreach (string span in daySpans)
                {
                    if (ShowDebug)
                    {
                        Debug.Log("Span: " + span);
                    }
                    List<string> spantimes = span.Split(',').ToList<string>();
                    if (spantimes.Count < 2)
                    {
                        Debug.Log("Span format error");
                        continue;
                    }
                    int firstDay = int.Parse(spantimes[0]);
                    int lastDay = int.Parse(spantimes[1]);
                    if (ShowDebug)
                    {
                        Debug.Log("Min: " + firstDay.ToString() + " Max: " + lastDay.ToString());
                    }
                    if (TimeManager.Instance.CheckWithinDaySpan(firstDay, lastDay) == false)
                    {
                        if (ShowDebug)
                        {
                            Debug.Log("Days not compatible. Not adding.");
                        }
                        return false;
                    }
                }
            }

            int minYear = pConversation.LookupInt("MinYear");
            int maxYear = pConversation.LookupInt("MaxYear");
            int currentYear = TimeManager.Instance.CurrentYear;
            if (minYear > 0 && currentYear < minYear)
            {
                if (ShowDebug)
                {
                    Debug.Log("Year too low. Not adding.");
                }
                return false;
            }
            if (maxYear > 0 && currentYear > maxYear)
            {
                if (ShowDebug)
                {
                    Debug.Log("Years too high. Not adding.");
                }
                return false;
            }

            string weatherSting = pConversation.LookupValue("Weathers");
            List<string> weathers = weatherSting.Split(',').ToList<string>();
            if (ShowDebug)
            {
                Debug.Log("Weathers: " + weathers.ToString());
            }
            if (weatherSting != string.Empty && !weathers.Contains(WeatherManager.Instance.CurrentWeather.Name.ToString()))
            {
                if (ShowDebug)
                {
                    Debug.Log("Weathers not compatible. Not adding");
                }
                return false;
            }

            float minAffection = pConversation.LookupFloat("MinAffection");
            float maxAffection = pConversation.LookupFloat("MaxAffection");
            if (pNPC.CurrentAffection < minAffection)
            {
                if (ShowDebug)
                {
                    Debug.Log("Affection too low. Not adding");
                }
                return false;
            }
            if (maxAffection > 0 && pNPC.CurrentAffection > maxAffection)
            {
                if (ShowDebug)
                {
                    Debug.Log("Affection too high. Not adding");
                }
                return false;
            }

            float minAcquaintance = pConversation.LookupFloat("MinAcquaintance");
            float maxAcquaintance = pConversation.LookupFloat("MaxAcquaintance");
            if (pNPC.CurrentAcquaintance < minAcquaintance)
            {
                if (ShowDebug)
                {
                    Debug.Log("Acquaintance too low. Not adding");
                }
                return false;
            }
            if (maxAcquaintance > 0 && pNPC.CurrentAcquaintance > maxAcquaintance)
            {
                if (ShowDebug)
                {
                    Debug.Log("Acquaintance too high. Not adding");
                }
                return false;
            }


            return true;
        }

        public List<Conversation> CheckEligibleConversations()
        {
            List<LocationConversationData> locationDatas = CheckLocationFilter();
            List<Conversation> conversations = new List<Conversation>();
            foreach (LocationConversationData locationData in locationDatas)
            {
                foreach (Conversation conversation in locationData.Conversations)
                {
                    if (CheckTimeSpanFilter(conversation) && !conversations.Contains(conversation))
                    {
                        if (conversation.LookupBool("SayOncePerDay"))
                        {
                            if (conversation.LookupBool("SaidToday"))
                            {
                                continue;
                            }
                        }
                        conversations.Add(conversation);
                    }
                }
            }
            return conversations;
        }

        List<LocationConversationData> CheckLocationFilter()
        {
            List<LocationConversationData> locationDatas = new List<LocationConversationData>();
            string locationString = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            GameManager.GameLocations location = (GameManager.GameLocations)System.Enum.Parse((typeof(GameManager.GameLocations)), locationString);
            LocationConversationData allLocations = new LocationConversationData();
            foreach (LocationConversationData locationData in DailyConversationLocations)
            {
                if (locationData.Location == GameManager.GameLocations.All)
                {
                    allLocations = locationData;
                    locationDatas.Add(allLocations);
                }
                if (locationData.Location == location)
                {
                    locationDatas.Add(locationData);
                }
            }
            return locationDatas;
        }

        bool CheckTimeSpanFilter(Conversation pConversation)
        {
            string timeSpanString = pConversation.LookupValue("TimeSpans");
            if (timeSpanString != string.Empty)
            {
                List<string> timeSpans = timeSpanString.Split('/').ToList<string>();
                foreach (string span in timeSpans)
                {
                    List<string> spantimes = span.Split(',').ToList<string>();
                    if (spantimes.Count < 2)
                    {
                        Debug.Log("Span format error");
                        continue;
                    }
                    int startHour = int.Parse(spantimes[0]);
                    int endHour = int.Parse(spantimes[1]);
                    HourSpan hourSpan = new HourSpan();
                    hourSpan.StartHour = startHour;
                    hourSpan.StartMinute = 0;
                    hourSpan.EndHour = endHour;
                    hourSpan.EndMinute = 0;
                    if (TimeManager.Instance.CheckWithinHourSpan(hourSpan, TimeManager.Instance.CurrentHour) == false)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }

    [System.Serializable]
    public class LocationConversationData
    {
        public List<Conversation> Conversations = new List<Conversation>();
        public GameManager.GameLocations Location;
    }


}