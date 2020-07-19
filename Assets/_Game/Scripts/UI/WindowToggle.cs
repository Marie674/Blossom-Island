using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using PixelCrushers;

public class WindowToggle : MonoBehaviour
{

    public UIPanel Window;
    public KeyCode ToggleKeycode = KeyCode.None;
    public bool CloseOnEsc = true;
    public bool UsesKeyboard;

    public bool PauseWhileOpen = true;
    private float PreviousTimeScale = 1;

    private bool isOpen = false;


    void Start()
    {
        if (Window == null)
        {
            Window = GetComponentInChildren<UIPanel>();
        }
    }


    protected void OnEnable()
    {
        GameInputManager.ObserveKeyCode(ToggleKeycode);
    }

    protected void OnDisable()
    {
    }

    void LateUpdate()
    {
        if (UsesKeyboard == true && Input.GetKeyDown(ToggleKeycode))
        {
            Toggle();
        }
        else if (CloseOnEsc == true && InputDeviceManager.IsButtonDown("Cancel"))
        {
            Close();
        }

    }

    public void Open()
    {

        if (GameManager.Instance.CurrentWindow != null) return;
        if (PixelCrushers.DialogueSystem.DialogueManager.IsConversationActive) return;
        if (GameManager.Instance.Paused == true) return;
        //        print("open " + gameObject.name);

        Window.Open();
        isOpen = true;
        GameManager.Instance.CurrentWindow = this;
        AkSoundEngine.PostEvent("Play_SFX_Window_Open", gameObject);
        if (PauseWhileOpen) GameManager.Instance.PauseGame();
    }

    public void Toggle()
    {
        if (isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }

    }

    public void Close()
    {

        Window.Close();
        GameManager.Instance.CurrentWindow = null;
        if (isOpen && PauseWhileOpen) GameManager.Instance.UnPauseGame();
        isOpen = false;
        AkSoundEngine.PostEvent("Play_SFX_Window_Close", gameObject);
    }

}
