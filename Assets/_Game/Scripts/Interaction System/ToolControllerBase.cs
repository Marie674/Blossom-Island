using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Items;
using CreativeSpore.SuperTilemapEditor;

[System.Serializable]
public struct AffectedLayer
{
    public string Name;
    public ItemOutput Output;
    public ItemBase Input;
}
public class ToolControllerBase : MonoBehaviour
{
    public bool ShowCursor = true;
    public bool AffectsObjects = false;
    public bool AffectsTilemap = true;

    //For objects
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
        StartCoroutine("UseCountdown", CurrentTool.UseInterval);
    }

    protected virtual IEnumerator UseCountdown()
    {
        Vector2 pos = ToolCursorManager.Instance.CurrentCursor.transform.position;

        GameManager.Instance.Player.DoAction(CurrentTool.PlayerTrigger, CurrentTool.UseInterval, pos, 0, CurrentTool.ToolTrigger, true);
        yield return new WaitForSeconds(CurrentTool.UseInterval);
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

        // if (CheckCursorDistance() == false)
        // {
        //     return false;
        // }
        if (CheckEnergy() == false)
        {
            return false;
        }

        // if (ToolCursorManager.Instance.GetComponent<ToolCursorPixelbased>() != null)
        // {
        //     if (ToolCursorManager.Instance.GetComponent<ToolCursorPixelbased>().CanUse == false)
        //     {
        //         return false;
        //     }
        // }

        if (GameManager.Instance.Player.IsActing)
        {
            return false;
        }
        return true;
    }

    // protected bool CheckCursorDistance()
    // {
    //     float distance = ToolCursorManager.Instance.GetDistance();
    //     if (distance > ToolCursorManager.Instance.CurrentCursor.MaxDistance)
    //     {
    //         return false;
    //     }
    //     return true;
    // }
    protected bool CheckEnergy()
    {

        if (CurrentTool == null)
        {
            return false;
        }

        float energyNeeded = CurrentTool.EnergyCost * (ToolCursorManager.Instance.CursorIndex + 1);
        NeedBase energyNeed = PlayerNeedManager.Instance.GetNeed("Energy");
        if (energyNeed.CurrentValue < energyNeeded)
        {
            return false;
        }
        return true;
    }

}
