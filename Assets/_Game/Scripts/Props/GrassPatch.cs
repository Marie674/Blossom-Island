using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GrassPatch : MonoBehaviour
{

    public List<GrassTuft> Children = new List<GrassTuft>();
    private List<GrassTuft> SleepingChildren = new List<GrassTuft>();
    private int AwakeChildren = 0;
    void OnEnable()
    {
        TimeManager.OnDayChanged += Grow;
        SleepingChildren = Children;
        foreach (HarvestObject child in Children)
        {
            child.OnObjectDestroyed += ChildDestroyed;
        }
        Children[0].gameObject.SetActive(true);
        AwakeChildren += 1;
        SleepingChildren.Remove(Children[0]);
    }

    void OnDisable()
    {
        TimeManager.OnDayChanged -= Grow;

        foreach (HarvestObject child in Children)
        {
            child.OnObjectDestroyed -= ChildDestroyed;
        }
    }

    private void ChildDestroyed()
    {
        AwakeChildren -= 1;
        if (AwakeChildren < 1)
        {
            Destroy(gameObject);
        }
    }

    private void Grow(int pDayIndex)
    {
        if (SleepingChildren.Count > 0)
        {
            int rand = Random.Range(0, SleepingChildren.Count - 1);
            SleepingChildren[rand].gameObject.SetActive(true);
            SleepingChildren.Remove(SleepingChildren[rand]);
            AwakeChildren += 1;
        }
    }

}
