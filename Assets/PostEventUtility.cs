using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostEventUtility : MonoBehaviour
{

    public void PostEvent(string EventName)
    {
        AkSoundEngine.PostEvent(EventName.ToString(), gameObject);
    }
}
