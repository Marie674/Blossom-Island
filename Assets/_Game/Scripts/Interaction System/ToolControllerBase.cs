﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;
using CreativeSpore.SuperTilemapEditor;

[System.Serializable]
public struct AffectedLayer
{
    public string Name;
    public ItemOutput Output;
    public ItemContainer Input;
}
public class ToolControllerBase : MonoBehaviour
{
    public List<string> AllowedTags = new List<string>();

    public ItemTool CurrentTool;
    protected virtual void OnEnable()
    {
        ToolManager.Instance.OnSelectedToolChanged += SetTool;
        SetTool();
    }

    protected virtual void OnDisable()
    {
        ToolManager.Instance.OnSelectedToolChanged -= SetTool;
    }

    void SetTool()
    {
        CurrentTool = ToolManager.Instance.CurrentTool;
    }
    public virtual bool CheckTileValidity(Vector2 pTilePosition)
    {
        return true;
    }
    public virtual void Use()
    {
        if (CheckUseValidity() == false)
        {
            return;
        }
        StartCoroutine("UseCountdown", CurrentTool.useInterval);
    }

    protected virtual IEnumerator UseCountdown()
    {
        Vector2 pos = ToolCursorManager.Instance.CurrentCursor.transform.position;
        GameManager.Instance.Player.DoAction(CurrentTool.trigger, CurrentTool.useInterval, pos, 0, CurrentTool.toolTrigger, true);
        yield return new WaitForSeconds(CurrentTool.useInterval);
        ProceedUse();
    }
    protected virtual void ProceedUse()
    {

    }
    protected virtual void ProceedUse(List<Vector2> pTiles)
    {

    }

    protected virtual void ProceedUse(List<GameObject> pObjects)
    {

    }
    public virtual bool CheckUseValidity()
    {

        if (CheckCursorDistance() == false)
        {
            return false;
        }
        if (CheckEnergy() == false)
        {
            return false;
        }

        if (ToolCursorManager.Instance.GetComponent<ToolCursorPixelbased>() != null)
        {
            if (ToolCursorManager.Instance.GetComponent<ToolCursorPixelbased>().CanUse == false)
            {
                return false;
            }
        }

        if (GameManager.Instance.Player.IsActing)
        {
            return false;
        }
        return true;
    }

    protected bool CheckCursorDistance()
    {
        float distance = ToolCursorManager.Instance.GetDistance();
        if (distance > ToolCursorManager.Instance.CurrentCursor.MaxDistance)
        {
            return false;
        }
        return true;
    }
    protected bool CheckEnergy()
    {

        if (CurrentTool == null)
        {
            return false;
        }

        float energyNeeded = CurrentTool.energyCost * (ToolCursorManager.Instance.CursorIndex + 1);
        NeedBase energyNeed = PlayerNeedManager.Instance.GetNeed("Energy");
        if (energyNeed.CurrentValue < energyNeeded)
        {
            return false;
        }
        return true;
    }

}
