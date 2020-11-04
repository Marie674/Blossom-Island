using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.SceneManagement;
using System.Linq;
using Game.NPCs.Blossoms;

namespace Game.NPCs
{

    public enum CharacterDirection
    {
        Down,
        Left,
        Right,
        Up

    }
    public class NPCManager : Singleton<NPCManager>
    {

        public NPC[] NPCs = new NPC[0];

        public NPCData[] NPCDummies = new NPCData[0];

        public List<NPCData> SpawnedNPCs = new List<NPCData>();

        public List<NPCData> SpawnedDummyNPCs = new List<NPCData>();
        void OnEnable()
        {

            GameManager.OnSceneChanged += SpawnLevelNPCs;
            TimeManager.OnHourChanged += SpawnLevelNPCs;
            TimeManager.OnDayChanged += SetupDailyData;
        }

        void OnDisable()
        {
            GameManager.OnSceneChanged -= SpawnLevelNPCs;
            TimeManager.OnHourChanged -= SpawnLevelNPCs;
            TimeManager.OnDayChanged -= SetupDailyData;

        }

        void SetupDailyData(int pCurrentDay)
        {
            foreach (NPC npc in NPCs)
            {
                if (npc.ConversationData != null && npc.Data != null)
                {
                    npc.ConversationData.DailyConversationLocations = npc.ConversationData.ConversationDailyFilter(npc.Data);
                }
            }
        }
        void SpawnLevelNPCs()
        {

            if (SpawnedNPCs.Count > 0)
            {
                for (int i = 0; i < SpawnedNPCs.Count; i++)
                {
                    NPCData npc = SpawnedNPCs[i];
                    npc.GetCurrentPosition();
                    if (npc.SlotChanged)
                    {
                        RemoveSpawnedNPC(SpawnedNPCs[i].NPCID);
                    }
                }
            }

            SpawnedNPCs = SpawnedNPCs.Where(x => x != null).ToList();

            foreach (NPC NPC in NPCs)
            {
                //                print(NPC.NPCID);
                if (NPC.Data == null)
                {
                    continue;
                }
                NPC.Data.GetCurrentPosition();

                if (SceneManager.GetActiveScene().name == NPC.Data.CurrentLevel)
                {
                    SpawnNPCInLevel(NPC.Data.NPCID, NPC.Data.CurrentPosition, NPC.Data.CurrentFacing.ToString(), true);
                }
            }
        }

        List<GameObject> EventNPCs = new List<GameObject>();

        public void SpawnEventNPCs(List<EventNPCLocation> pNPCs)
        {
            EventNPCs.Clear();

            foreach (EventNPCLocation npc in pNPCs)
            {
                NPCData newNPC = Instantiate(GetNPCDummy(npc.NPCID), npc.Position, transform.rotation);
                newNPC.GetComponent<AnimationController>().ChangeFacing(npc.Facing);
                EventNPCs.Add(newNPC.gameObject);
            }

        }
        public void DeSpawnEventNPCSs()
        {
            for (int i = 0; i < EventNPCs.Count; i++)
            {
                Destroy(EventNPCs[i]);
            }
            EventNPCs.Clear();
        }
        public void ChangeNPCAffection(float pAmount, string pNPCID)
        {
            GetNPC(pNPCID).GetComponent<NPC>().ChangeAffection(pAmount);
        }
        public void ChangeNPCAcquaintance(float pAmount, string pNPCID)
        {
            GetNPC(pNPCID).GetComponent<NPC>().ChangeAcquaintance(pAmount);

        }
        NPCData GetNPC(string pNPCID)
        {
            foreach (NPC NPC in NPCs)
            {
                if (NPC.Data != null && NPC.Data.NPCID == pNPCID)
                {
                    return NPC.Data;
                }
            }
            return null;
        }

        NPCData GetNPCDummy(string pNPCID)
        {
            foreach (NPCData NPC in NPCDummies)
            {
                if (NPC.NPCID == pNPCID)
                {
                    return NPC;
                }
            }
            return null;
        }

        NPCData GetSpawnedNPC(string pNPCID)
        {
            foreach (NPCData NPC in SpawnedNPCs)
            {
                if (NPC.NPCID == pNPCID)
                {
                    return NPC;
                }
            }
            return null;
        }


        NPCData GetSpawnedDummyNPC(string pNPCID)
        {
            foreach (NPCData NPC in SpawnedDummyNPCs)
            {
                if (NPC.NPCID == pNPCID)
                {
                    return NPC;
                }
            }
            return null;
        }

        public void RemoveSpawnedNPC(string pNPCID)
        {
            NPCData NPC = GetSpawnedNPC(pNPCID);
            if (NPC == null)
            {
                return;
            }
            //            print("removing: " + pNPCID);
            SpawnedNPCs.Remove(NPC);
            SpawnedNPCs = SpawnedNPCs.Where(x => x != null).ToList();

            DestroyImmediate(NPC.gameObject, true);

        }

        public void RemoveSpawnedDummyNPC(string pNPCID)
        {
            NPCData NPC = GetSpawnedDummyNPC(pNPCID);
            if (NPC == null)
            {
                return;
            }
            //            print("removing: " + pNPCID);
            SpawnedDummyNPCs.Remove(NPC);
            SpawnedDummyNPCs = SpawnedDummyNPCs.Where(x => x != null).ToList();
            Destroy(NPC.gameObject);

        }
        public void SpawnNPCInLevel(string pNPCID, Vector2 pPosition, string pFacing, bool UpdateDataLocation = false)
        {
            NPCData NPC = GetNPC(pNPCID);

            foreach (NPCData npc in SpawnedNPCs)
            {
                if (npc.NPCID == NPC.NPCID)
                {
                    return;
                }
            }
            NPCData newNPC = Instantiate<NPCData>(NPC, pPosition, transform.rotation);

            CharacterDirection facing = (CharacterDirection)System.Enum.Parse(typeof(CharacterDirection), pFacing);
            newNPC.GetComponent<AnimationController>().ChangeFacing(facing);
            SpawnedNPCs.Add(newNPC);
            //            print("Spawning: " + newNPC.NPCID);

            if (UpdateDataLocation)
            {
                ChangeNPCDataLocation(pNPCID, SceneManager.GetActiveScene().name, pPosition);

            }
        }

        public void SpawnDummyNPC(string pNPCID, Vector2 pPosition, string pFacing)
        {
            NPCData NPC = GetNPCDummy(pNPCID);
            print(NPC.NPCID);
            NPCData newNPC = Instantiate<NPCData>(NPC, pPosition, transform.rotation);
            CharacterDirection facing = (CharacterDirection)System.Enum.Parse(typeof(CharacterDirection), pFacing);
            newNPC.GetComponent<AnimationController>().ChangeFacing(facing);
            SpawnedDummyNPCs.Add(newNPC);
        }

        void ChangeNPCDataLocation(string pNPCID, string pLevel, Vector2 pPosition)
        {
            NPCData NPC = GetNPC(pNPCID);
            NPC.CurrentLevel = pLevel;
            NPC.CurrentPosition = pPosition;
        }
    }
}
