using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;
using PixelCrushers.DialogueSystem;
using CreativeSpore.SuperTilemapEditor;

public class ToolManager : Singleton<ToolManager>
{

    public ItemTool CurrentTool = null;
    public float TimeBeforeHold = 0.15f;
    public float HoldDelayTimer = 0f;
    public bool HoldOnDelay = false;
    public string UseButton = "Use Tool";

    public ToolControllerBase CurrentToolController;
    void OnEnable()
    {
        Toolbar.Instance.OnSelectedSlotItemChanged += ChangeTool;
    }


    void OnDisable()
    {
        if (Toolbar.Instance != null)
            Toolbar.Instance.OnSelectedSlotItemChanged -= ChangeTool;
    }

    void Update()
    {

        if (CurrentToolController == null)
        {
            return;
        }
        if (HoldOnDelay)
        {
            HoldDelayTimer += Time.deltaTime;
            if (HoldDelayTimer >= TimeBeforeHold)
            {
                HoldDelayTimer = 0f;
                HoldOnDelay = false;
            }
        }

        if (IsUseButton())
        {

            if (HoldOnDelay)
            {
                return;
            }

            CurrentToolController.Use();
            HoldOnDelay = true;

        }

        if (IsUseButtonUp())
        {
            HoldDelayTimer = HoldDelayTimer;
        }

    }

    private bool IsUseButton()
    {
        if (DialogueManager.IsDialogueSystemInputDisabled()) return false;
        return (!string.IsNullOrEmpty(UseButton) && Input.GetButton("Use Tool"));
    }

    private bool IsUseButtonUp()
    {
        if (DialogueManager.IsDialogueSystemInputDisabled()) return false;
        return (!string.IsNullOrEmpty(UseButton) && Input.GetButtonUp("Use Tool"));
    }


    public delegate void SelectedToolChange();
    public event SelectedToolChange OnSelectedToolChanged;
    void ChangeTool()
    {
        if (CurrentToolController != null)
        {
            Destroy(CurrentToolController.gameObject);
            CurrentToolController = null;
        }
        if (Toolbar.Instance.SelectedSlot.ReferencedItemStack != null)
        {
            ItemBase selectedItem = Toolbar.Instance.SelectedSlot.ReferencedItemStack.ContainedItem;
            if (selectedItem as ItemTool == null)
            {
                CurrentTool = null;
            }
            else
            {
                CurrentTool = ItemSystemUtility.GetItemCopy(selectedItem.itemID, ItemType.Tool) as ItemTool;
                TimeBeforeHold = CurrentTool.useInterval / 4;
            }
        }
        else
        {
            CurrentTool = null;
        }

        if (CurrentTool != null)
        {
            CurrentToolController = Instantiate(CurrentTool.ToolController.gameObject, this.transform).GetComponent<ToolControllerBase>();
        }

        if (OnSelectedToolChanged != null)
        {
            OnSelectedToolChanged();
        }
    }

}