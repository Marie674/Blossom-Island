using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CursorTile : MonoBehaviour
{
    SpriteRenderer Sprite;
    ToolControllerTileBased CurrentToolController;

    void Start()
    {
        Sprite = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        ToolManager.Instance.OnSelectedToolChanged += ToolChange;
        ToolChange();
    }

    void Disable()
    {
        ToolManager.Instance.OnSelectedToolChanged += ToolChange;
    }

    void ToolChange()
    {
        if (ToolManager.Instance.CurrentTool == null)
        {
            CurrentToolController = null;
            return;
        }
        CurrentToolController = ToolManager.Instance.CurrentToolController as ToolControllerTileBased;
    }
    void LateUpdate()
    {
        CheckValidity();
    }
    void CheckValidity()
    {
        if (CurrentToolController == null)
        {
            return;
        }
        bool isValid = true;
        if (CurrentToolController.CheckTileValidity(transform.position) == false)
        {
            isValid = false;
        }
        if (CurrentToolController.CheckUseValidity() == false)
        {
            isValid = false;
        }

        // if(isValid){
        //     Sprite.color = Color.cyan;
        // }
        // else{
        //     Sprite.color = Color.red;
        // }
    }

}
