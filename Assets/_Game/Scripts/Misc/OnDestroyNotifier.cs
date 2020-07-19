using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDestroyNotifier : MonoBehaviour
{

    public delegate void ObjectDestroy();
    public event ObjectDestroy OnObjectDetroyed;

    void OnDestroy()
    {
        if (OnObjectDetroyed != null)
        {
            OnObjectDetroyed();
        }
    }
}
