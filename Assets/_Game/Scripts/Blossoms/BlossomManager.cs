using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.SceneManagement;

namespace Game.Blossoms
{

    public class BlossomManager : Singleton<BlossomManager>
    {

        public int MaxHuts = 10;

        public int HutAmount = 0;

        public BlossomData BlossomPrefab;
        public Hut HutPrefab;

        public BlossomData BlossomStoreDummy;
        public int CurrentBlossomID = 0;
        public List<string> OwnedBlossoms = new List<string>();
        public List<string> ExistingBlossoms = new List<string>();
        public int OwnedBlossomAmount;
        public int ExistingBlossomAmount;

        Dictionary<string, BlossomData> SpawnedBlossoms = new Dictionary<string, BlossomData>();

        public List<string> BlossomHuts = new List<string>();

        void Start()
        {
            string hut = "Hut25.588.0";
            DialogueLua.SetVariable(hut + "X", 25.48631);
            DialogueLua.SetVariable(hut + "Y", 89.13724);
            AddHut(hut);
        }


        public void GiveStarterBlossom()
        {
            string starter = CreateStarterBlossom();
            OwnedBlossoms.Add(starter);
            DialogueLua.SetVariable(starter + "Name", "Crogne");
            DialogueLua.SetVariable(starter + "CurrentLevel", "Home");
            DialogueLua.SetVariable(starter + "Age", 7);
            DialogueLua.SetVariable(starter + "CurrentX", 17);
            DialogueLua.SetVariable(starter + "CurrentY", 82);
            DialogueLua.SetVariable(starter + "Growth", BlossomData.BlossomGrowth.Baby);
            string hut = "Hut25.588.0";
            DialogueLua.SetVariable(hut + "Blossom", starter);
            DialogueLua.SetVariable(starter + "HutX", 25.48631);
            DialogueLua.SetVariable(starter + "HutY", 89.13724);
            DialogueLua.SetVariable(starter + "HutName", hut);
            SpawnLevelBlossoms();
        }

        void OnEnable()
        {
            GameManager.OnSceneChanged += SpawnLevelBlossoms;
            GameManager.OnSceneChanged += SpawnHuts;
            SpawnedBlossoms.Clear();
            BlossomData[] allBlossoms = FindObjectsOfType<BlossomData>();
            foreach (BlossomData blossom in allBlossoms)
            {
                SpawnedBlossoms.Add(blossom.ID, blossom);
            }
        }

        void OnDisable()
        {
            GameManager.OnSceneChanged -= SpawnHuts;

            GameManager.OnSceneChanged -= SpawnLevelBlossoms;
        }

        public BlossomData GetSpawnedBlossom(string pID)
        {
            foreach (KeyValuePair<string, BlossomData> blossom in SpawnedBlossoms)
            {
                if (blossom.Key == pID)
                {
                    return blossom.Value;
                }
            }
            return null;
        }

        public void SpawnHuts()
        {
            if (SceneManager.GetActiveScene().name == "Home")
            {
                foreach (string hut in BlossomHuts)
                {
                    float hutX = DialogueLua.GetVariable(hut + "X").asFloat;
                    float hutY = DialogueLua.GetVariable(hut + "Y").asFloat;
                    Vector2 hutPosition = new Vector2(hutX, hutY);
                    Instantiate(HutPrefab, hutPosition, transform.rotation);

                }
                //if (AstarPath.active != null)
                //{
                //    AstarPath.active.Scan();
                //}
            }
            HutAmount = BlossomHuts.Count;

        }

        public void AddHut(string pName)
        {
            if (!BlossomHuts.Contains(pName))
            {
                BlossomHuts.Add(pName);
                HutAmount = BlossomHuts.Count;
                PixelCrushers.MessageSystem.SendMessage(this, "PlaceHut", pName);
            }

        }

        public void RemoveHut(string pName)
        {
            BlossomHuts.Remove(pName);
            HutAmount = BlossomHuts.Count;
        }

        public string GetEmptyHut()
        {
            foreach (string hut in BlossomHuts)
            {
                string containedBlossom = DialogueLua.GetVariable(hut + "Blossom").AsString;
                if (containedBlossom == string.Empty)
                {
                    return hut;
                }
            }
            return null;
        }

