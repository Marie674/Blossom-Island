using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

namespace Game.Blossoms

{

    [System.Serializable]
    public struct CompetitioniTier
    {
        public string Name;
        public int Tier;
        public int EntryFee;
        public float MinAgility;
        public float MaxAgility;
        public float MinStrength;
        public float MaxStrength;
        public float MinCharm;
        public float MaxCharm;
        public float MinIntellect;
        public float MaxIntellect;
        public int CompetitorMinAffection;
        public int CompetitorMaxAffection;
        public BlossomData.BlossomGrowth Growth;
    }
    public class BlossomCompetitionManager : Singleton<BlossomCompetitionManager>
    {

        //public BlossomController BlossomPrefab;

        public enum CompetitionNames
        {
            Null,
            Race,
            StrengthShow,
            BeautyShow,
            PuzzleContest,
            Pageant,
            Marathon,
            Dance,
            FitnessShow,
            WeightPuzzle,
            Tag,
            FullCourse

        }

        public Dictionary<CompetitionNames, string> CompetitionStrings = new Dictionary<CompetitionNames, string>()
        {
            { CompetitionNames.Null,"Null" },
            { CompetitionNames.Race,"Race" },
            { CompetitionNames.StrengthShow,"Strength Show" },
            { CompetitionNames.BeautyShow,"Beauty Show" },
            { CompetitionNames.PuzzleContest,"Puzzle Contest" },
            { CompetitionNames.Pageant,"Pageant" },
            { CompetitionNames.Marathon,"Marathon" },
            { CompetitionNames.Dance,"Dance" },
            { CompetitionNames.FitnessShow,"Fitness Show" },
            { CompetitionNames.WeightPuzzle,"Weight Puzzle" },
            { CompetitionNames.Tag,"Tag" },
            { CompetitionNames.FullCourse,"Full Course" },
        };

        [SerializeField]
        public List<CompetitioniTier> CompetitionTiers;

        public CompetitioniTier CurrentTier;

        List<string> CurrentCompetitors = new List<string>();

        public int CompetitorAmount = 3;

        public BlossomCompetition CurrentCompetition;

        public List<BlossomCompetition> Competitions = new List<BlossomCompetition>();

        string Blossom = string.Empty;

        Dictionary<string, float> CurrentResults = new Dictionary<string, float>();
        float BlossomResult = 0;

        float maxAffectionLuckMultiplier = 0.2f;
        float maxGenericLuckMultiplier = 0.1f;

        string CompetitionText = string.Empty;

        public Transform CompetitionPresenter;

        public bool CompetitionDone = false;

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
            if (CompetitionDone == true)
                CompetitionDone = false;
        }

