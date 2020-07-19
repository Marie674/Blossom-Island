using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using PixelCrushers;
public class NativeTreeSpawner : MonoBehaviour
{
    public void SpawnTree()
    {
        print(GameManager.Instance.NativeTree.name);
        TreeBase newTree = GameObject.Instantiate(GameManager.Instance.NativeTree, transform.position, transform.rotation);
        newTree.ProduceSeasons = new List<TimeManager.MonthNames>();
        newTree.NativeTree = true;
        newTree.ProduceSeasons.Add(TimeManager.MonthNames.Fall);
        newTree.ProduceSeasons.Add(TimeManager.MonthNames.Spring);
        newTree.ProduceSeasons.Add(TimeManager.MonthNames.Summer);
        newTree.ProduceSeasons.Add(TimeManager.MonthNames.Winter);
        newTree.CurrentProduceGrowth = Random.Range(0, newTree.ProduceGrowthTime);
    }

}