        public Hut GetHutObject(string pName)
        {
            Hut[] huts = FindObjectsOfType<Hut>();
            foreach (Hut hut in huts)
            {
                if (hut.Name == pName)
                {
                    return hut;
                }
            }
            return null;
        }
        public void SpawnLevelBlossoms()
        {

            SpawnedBlossoms.Clear();

            foreach (string blossom in ExistingBlossoms)
            {
                string blossomCurrentLevel = DialogueLua.GetVariable(blossom + "CurrentLevel").asString;
                // print(blossomCurrentLevel);
                float blossomCurrentX = DialogueLua.GetVariable(blossom + "CurrentX").asFloat;
                float blossomCurrentY = DialogueLua.GetVariable(blossom + "CurrentY").asFloat;
                Vector2 blossomPosition = new Vector2(blossomCurrentX, blossomCurrentY);
                if (SceneManager.GetActiveScene().name == blossomCurrentLevel)
                {
                    if (DialogueLua.GetVariable(blossom + "ForSale").asBool == true)
                    {
                        BlossomData newBlossom = Instantiate(BlossomStoreDummy, blossomPosition, transform.rotation);
                        newBlossom.GetComponent<BlossomDataSaver>().ApplyPersistentData(blossom);
                        SpawnedBlossoms.Add(blossom, newBlossom);
                    }
                    else
                    {
                        BlossomData newBlossom = Instantiate(BlossomPrefab, blossomPosition, transform.rotation);
                        newBlossom.GetComponent<BlossomDataSaver>().ApplyPersistentData(blossom);
                        SpawnedBlossoms.Add(blossom, newBlossom);
                    }

                }
            }

        }

        public void SpawnBlossom(string pBlossomID, Vector2 pPosition)
        {
            if (ExistingBlossoms.Contains(pBlossomID) == false)
            {
                return;
            }
            if (SpawnedBlossoms.ContainsKey(pBlossomID) == true)
            {
                DespawnBlossom(pBlossomID);
            }
            BlossomData newBlossom = Instantiate(BlossomPrefab, pPosition, transform.rotation);
            newBlossom.GetComponent<BlossomDataSaver>().ApplyPersistentData(pBlossomID);
            SpawnedBlossoms.Add(pBlossomID, newBlossom);
        }