        public bool IsBlossomValid(string pID)
        {
            if (CurrentCompetition == null)
            {
                Debug.LogError("No competition selected");
                return false;

            }
            if (CurrentTier.Tier == -1)
            {
                Debug.LogError("No valid tier selected");
                return false;
            }


            if (CurrentCompetition.Stat1 != Stat.StatName.Null)
            {
                float minValue = -1;
                float maxValue = -1;

                switch (CurrentCompetition.Stat1)
                {
                    case Stat.StatName.Strength:
                        minValue = CurrentTier.MinStrength;
                        maxValue = CurrentTier.MaxStrength;
                        break;
                    case Stat.StatName.Agility:
                        minValue = CurrentTier.MinAgility;
                        maxValue = CurrentTier.MaxAgility;
                        break;
                    case Stat.StatName.Intellect:
                        minValue = CurrentTier.MinIntellect;
                        maxValue = CurrentTier.MaxIntellect;
                        break;
                    case Stat.StatName.Charm:
                        minValue = CurrentTier.MinCharm;
                        maxValue = CurrentTier.MaxCharm;
                        break;
                    default:
                        break;
                }
                if (minValue == -1 || maxValue == -1)
                {
                    Debug.LogError("Competition stats not set properly");
                    return false;
                }
                float blossomValue = DialogueLua.GetVariable(pID + CurrentCompetition.Stat1.ToString() + "Value").asFloat;
                if (blossomValue < minValue || blossomValue > maxValue)
                {
                    return false;
                }
            }

            if (CurrentCompetition.Stat2 != Stat.StatName.Null)
            {
                float minValue = -1;
                float maxValue = -1;

                switch (CurrentCompetition.Stat2)
                {
                    case Stat.StatName.Strength:
                        minValue = CurrentTier.MinStrength;
                        maxValue = CurrentTier.MaxStrength;
                        break;
                    case Stat.StatName.Agility:
                        minValue = CurrentTier.MinAgility;
                        maxValue = CurrentTier.MaxAgility;
                        break;
                    case Stat.StatName.Intellect:
                        minValue = CurrentTier.MinIntellect;
                        maxValue = CurrentTier.MaxIntellect;
                        break;
                    case Stat.StatName.Charm:
                        minValue = CurrentTier.MinCharm;
                        maxValue = CurrentTier.MaxCharm;
                        break;
                    default:
                        break;
                }
                if (minValue == -1 || maxValue == -1)
                {
                    Debug.LogError("Competition stats not set properly");
                    return false;
                }
                float blossomValue = DialogueLua.GetVariable(pID + CurrentCompetition.Stat2.ToString() + "Value").asFloat;
                if (blossomValue < minValue || blossomValue > maxValue)
                {
                    return false;
                }
            }

            if (CurrentCompetition.Stat3 != Stat.StatName.Null)
            {
                float minValue = -1;
                float maxValue = -1;

                switch (CurrentCompetition.Stat3)
                {
                    case Stat.StatName.Strength:
                        minValue = CurrentTier.MinStrength;
                        maxValue = CurrentTier.MaxStrength;
                        break;
                    case Stat.StatName.Agility:
                        minValue = CurrentTier.MinAgility;
                        maxValue = CurrentTier.MaxAgility;
                        break;
                    case Stat.StatName.Intellect:
                        minValue = CurrentTier.MinIntellect;
                        maxValue = CurrentTier.MaxIntellect;
                        break;
                    case Stat.StatName.Charm:
                        minValue = CurrentTier.MinCharm;
                        maxValue = CurrentTier.MaxCharm;
                        break;
                    default:
                        break;
                }
                if (minValue == -1 || maxValue == -1)
                {
                    Debug.LogError("Competition stats not set properly");
                    return false;
                }
                float blossomValue = DialogueLua.GetVariable(pID + CurrentCompetition.Stat3.ToString() + "Value").asFloat;
                if (blossomValue < minValue || blossomValue > maxValue)
                {
                    return false;
                }
            }

            if (CurrentCompetition.Stat4 != Stat.StatName.Null)
            {
                float minValue = -1;
                float maxValue = -1;

                switch (CurrentCompetition.Stat4)
                {
                    case Stat.StatName.Strength:
                        minValue = CurrentTier.MinStrength;
                        maxValue = CurrentTier.MaxStrength;
                        break;
                    case Stat.StatName.Agility:
                        minValue = CurrentTier.MinAgility;
                        maxValue = CurrentTier.MaxAgility;
                        break;
                    case Stat.StatName.Intellect:
                        minValue = CurrentTier.MinIntellect;
                        maxValue = CurrentTier.MaxIntellect;
                        break;
                    case Stat.StatName.Charm:
                        minValue = CurrentTier.MinCharm;
                        maxValue = CurrentTier.MaxCharm;
                        break;
                    default:
                        break;
                }
                if (minValue == -1 || maxValue == -1)
                {
                    Debug.LogError("Competition stats not set properly");
                    return false;
                }
                float blossomValue = DialogueLua.GetVariable(pID + CurrentCompetition.Stat4.ToString() + "Value").asFloat;
                if (blossomValue < minValue || blossomValue > maxValue)
                {
                    return false;
                }
            }
            return true;
        }

