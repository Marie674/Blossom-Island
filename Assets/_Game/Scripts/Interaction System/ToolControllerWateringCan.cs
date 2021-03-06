﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolControllerWateringCan : ToolControllerTileBased
{
    protected override IEnumerator UseCountdown()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameManager.Instance.Player.DoAction(CurrentTool.PlayerTrigger, CurrentTool.UseInterval, pos, 0, CurrentTool.ToolTrigger, true);
        List<Vector2> tiles = ToolCursorManager.Instance.GetTiles();

        yield return new WaitForSeconds(CurrentTool.UseInterval);
        ProceedUse(tiles);
    }
    protected override void ProceedUse(List<Vector2> pTiles)
    {
        NeedBase energyNeed = PlayerNeedManager.Instance.GetNeed("Energy");
        energyNeed.Change(-CurrentTool.EnergyCost * (ToolCursorManager.Instance.CursorIndex + 1));

        List<Vector2> tiles = pTiles;

        foreach (Vector2 tile in tiles)
        {
            if (CheckTileValidity(tile))
            {
                FarmPlot plot = GetPlot(tile);
                plot.Water();
                CurrentTool.CurrentCharge = Mathf.Clamp(CurrentTool.CurrentCharge - 1, 0, CurrentTool.MaxCharge);

                if (plot.Crop != null)
                {

                    plot.Crop.Water();
                }
            }
        }

    }

    public override bool CheckUseValidity()
    {
        if (base.CheckUseValidity() == false)
        {
            return false;
        }
        if (CurrentTool.CurrentCharge < 1)
        {
            return false;
        }

        return true;
    }
    public override bool CheckTileValidity(Vector2 pTileWorldPos)
    {
        if (GetPlot(pTileWorldPos) == null)
        {
            return false;
        }
        return true;
    }

    FarmPlot GetPlot(Vector2 pTileWorldPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(pTileWorldPos, Vector2.zero, 0.5f, LayerMask.GetMask("Farm"));
        if (hit.collider == null)
        {
            return null;
        }
        if (hit.collider.GetComponent<FarmPlot>() == null)
        {
            return null;
        }

        FarmPlot farmPlot = hit.collider.gameObject.GetComponent<FarmPlot>();
        return farmPlot;
    }

    public void Refill()
    {
        CurrentTool.CurrentCharge = CurrentTool.MaxCharge;
    }

}