        public void DespawnBlossom(string pBlossomID)
        {
            BlossomData spawnedBlossom;
            if (SpawnedBlossoms.TryGetValue(pBlossomID, out spawnedBlossom) == false)
            {
                return;
            }

            print(SpawnedBlossoms[pBlossomID].ID);
            if (SpawnedBlossoms[pBlossomID] != null)
            {
                Destroy(SpawnedBlossoms[pBlossomID].gameObject);

                SpawnedBlossoms.Clear();
                BlossomData[] allBlossoms = FindObjectsOfType<BlossomData>();
                foreach (BlossomData blossom in allBlossoms)
                {
                    spawnedBlossom = null;

                    if (SpawnedBlossoms.TryGetValue(pBlossomID, out spawnedBlossom) == false)
                    {
                        SpawnedBlossoms.Add(blossom.ID, blossom);

                    }
                }
            }


        }
        public string CreateChildBlossom(string pParent1, string pParent2)
        {
            //CREATE BLOSSOM

            BlossomData newBlossom = Instantiate(BlossomPrefab, transform);
            newBlossom.ID = "Blossom" + CurrentBlossomID.ToString();
            string ID = newBlossom.ID;
            CurrentBlossomID += 1;
            newBlossom.Age = 0;
            newBlossom.Name = GetRandomBlossomName();



            newBlossom.Parent1 = pParent1;
            newBlossom.Parent2 = pParent2;

            newBlossom.Energy = 2;

            //STATS
            //Agility
            newBlossom.Agility.Value = 0;
            newBlossom.Agility.Potential = SetChildStat(pParent1, pParent2, "AgilityPotential");
            newBlossom.Agility.LearningSpeed = SetChildStat(pParent1, pParent2, "AgilityLearningSpeed");
            //Strength
            newBlossom.Strength.Value = 0;
            newBlossom.Strength.Potential = SetChildStat(pParent1, pParent2, "StrengthPotential");
            newBlossom.Strength.LearningSpeed = SetChildStat(pParent1, pParent2, "StrengthLearningSpeed");
            //Intellect
            newBlossom.Intellect.Value = 0;
            newBlossom.Intellect.Potential = SetChildStat(pParent1, pParent2, "IntellectPotential");
            newBlossom.Intellect.LearningSpeed = SetChildStat(pParent1, pParent2, "IntellectLearningSpeed");
            //Charm
            newBlossom.Charm.Value = 0;
            newBlossom.Charm.Potential = SetChildStat(pParent1, pParent2, "CharmPotential");
            newBlossom.Charm.LearningSpeed = SetChildStat(pParent1, pParent2, "CharmLearningSpeed");


            //TRAITS
            string dominantParent;
            int rand = Random.Range(0, 100);
            if (rand <= 50)
            {
                dominantParent = pParent2;
            }
            else
            {
                dominantParent = pParent1;
            }
            Dictionary<Trait, int> parentTraits = new Dictionary<Trait, int>();

            List<Trait> parent1Traits = new List<Trait>();
            int parent1TraitAmount = DialogueLua.GetVariable(pParent1 + "TraitAmount").asInt;
            for (int i = 0; i < parent1TraitAmount; i++)
            {
                string traitName = DialogueLua.GetVariable(pParent1 + "Trait" + i).asString;
                parent1Traits.Add(Resources.Load<Trait>("BlossomTraits/" + traitName));
            }

            List<Trait> parent2Traits = new List<Trait>();
            int parent2TraitAmount = DialogueLua.GetVariable(pParent2 + "TraitAmount").asInt;
            for (int i = 0; i < parent1TraitAmount; i++)
            {
                string traitName = DialogueLua.GetVariable(pParent2 + "Trait" + i).asString;
                parent1Traits.Add(Resources.Load<Trait>("BlossomTraits/" + traitName));
            }

            List<Trait> dominantTraits = new List<Trait>();
            List<Trait> nonDominantTraits = new List<Trait>();
            if (dominantParent == pParent1)
            {
                dominantTraits = parent1Traits;
                nonDominantTraits = parent2Traits;
            }
            else
            {
                dominantTraits = parent2Traits;
                nonDominantTraits = parent1Traits;
            }

            foreach (Trait trait in dominantTraits)
            {
                if (!parentTraits.ContainsKey(trait))
                    parentTraits.Add(trait, trait.Probabilty * 2);
            }

            foreach (Trait trait in nonDominantTraits)
            {
                if (!parentTraits.ContainsKey(trait))
                {
                    parentTraits.Add(trait, trait.Probabilty);

                }
                else
                {
                    parentTraits[trait] += trait.Probabilty;
                }
            }

            rand = Random.Range(1, 2);
            for (int i = 0; i < rand; i++)
            {
                Trait trait = WeightedRandomizer.From(parentTraits).TakeOne();
                if (!newBlossom.Traits.Contains(trait))
                {
                    newBlossom.Traits.Add(trait);

                }
            }



            //STAT POTENTIAL BONUS DISTRIBUTION
            for (int potentialBonus = 10; potentialBonus > 0; potentialBonus--)
            {
                List<string> availableStats = new List<string>();
                availableStats.Add("AgilityPotential");
                availableStats.Add("StrengthPotential");
                availableStats.Add("IntellectPotential");
                availableStats.Add("CharmPotential");

                if (availableStats.Count > 0)
                {
                    rand = Random.Range(0, availableStats.Count);
                    float currentVal = DialogueLua.GetVariable(newBlossom + availableStats[rand]).asFloat;
                    DialogueLua.SetVariable(newBlossom + availableStats[rand], currentVal + 1);
                }
                else
                {
                    break;
                }
            }

            //STAT LEARNING SPEED BONUS DISTRIBUTION
            for (int learningSpeedBonus = 2; learningSpeedBonus > 0; learningSpeedBonus--)
            {
                List<string> availableStats = new List<string>();

                availableStats.Add("AgilityLearningSpeed");
                availableStats.Add("StrengthLearningSpeed");
                availableStats.Add("IntellectLearningSpeed");
                availableStats.Add("CharmLearningSpeed");

                if (availableStats.Count > 0)
                {
                    rand = Random.Range(0, availableStats.Count);
                    float currentVal = DialogueLua.GetVariable(newBlossom + availableStats[rand]).asFloat;
                    DialogueLua.SetVariable(newBlossom + availableStats[rand], currentVal + 0.1f);
                }
                else
                {
                    break;
                }
            }


            string parent1Color = DialogueLua.GetVariable(pParent1 + "Color").asString;
            string parent2Color = DialogueLua.GetVariable(pParent2 + "Color").asString;

            BlossomColorMix[] allMixes = Resources.LoadAll<BlossomColorMix>("BlossomColorMixes");
            BlossomColorMix matchingMix = null;
            foreach (BlossomColorMix mix in allMixes)
            {
                if ((mix.Input1.Name == parent1Color && mix.Input2.Name == parent2Color) || (mix.Input1.Name == parent2Color && mix.Input2.Name == parent1Color))
                {
                    matchingMix = mix;
                }
            }
            if (matchingMix != null)
            {
                Dictionary<string, int> outputs = new Dictionary<string, int>();
                foreach (ColorOutput output in matchingMix.Outputs)
                {
                    outputs.Add(output.Output.Name, output.Output.Probability);
                }
                string color = WeightedRandomizer.From(outputs).TakeOne();
                newBlossom.Color = color;
            }
            else
            {
                Dictionary<string, int> parentColors = new Dictionary<string, int>();
                BlossomColor color1 = Resources.Load<BlossomColor>("BlossomColors/" + parent1Color);
                BlossomColor color2 = Resources.Load<BlossomColor>("BlossomColors/" + parent2Color);

                parentColors.Add(color1.Name, color1.Probability);
                if (!parentColors.ContainsKey(color2.Name))
                {
                    parentColors.Add(color2.Name, color2.Probability);

                }
                string color = WeightedRandomizer.From(parentColors).TakeOne();
                newBlossom.Color = color;
            }


            //DESTROY OBJECT
            Destroy(newBlossom.gameObject);
            return ID;
        }

