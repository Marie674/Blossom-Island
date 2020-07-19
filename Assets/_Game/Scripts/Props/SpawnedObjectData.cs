using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedObjectData : MonoBehaviour
{
    public string KeyName;

    public delegate void ObjectDestroyed(GameObject pObj);
    public event ObjectDestroyed OnObjectDestroyed;

    void OnDestroy()
    {
        if (OnObjectDestroyed != null)
        {
            OnObjectDestroyed(gameObject);
        }
    }
}
