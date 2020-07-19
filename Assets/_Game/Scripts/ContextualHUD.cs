using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextualHUD : MonoBehaviour
{
    public Transform Canvas;

    public virtual void Show()
    {
        if (Canvas != null)
        {
            Canvas.gameObject.SetActive(true);
        }
    }

    public virtual void Hide()
    {
        if (Canvas != null)
        {
            Canvas.gameObject.SetActive(false);
        }
    }
}