        float SetChildStat(string pParent1, string pParent2, string pStat)
        {
            float parent1Value = DialogueLua.GetVariable(pParent1 + pStat).asFloat;
            float parent2Value = DialogueLua.GetVariable(pParent2 + pStat).asFloat;

            int rand = Random.Range(0, 100);
            float value = 0;
            if (rand <= 50)
            {
                value = (parent1Value + parent2Value) / 2;
            }
            else
            {
                rand = Random.Range(0, 100);

                if (rand <= 50)
                {
                    value = parent1Value;
                }
                else
                {
                    value = parent2Value;

                }
            }
            return value;
        }

        public string CreateRandomBlossom(string pName, BlossomData.BlossomGrowth pGrowth, float pPotentialMin, float pPotentialMax, float pSpeedMin, float pSpeedMax, int pAge, int pTraitMinAmount, int pTraitMaxAmount, string pCurrentLevel, Vector2 pCurrentPosition, bool pOwned = false)
        {
            //CREATE BLOSSOM
            BlossomData newBlossom = Instantiate(BlossomPrefab, transform);
            newBlossom.ID = "Blossom" + CurrentBlossomID.ToString();
            string ID = newBlossom.ID;
            CurrentBlossomID += 1;
            //newBlossom.Age = pAge;
            newBlossom.Name = pName;
            newBlossom.Parent1 = string.Empty;
            newBlossom.Parent2 = string.Empty;
            newBlossom.Growth = pGrowth;
            newBlossom.Energy = 2;

            //STATS
            //Agility
            newBlossom.Agility.Value = 0;
            newBlossom.Agility.Potential = Random.Range(pPotentialMin, pPotentialMax);
            newBlossom.Agility.LearningSpeed = Random.Range(pSpeedMin, pSpeedMax);
            //Strength
            newBlossom.Strength.Value = 0;
            newBlossom.Strength.Potential = Random.Range(pPotentialMin, pPotentialMax);
            newBlossom.Strength.LearningSpeed = Random.Range(pSpeedMin, pSpeedMax);
            //Intellect
            newBlossom.Intellect.Value = 0;
            newBlossom.Intellect.Potential = Random.Range(pPotentialMin, pPotentialMax);
            newBlossom.Intellect.LearningSpeed = Random.Range(pSpeedMin, pSpeedMax);
            //Charm
            newBlossom.Charm.Value = 0;
            newBlossom.Charm.Potential = Random.Range(pPotentialMin, pPotentialMax);
            newBlossom.Charm.LearningSpeed = Random.Range(pSpeedMin, pSpeedMax);


            //TRAITS
            Trait[] allTraits = Resources.LoadAll<Trait>("BlossomTraits");
            Dictionary<Trait, int> traitPool = new Dictionary<Trait, int>();
            foreach (Trait trait in allTraits)
            {
                traitPool.Add(trait, trait.Probabilty);
            }
            int rand = Random.Range(pTraitMinAmount, pTraitMinAmount);
            for (int i = 0; i < rand; i++)
            {
                Trait trait = WeightedRandomizer.From(traitPool).TakeOne();
                if (!newBlossom.Traits.Contains(trait))
                {
                    newBlossom.Traits.Add(trait);

                }
            }

            newBlossom.CurrentLevel = pCurrentLevel;
            newBlossom.transform.position = pCurrentPosition;

            if (pOwned == true)
            {
                OwnedBlossoms.Add(newBlossom.ID);
            }
            ExistingBlossoms.Add(newBlossom.ID);


            BlossomColor[] allColors = Resources.LoadAll<BlossomColor>("BlossomColors");
            Dictionary<string, int> colorPool = new Dictionary<string, int>();
            foreach (BlossomColor color in allColors)
            {
                colorPool.Add(color.Name, color.Probability);
            }
            newBlossom.Color = WeightedRandomizer.From(colorPool).TakeOne();

            //DESTROY OBJECT
            Destroy(newBlossom.gameObject);
            return ID;
        }


