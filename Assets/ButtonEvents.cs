using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
public class ButtonEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public Button Button;
    public UnityEvent PointerEnterEvent;
    public UnityEvent PointerExitEvent;
    public UnityEvent PointerClickEvent;

    public UnityEvent PointerClickDisabledEvent;

    void Awake()
    {
        Button = GetComponent<Button>();
    }
    public void OnPointerEnter(PointerEventData pData)
    {
        if (Button.interactable)
        {
            PointerEnterEvent.Invoke();
        }
    }

    public void OnPointerExit(PointerEventData pData)
    {
        PointerExitEvent.Invoke();
    }

    public void OnPointerClick(PointerEventData pData)
    {
        if (Button.interactable)
        {
            PointerClickEvent.Invoke();
        }
        else
        {
            PointerClickDisabledEvent.Invoke();
        }
    }

    public void PlayHoverSFX()
    {
        AkSoundEngine.PostEvent("Play_SFX_BTN_Hover", gameObject);
    }

    public void PlaySelectSFX()
    {
        AkSoundEngine.PostEvent("Play_SFX_BTN_Select", gameObject);

    }

    public void PlaySelectDisabledSFX()
    {
        AkSoundEngine.PostEvent("Play_SFX_BTN_Select_Disabled", gameObject);

    }
}
