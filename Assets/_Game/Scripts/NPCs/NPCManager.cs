using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.SceneManagement;
using System.Linq;

namespace BlossomIsland
{

    public class NPCManager : Singleton<NPCManager>
    {

        NPCData[] NPCs = new NPCData[0];

        public List<NPCData> SpawnedNPCs = new List<NPCData>();
        void OnEnable()
        {
            NPCs = Resources.LoadAll<NPCData>("NPCs");
            GameManager.OnSceneChanged += SpawnLevelNPCs;
            TimeManager.OnHourChanged += SpawnLevelNPCs;
        }

        void OnDisable()
        {
            GameManager.OnSceneChanged -= SpawnLevelNPCs;
            TimeManager.OnHourChanged -= SpawnLevelNPCs;

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

            foreach (NPCData NPC in NPCs)
            {
                print(NPC.NPCID);
                NPC.GetCurrentPosition();

                if (SceneManager.GetActiveScene().name == NPC.CurrentLevel)
                {
                    SpawnNPCInLevel(NPC.NPCID, NPC.CurrentPosition, true);
                }
            }
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
            foreach (NPCData NPC in NPCs)
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

        public void RemoveSpawnedNPC(string pNPCID)
        {
            NPCData NPC = GetSpawnedNPC(pNPCID);
            if (NPC == null)
            {
                return;
            }
            //            print("removing: " + pNPCID);
            SpawnedNPCs.Remove(NPC);
            Destroy(NPC.gameObject);

        }
        public void SpawnNPCInLevel(string pNPCID, Vector2 pPosition, bool UpdateDataLocation = false)
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


            SpawnedNPCs.Add(newNPC);
            //            print("Spawning: " + newNPC.NPCID);

            if (UpdateDataLocation)
            {
                ChangeNPCDataLocation(pNPCID, SceneManager.GetActiveScene().name, pPosition);

            }
        }

        void ChangeNPCDataLocation(string pNPCID, string pLevel, Vector2 pPosition)
        {
            NPCData NPC = GetNPC(pNPCID);
            NPC.CurrentLevel = pLevel;
            NPC.CurrentPosition = pPosition;
        }
    }
}
