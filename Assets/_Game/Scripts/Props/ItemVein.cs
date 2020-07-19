using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;
using PixelCrushers.DialogueSystem;
[System.Serializable]
public struct VeinItem
{
    public ItemContainer Item;
    public int Probability;
}
public class ItemVein : MonoBehaviour
{
    public uint RequiredToolLevel = 1;
    [SerializeField]
    public List<VeinItem> ItemOutputs = new List<VeinItem>();
    public ParticleSpawner.ParticleTypes Particle;
    public Transform SpawnPoint;

    public int TriesPerDay = 10;
    public int TriesToday = 0;

    void OnEnable()
    {
        TimeManager.OnDayChanged += ResetTries;
    }

    void OnDisable()
    {
        TimeManager.OnDayChanged -= ResetTries;
    }

    void ResetTries(int pDayIndex)
    {
        TriesToday = 0;
    }

    public void Hit()
    {
        if (TriesToday >= TriesPerDay)
        {
            DialogueManager.ShowAlert("Enough for today");
            return;
        }
        ItemTool tool = ToolManager.Instance.CurrentTool;

        if (RequiredToolLevel > tool.level)
        {
            DialogueManager.ShowAlert("My tool needs to be stronger");

            return;
        }
        Vector3 position = transform.position;
        position.z -= 0.0001f;
        ParticleSpawner.Instance.SpawnOneShot(Particle, position);

        TriesToday += 1;
        var weights = new Dictionary<ItemContainer, int>();


        foreach (VeinItem item in ItemOutputs)
        {
            weights.Add(item.Item, item.Probability);
        }

        ItemContainer pickedItem = WeightedRandomizer.From(weights).TakeOne();
        if (pickedItem != null)
        {
            ItemSpawner.Instance.SpawnItems(pickedItem.item, SpawnPoint.position);

        }


    }
}