        public void SetBlossom(string pID)
        {
            if (IsBlossomValid(pID) == false)
            {
                Blossom = string.Empty;
                return;
            }
            Blossom = pID;
        }

        public void ShowTierSelect()
        {
            print("seleeecttt");
            if (CurrentCompetition == null)
            {
                return;
            }
            GameObject.FindObjectOfType<CompetitionTierSelectUI>().Open(CompetitionTiers, CurrentCompetition);
        }

        bool SelectTier(int pTier)
        {

            foreach (CompetitioniTier tier in CompetitionTiers)
            {
                if (tier.Tier == pTier)
                {
                    CurrentTier = tier;
                    return true;
                }
            }
            CurrentTier = new CompetitioniTier();
            CurrentTier.Tier = -1;
            return false;
        }

        public bool SelectCompetition(CompetitionNames pCompetitionName, Transform pPresenter)
        {
            CompetitionPresenter = pPresenter;
            foreach (BlossomCompetition competition in Competitions)
            {
                if (pCompetitionName.ToString() == competition.Name.ToString())
                {
                    CurrentCompetition = competition;
                    return true;
                }
            }
            CurrentCompetition = null;
            return false;
        }
        void GenerateCompetitors()
        {
            CurrentCompetitors.Clear();
            for (int i = 0; i < CompetitorAmount; i++)
            {
                string name = BlossomManager.Instance.GetRandomBlossomName();
                string newBlossom = BlossomManager.Instance.CreateCompetitor(name, CurrentTier.Growth, CurrentTier.MinStrength, CurrentTier.MaxStrength, CurrentTier.MinAgility, CurrentTier.MaxAgility, CurrentTier.MinIntellect,
                     CurrentTier.MaxIntellect, CurrentTier.MinCharm, CurrentTier.MaxCharm, CurrentTier.CompetitorMinAffection, CurrentTier.CompetitorMaxAffection);
                CurrentCompetitors.Add(newBlossom);
            }

        }

        public bool StartCompetition(string pBlossom = null, CompetitionNames pCompetitionName = CompetitionNames.Null, int pTier = -1, Transform pPresenter = null)
        {
            if (pCompetitionName != CompetitionNames.Null)
            {

                SelectCompetition(pCompetitionName, pPresenter);
            }
            if (pTier != -1)
            {
                SelectTier(pTier);
            }
            if (pBlossom != null)
            {
                SetBlossom(pBlossom);
            }


            if (CurrentCompetition == null)
            {
                Debug.LogError("No competition selected");
                return false;

            }
            if (CurrentTier.Tier == -1)
            {
                Debug.LogError("No valid tier selected");
                return false;
            }
            if (Blossom == string.Empty)
            {
                Debug.LogError("No blossom selected");
                return false;
            }

            PixelCrushers.MessageSystem.SendMessage(this, "EnterBlossomCompetition", CurrentCompetition.Name.ToString());
            CurrentResults.Clear();

            GenerateCompetitors();
            ProceedCompetition();

            return true;
        }

        public string GenerateText(int pPos)
        {
            string text = string.Empty;
            string start;
            string result;
            string end;

            int rand = (int)Random.Range(0, CurrentCompetition.StartingText.Count);
            start = CurrentCompetition.StartingText[rand];

            switch (pPos)
            {
                case 0:
                    rand = (int)Random.Range(0, CurrentCompetition.GoodResultText.Count);
                    result = CurrentCompetition.GoodResultText[rand];
                    break;
                case 1:
                    rand = (int)Random.Range(0, CurrentCompetition.GoodResultText.Count);
                    result = CurrentCompetition.GoodResultText[rand];
                    break;
                case 2:
                    rand = (int)Random.Range(0, CurrentCompetition.BadResultText.Count);
                    result = CurrentCompetition.BadResultText[rand];
                    break;
                case 3:
                    rand = (int)Random.Range(0, CurrentCompetition.BadResultText.Count);
                    result = CurrentCompetition.BadResultText[rand];
                    break;
                default:
                    rand = (int)Random.Range(0, CurrentCompetition.BadResultText.Count);
                    result = CurrentCompetition.BadResultText[rand];
                    break;
            }

            rand = (int)Random.Range(0, CurrentCompetition.EndText.Count);
            end = CurrentCompetition.EndText[rand];

            text = start + "\n" + result + "\n" + end;


            return text;
        }

