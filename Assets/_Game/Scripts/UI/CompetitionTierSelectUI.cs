using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Blossoms
{


public class CompetitionTierSelectUI : MonoBehaviour
{
    public CompetitionTierUI TierUIPrefab;
        public Transform TierContainer;
    public void Open(List<CompetitioniTier> pTiers,BlossomCompetition pCompetition)
    {
            print("pls open");

            CompetitionTierUI[] children = TierContainer.GetComponentsInChildren<CompetitionTierUI>();
            foreach (CompetitionTierUI child in children)
            {
                Destroy(child.gameObject);
            }

            List<Stat.StatName> stats = new List<Stat.StatName>();
            if (pCompetition.Stat1 != Stat.StatName.Null)
            {
                stats.Add(pCompetition.Stat1);
            }
            if (pCompetition.Stat2 != Stat.StatName.Null)
            {
                stats.Add(pCompetition.Stat2);
            }
            if (pCompetition.Stat3 != Stat.StatName.Null)
            {
                stats.Add(pCompetition.Stat3);
            }
            if (pCompetition.Stat4 != Stat.StatName.Null)
            {
                stats.Add(pCompetition.Stat4);
            }
            if (stats.Count <= 0)
            {
                Debug.LogError("Competition Stats not set");
                return;
            }

            foreach (CompetitioniTier tier in pTiers)
            {
                CompetitionTierUI tierUI = Instantiate(TierUIPrefab, TierContainer);
                tierUI.Tier = tier;
                tierUI.TierText.text = tier.Name;
                float minStat = 0;

                float minAgility = -1;
                float maxAgility = -1;
                float minStrength = -1;
                float maxStrength = -1;
                float minIntellect = -1;
                float maxIntellect = -1;
                float minCharm = -1;
                float maxCharm = -1;

                foreach (Stat.StatName stat in stats)
                {
                    minStat += getMinStat(stat,tier);
                    switch (stat)
                    {
                        case Stat.StatName.Null:
                            break;
                        case Stat.StatName.Strength:
                            minStrength = getMinStat(stat, tier);
                            break;
                        case Stat.StatName.Agility:
                            minAgility = getMinStat(stat, tier);
                            break;
                        case Stat.StatName.Intellect:
                            minIntellect = getMinStat(stat, tier);
                            break;
                        case Stat.StatName.Charm:
                            minCharm = getMinStat(stat, tier);
                            break;
                        default:
                            break;
                    }
                }
                minStat = minStat / stats.Count;

                float maxStat = 0;
                foreach (Stat.StatName stat in stats)
                {
                    maxStat += getMaxStat(stat, tier);
                    switch (stat)
                    {
                        case Stat.StatName.Null:
                            break;
                        case Stat.StatName.Strength:
                            maxStrength = getMaxStat(stat, tier);
                            break;
                        case Stat.StatName.Agility:
                            maxAgility = getMaxStat(stat, tier);
                            break;
                        case Stat.StatName.Intellect:
                            maxIntellect = getMaxStat(stat, tier);
                            break;
                        case Stat.StatName.Charm:
                            maxCharm = getMaxStat(stat, tier);
                            break;
                        default:
                            break;
                    }

                }
                maxStat = maxStat / stats.Count;

                tierUI.TierStatText.text = "Stat range:\n" + minStat.ToString("F0") + " - " + maxStat.ToString("F0");
                tierUI.SelectButton.onClick.AddListener(
                    delegate ()
                    {
                        BlossomCompetitionManager.Instance.CurrentTier = tier;
                        GetComponent<WindowToggle>().Close();
                        FindObjectOfType<CompetitionBlossomPickerUI>().Open(minAgility,maxAgility,minStrength,maxStrength,minIntellect,maxIntellect,minCharm,maxCharm,this,pTiers,pCompetition);
                        
                    }

                    );

            }

            GetComponent<WindowToggle>().Open();
    }


        public float getMinStat(Stat.StatName pStat,CompetitioniTier pTier)
        {
            float min = 0;
            switch (pStat)
            {
                case Stat.StatName.Null:
                    break;
                case Stat.StatName.Strength:
                    min += pTier.MinStrength; 
                    break;
                case Stat.StatName.Agility:
                    min += pTier.MinAgility;
                    break;
                case Stat.StatName.Intellect:
                    min += pTier.MinIntellect;
                    break;
                case Stat.StatName.Charm:
                    min += pTier.MinCharm;
                    break;
                default:
                    break;
            }
            return min;
        }

        public float getMaxStat(Stat.StatName pStat, CompetitioniTier pTier)
        {
            float max = 0;
            switch (pStat)
            {
                case Stat.StatName.Null:
                    break;
                case Stat.StatName.Strength:
                    max += pTier.MaxStrength;
                    break;
                case Stat.StatName.Agility:
                    max += pTier.MaxAgility;
                    break;
                case Stat.StatName.Intellect:
                    max += pTier.MaxIntellect;
                    break;
                case Stat.StatName.Charm:
                    max += pTier.MaxCharm;
                    break;
                default:
                    break;
            }
            return max;
        }


        public void Close()
        {
            GetComponent<WindowToggle>().Close();

        }
    }
}