        public string CreateCompetitor(string pName, BlossomData.BlossomGrowth pGrowth, float pMinStrength, float pMaxStrength, float pMinAgility,
                                        float pMaxAgility, float pMinIntellect, float pMaxIntellect, float pMinCharm, float pMaxCharm, int pMinAffection = -1, int pMaxAffection = 1)
        {
            //CREATE BLOSSOM
            BlossomData newBlossom = Instantiate(BlossomPrefab, transform);
            newBlossom.ID = "Blossom" + CurrentBlossomID.ToString();
            string ID = newBlossom.ID;
            CurrentBlossomID += 1;
            //newBlossom.Age = pAge;
            newBlossom.Name = pName;
            newBlossom.Parent1 = string.Empty;
            newBlossom.Parent2 = string.Empty;
            newBlossom.Growth = pGrowth;
            newBlossom.Energy = 2;

            //STATS
            //Agility
            newBlossom.Agility.Value = Random.Range(pMinAgility, pMaxAgility);

            //Strength
            newBlossom.Strength.Value = Random.Range(pMinStrength, pMaxStrength);

            //Intellect
            newBlossom.Intellect.Value = Random.Range(pMinIntellect, pMaxIntellect);

            //Charm
            newBlossom.Charm.Value = Random.Range(pMinCharm, pMaxCharm);

            newBlossom.CurrentLevel = string.Empty;

            newBlossom.transform.position = Vector2.zero;

            //ExistingBlossoms.Add(newBlossom.ID);

            BlossomColor[] allColors = Resources.LoadAll<BlossomColor>("BlossomColors");
            Dictionary<string, int> colorPool = new Dictionary<string, int>();
            foreach (BlossomColor color in allColors)
            {
                colorPool.Add(color.Name, color.Probability);
            }
            newBlossom.Color = WeightedRandomizer.From(colorPool).TakeOne();


            if (pMinAffection != -1 && pMaxAffection != -1 && pMaxAffection >= pMinAffection)
            {
                int affection = Random.Range(pMinAffection, pMaxAffection);
                newBlossom.Affection = affection;
            }
            //DESTROY OBJECT, THIS WILL SAVE DATA
            Destroy(newBlossom.gameObject);
            return ID;
        }


