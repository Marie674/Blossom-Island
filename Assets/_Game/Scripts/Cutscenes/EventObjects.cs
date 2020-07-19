using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventObjects : MonoBehaviour
{

    public string Event;
    public GameObject Container;

    void OnEnable()
    {
        if (CalendarManager.Instance.CurrentEvent.Name == Event)
        {
            Container.SetActive(true);
        }
        else
        {
            Container.SetActive(false);
        }
    }
}