        public void ProceedCompetition()
        {
            List<float> sortedScores = new List<float>();
            float blossomResult = GetBlossomResult(Blossom);
            CurrentResults.Add(Blossom, blossomResult);
            BlossomResult = blossomResult;
            sortedScores.Add(blossomResult);
            foreach (string competitor in CurrentCompetitors)
            {
                float result = GetBlossomResult(competitor);

                if (sortedScores.Contains(result))
                {
                    result = result - 0.0001f;
                }
                sortedScores.Add(result);
                CurrentResults.Add(competitor, result);
            }

            sortedScores.Sort(CompareScores);
            sortedScores.Reverse();

            Dictionary<string, float> newScores = new Dictionary<string, float>();

            foreach (float score in sortedScores)
            {
                foreach (KeyValuePair<string, float> result in CurrentResults)
                {
                    if (result.Value == score)
                    {
                        newScores.Add(result.Key, result.Value);
                    }
                }

            }

            CurrentResults = newScores;
            float maxScore = GetCompetitionMaxScore();
            print("MaxScore: " + maxScore);
            int i = 0;
            int place = -1;
            foreach (KeyValuePair<string, float> result in CurrentResults)
            {
                float score = result.Value;
                score = (score * 10) / maxScore;
                string name = DialogueLua.GetVariable(result.Key + "Name").AsString;
                print(name + ": " + score.ToString("F2") + "/10");
                if (result.Key == Blossom)
                {
                    place = i;
                }
                i++;
            }
            CompetitionText = (GenerateText(place));

            FindObjectOfType<CompetitionProcessWindow>().Open(CompetitionText);
        }

        public void ShowResultsScreen()
        {

            float maxScore = GetCompetitionMaxScore();
            int i = 0;
            int place = -1;

            List<string> results = new List<string>();
            List<string> blossoms = new List<string>();

            foreach (KeyValuePair<string, float> result in CurrentResults)
            {
                float score = result.Value;
                score = (score * 10) / maxScore;
                string name = DialogueLua.GetVariable(result.Key + "Name").AsString;
                blossoms.Add(result.Key);
                if (result.Key == Blossom)
                {
                    results.Add("<b>" + name + ": " + score.ToString("F2") + "/10" + "</b>");
                    place = i;
                }
                else
                {
                    results.Add(name + ": " + score.ToString("F2") + "/10");
                }
                i++;
            }

            FindObjectOfType<BlossomCompetitionResultsUI>().Open(results, blossoms, place, CurrentCompetition.Name + " Rankings");

        }

        public void ShowResultConversation(int pRank)
        {
            DialogueLua.SetVariable("CompetitionRank", pRank);
            DialogueManager.StartConversation("Competition Result", CompetitionPresenter);
            CompetitionDone = true;
        }


        static int CompareScores(float a, float b)
        {
            return a.CompareTo(b);
        }