        public string CreateStarterBlossom()
        {
            string name = GetRandomBlossomName();
            string newBlossom = CreateRandomBlossom(name, BlossomData.BlossomGrowth.Baby, 20, 20, 0.5f, 0.5f, 0, 1, 2, string.Empty, new Vector2(0, 0), false);

            Dictionary<string, int> colorPool = new Dictionary<string, int>();
            BlossomColor blue = Resources.Load<BlossomColor>("BlossomColors/Blue");

            colorPool.Add(blue.Name, blue.Probability);
            BlossomColor red = Resources.Load<BlossomColor>("BlossomColors/Red");
            colorPool.Add(red.Name, red.Probability);
            BlossomColor yellow = Resources.Load<BlossomColor>("BlossomColors/Yellow");
            colorPool.Add(yellow.Name, yellow.Probability);
            BlossomColor green = Resources.Load<BlossomColor>("BlossomColors/Green");
            colorPool.Add(green.Name, green.Probability);

            string color = WeightedRandomizer.From(colorPool).TakeOne();
            DialogueLua.SetVariable(newBlossom + "Color", color);
            //STAT POTENTIAL BONUS DISTRIBUTION
            for (int potentialBonus = 30; potentialBonus > 0; potentialBonus--)
            {
                List<string> availableStats = new List<string>();
                if (DialogueLua.GetVariable(newBlossom + "AgilityPotential").asFloat < 40)
                {
                    availableStats.Add("AgilityPotential");
                }
                if (DialogueLua.GetVariable(newBlossom + "StrengthPotential").asFloat < 40)
                {
                    availableStats.Add("StrengthPotential");
                }
                if (DialogueLua.GetVariable(newBlossom + "IntellectPotential").asFloat < 40)
                {
                    availableStats.Add("IntellectPotential");
                }
                if (DialogueLua.GetVariable(newBlossom + "CharmPotential").asFloat < 40)
                {
                    availableStats.Add("CharmPotential");
                }
                if (availableStats.Count > 0)
                {
                    int rand = Random.Range(0, availableStats.Count);
                    float currentVal = DialogueLua.GetVariable(newBlossom + availableStats[rand]).asFloat;
                    DialogueLua.SetVariable(newBlossom + availableStats[rand], currentVal + 1);
                }
                else
                {
                    break;
                }
            }

            //STAT LEARNING SPEED BONUS DISTRIBUTION
            for (int learningSpeedBonus = 5; learningSpeedBonus > 0; learningSpeedBonus--)
            {
                List<string> availableStats = new List<string>();

                availableStats.Add("AgilityLearningSpeed");
                availableStats.Add("StrengthLearningSpeed");
                availableStats.Add("IntellectLearningSpeed");
                availableStats.Add("CharmLearningSpeed");

                if (availableStats.Count > 0)
                {
                    int rand = Random.Range(0, availableStats.Count);
                    float currentVal = DialogueLua.GetVariable(newBlossom + availableStats[rand]).asFloat;
                    DialogueLua.SetVariable(newBlossom + availableStats[rand], currentVal + 0.1f);
                }
                else
                {
                    break;
                }
            }


            return newBlossom;
        }

        public void RemoveBlossom(string pID)
        {
            if (ExistingBlossoms.Contains(pID))
            {
                ExistingBlossoms.Remove(pID);
            }
            if (OwnedBlossoms.Contains(pID))
            {
                OwnedBlossoms.Remove(pID);
            }

        }

        public string GetRandomBlossomName()
        {
            int rand = Random.Range(0, BlossomNames.Count);
            return BlossomNames[rand];
        }
        public List<string> BlossomNames = new List<string>()
        {
            "Baby",
            "Kiwi",
            "Berry",
            "Capi",
            "Choco",
            "Rono",
            "Honey",
            "Appletart",
            "Mielleux",
            "Fizz",
            "Buzz",
            "Mr. Sir",
            "Lily",
            "Bruh",
            "Seymour",
            "Coconut",
            "Dr. Mustache",
            "Fluff",
            "Rainbow",
            "Sunshine",
            "Newton",
            "Komo",
            "Sha",
            "Gustave",
            "Berlioz",
            "Gaston",
            "Giggle",
            "Snort",
            "Pistachio",
            "Cookie",
            "Trip Hazard",
            "Speedy",
            "Laurent",
            "Kim",
            "Cibou",
            "Zorino",
            "Zack",
            "Neil",
            "Toepads",
            "Larry",
            "Pat",
            "Cocotte",
            "Charlie",
            "Norbert",
            "Chu",
            "Theo",
            "Bubbles"
        };
    }

}