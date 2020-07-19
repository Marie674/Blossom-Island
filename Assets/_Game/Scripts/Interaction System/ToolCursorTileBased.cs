using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolCursorTileBased : ToolCursorBase
{

    public int Width;
    public int Height;

    public float Units = 1f;

    public List<CursorTile> Tiles = new List<CursorTile>();

    protected override void Position()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 pos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 SpriteTopLeft;

        pos.x = Units * Mathf.Round(pos.x / Units);
        pos.y = Units * Mathf.Round(pos.y / Units);
        pos.z = -9f;

        Sprite.transform.position = pos;
        SpriteTopLeft = new Vector3(Sprite.bounds.min.x, Sprite.bounds.max.y, 0);

        Vector3 spritePivot = Sprite.transform.position;
        Vector3 spriteOffset = spritePivot - SpriteTopLeft;
        spriteOffset.z = 0;

        transform.position = pos - spriteOffset;
    }
}
