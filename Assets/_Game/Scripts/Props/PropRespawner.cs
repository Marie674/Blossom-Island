using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropRespawner : MonoBehaviour
{

    public GameObject Target;
    public GameObject TargetPrefab;

    public List<GameObject> AcceptedObjects;
    public int DaysBeforeRespawn = 2;

    public int DaysPassed;

    void OnEnable()
    {
        // if (Target != null)
        // {
        //     if (Target.GetComponent<OnDestroyNotifier>() != null)
        //     {
        //         Target.GetComponent<OnDestroyNotifier>().OnObjectDetroyed += ObjectDestroyed;
        //     }
        // }
        TimeManager.OnDayChanged += DayChange;
    }

    void OnDisable()
    {
        // if (Target != null)
        // {
        //     if (Target.GetComponent<OnDestroyNotifier>() != null)
        //     {
        //         Target.GetComponent<OnDestroyNotifier>().OnObjectDetroyed -= ObjectDestroyed;
        //     }
        // }
        TimeManager.OnDayChanged -= DayChange;

    }

    // void ObjectDestroyed()
    // {
    //     Target = null;
    //     DaysPassed = 0;

    // }

    public void DayChange(int pDayIndex)
    {
        GetTarget();
    }
    public Collider2D[] GetOverlapObjects()
    {
        BoxCollider2D coll = GetComponent<BoxCollider2D>();
        return Physics2D.OverlapBoxAll(coll.bounds.center, coll.size, 0f);
    }

    public void GetTarget()
    {
        StartCoroutine("DelayedApply");
    }
    IEnumerator DelayedApply()
    {
        yield return new WaitForSeconds(0.1f);


        if (transform.childCount > 0)
        {
            Target = transform.GetChild(0).gameObject;
        }
        else
        {
            Collider2D[] objectOverlaps = GetOverlapObjects();
            foreach (Collider2D collider in objectOverlaps)
            {
                string prefabName = collider.gameObject.name.Replace("(Clone)", string.Empty);
                if (collider.gameObject == gameObject)
                {
                    continue;
                }

                if (AcceptedObjects.Find(x => x != null && string.Equals(x.name, prefabName)) != null && collider.isTrigger == false)
                {
                    collider.transform.parent = transform;
                    if (transform.childCount > 0)
                    {
                        Target = transform.GetChild(0).gameObject;
                    }
                    break;
                }
            }
        }

        if (Target == null)
        {
            DaysPassed += 1;
            if (DaysPassed >= DaysBeforeRespawn)
            {
                Respawn();
            }
        }
    }
    void Respawn()
    {
        DaysPassed = 0;
        Instantiate(TargetPrefab, transform.position, transform.rotation, transform);
        if (transform.childCount > 0)
        {
            Target = transform.GetChild(0).gameObject;

        }
    }
}
