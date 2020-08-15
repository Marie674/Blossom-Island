using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

namespace Game.NPCs.Blossoms
{

    public class BlossomSaleSlot : MonoBehaviour
    {

        public string LevelName;

        public int Fee;
        public string ContainedBlossom = string.Empty;


        public void NewDay(int pCurrentDay)
        {
            if (ContainedBlossom != string.Empty)
            {
                BlossomManager.Instance.RemoveBlossom(ContainedBlossom);
                BlossomManager.Instance.DespawnBlossom(ContainedBlossom);
            }
            ContainedBlossom = string.Empty;

            ContainedBlossom = BlossomManager.Instance.CreateStarterBlossom();
            DialogueLua.SetVariable(ContainedBlossom + "ForSale", true);
            DialogueLua.SetVariable(ContainedBlossom + "CurrentLevel", LevelName);
            DialogueLua.SetVariable(ContainedBlossom + "CurrentX", transform.position.x);
            DialogueLua.SetVariable(ContainedBlossom + "CurrentY", transform.position.y);
        }

        public void Interact()
        {
            if (ContainedBlossom != string.Empty)
            {
                BlossomStoreUI blossomStoreUI = FindObjectOfType<BlossomStoreUI>();
                blossomStoreUI.AcceptButton.onClick.AddListener(delegate { BuyBlossom(); blossomStoreUI.Close(); });
                blossomStoreUI.Open(ContainedBlossom, Fee);

            }


        }

        void BuyBlossom()
        {
            GameManager.Instance.Player.GetComponent<PlayerInventory>().ChangeGold(-Fee);
            BlossomManager.Instance.DespawnBlossom(ContainedBlossom);
            string hut = BlossomManager.Instance.GetEmptyHut();

            DialogueLua.SetVariable(hut + "Blossom", ContainedBlossom);
            DialogueLua.SetVariable(ContainedBlossom + "HutX", DialogueLua.GetVariable(hut + "X").asFloat);
            DialogueLua.SetVariable(ContainedBlossom + "HutY", DialogueLua.GetVariable(hut + "Y").asFloat);
            DialogueLua.SetVariable(ContainedBlossom + "HutName", hut);


            DialogueLua.SetVariable(ContainedBlossom + "ForSale", false);
            DialogueLua.SetVariable(ContainedBlossom + "CurrentLevel", "Home");
            DialogueLua.SetVariable(ContainedBlossom + "CurrentX", DialogueLua.GetVariable(hut + "X").asFloat);
            DialogueLua.SetVariable(ContainedBlossom + "CurrentY", DialogueLua.GetVariable(hut + "Y").asFloat);
            BlossomManager.Instance.OwnedBlossoms.Add(ContainedBlossom);
            PixelCrushers.MessageSystem.SendMessage(this, "AdoptBlossom", ContainedBlossom);
            ContainedBlossom = string.Empty;
        }

    }
}