        float GetCompetitionMaxScore()
        {
            float score = 0;
            float maxLuckMultiplier = maxAffectionLuckMultiplier + maxGenericLuckMultiplier;
            List<Stat.StatName> stats = new List<Stat.StatName>();
            if (CurrentCompetition.Stat1 != Stat.StatName.Null)
            {
                stats.Add(CurrentCompetition.Stat1);
            }
            if (CurrentCompetition.Stat2 != Stat.StatName.Null)
            {
                stats.Add(CurrentCompetition.Stat2);
            }
            if (CurrentCompetition.Stat3 != Stat.StatName.Null)
            {
                stats.Add(CurrentCompetition.Stat3);
            }
            if (CurrentCompetition.Stat4 != Stat.StatName.Null)
            {
                stats.Add(CurrentCompetition.Stat4);
            }

            foreach (Stat.StatName stat in stats)
            {

                switch (stat)
                {
                    case Stat.StatName.Null:
                        break;
                    case Stat.StatName.Strength:
                        score += CurrentTier.MaxStrength + (CurrentTier.MaxStrength * maxLuckMultiplier);
                        break;
                    case Stat.StatName.Agility:
                        score += CurrentTier.MaxAgility + (CurrentTier.MaxAgility * maxLuckMultiplier);

                        break;
                    case Stat.StatName.Intellect:
                        score += CurrentTier.MaxIntellect + (CurrentTier.MaxIntellect * maxLuckMultiplier);

                        break;
                    case Stat.StatName.Charm:
                        score += CurrentTier.MaxCharm + (CurrentTier.MaxCharm * maxLuckMultiplier);
                        break;
                    default:
                        break;
                }

            }
            return score;
        }

        float GetBlossomResult(string pID)
        {
            float score = 0;
            //float luck = Random.Range();
            List<Stat.StatName> stats = new List<Stat.StatName>();
            if (CurrentCompetition.Stat1 != Stat.StatName.Null)
            {
                stats.Add(CurrentCompetition.Stat1);
            }
            if (CurrentCompetition.Stat2 != Stat.StatName.Null)
            {
                stats.Add(CurrentCompetition.Stat2);
            }
            if (CurrentCompetition.Stat3 != Stat.StatName.Null)
            {
                stats.Add(CurrentCompetition.Stat3);
            }
            if (CurrentCompetition.Stat4 != Stat.StatName.Null)
            {
                stats.Add(CurrentCompetition.Stat4);
            }

            foreach (Stat.StatName stat in stats)
            {
                float blossomValue = DialogueLua.GetVariable(pID + stat.ToString() + "Value").asFloat;
                float blossomAffection = DialogueLua.GetVariable(pID + "Affection").asFloat;

                float affectionMultiplier = (blossomAffection / 1000);
                affectionMultiplier = Mathf.Clamp(affectionMultiplier, 0f, maxAffectionLuckMultiplier);

                float genericLuckMultiplier = Random.Range(0, maxGenericLuckMultiplier);
                float affectionLuckMultiplier = Random.Range(0, affectionMultiplier);

                float affectionLuck = 0;
                float genericLuck = 0;
                float totalLuckScore = 0;

                switch (stat)
                {
                    case Stat.StatName.Null:
                        break;
                    case Stat.StatName.Strength:
                        affectionLuck = CurrentTier.MaxStrength * affectionLuckMultiplier;
                        genericLuck = CurrentTier.MaxStrength * genericLuckMultiplier;
                        totalLuckScore = affectionLuck + genericLuck;
                        break;
                    case Stat.StatName.Agility:
                        affectionLuck = CurrentTier.MaxAgility * affectionLuckMultiplier;
                        genericLuck = CurrentTier.MaxAgility * genericLuckMultiplier;
                        totalLuckScore = affectionLuck + genericLuck;
                        break;
                    case Stat.StatName.Intellect:
                        affectionLuck = CurrentTier.MaxIntellect * affectionLuckMultiplier;
                        genericLuck = CurrentTier.MaxIntellect * genericLuckMultiplier;
                        totalLuckScore = affectionLuck + genericLuck;
                        break;
                    case Stat.StatName.Charm:
                        affectionLuck = CurrentTier.MaxCharm * affectionLuckMultiplier;
                        genericLuck = CurrentTier.MaxCharm * genericLuckMultiplier;
                        totalLuckScore = affectionLuck + genericLuck;
                        break;
                    default:
                        break;
                }
                totalLuckScore += blossomValue;

                float statScore = Random.Range(blossomValue, totalLuckScore);
                score += statScore;
            }
            return score;
        }
        float GetCompetitorResult()
        {
            float score = 0;

            return score;
        }

    }
}