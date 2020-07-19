using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreativeSpore.SuperTilemapEditor;
using ItemSystem;
public class ToolCursorManager : Singleton<ToolCursorManager>
{

    public ToolCursorBase CurrentCursor;
    ItemTool CurrentTool;

    public int CursorIndex = 0;

    void OnEnable()
    {
        ToolManager.Instance.OnSelectedToolChanged += ToolChange;
    }

    void Disable()
    {
        ToolManager.Instance.OnSelectedToolChanged += ToolChange;
    }

    void ToolChange()
    {
        CurrentTool = ToolManager.Instance.CurrentTool;
        CursorIndex = 0;
        SetCursor();
    }

    void Update()
    {
        if (Input.GetButtonDown("Cycle Cursor"))
        {
            CycleCursor();
        }
    }

    public List<Vector2> GetTiles()
    {
        if (CurrentCursor.GetComponent<ToolCursorTileBased>() == null)
        {
            return null;
        }
        ToolCursorTileBased cursor = CurrentCursor.GetComponent<ToolCursorTileBased>();
        List<Vector2> tiles = new List<Vector2>();
        List<CursorTile> cursorTiles = cursor.Tiles;
        foreach (CursorTile tile in cursorTiles)
        {
            tiles.Add(tile.transform.position);
        }
        return tiles;
    }

    public List<GameObject> GetObjects()
    {
        if (CurrentCursor.GetComponent<ToolCursorPixelbased>() == null)
        {
            return null;
        }
        ToolCursorPixelbased cursor = CurrentCursor.GetComponent<ToolCursorPixelbased>();
        return cursor.AffectedObjects;
    }

    public float GetDistance()
    {
        //Camera.main.ScreenToWorldPoint
        float distance = Vector2.Distance((CurrentCursor.transform.position), GameManager.Instance.Player.transform.position);
        return distance;

    }

    void CycleCursor()
    {
        if (CurrentTool == null)
        {
            return;
        }
        CursorIndex = ((CursorIndex + 1) % (CurrentTool.Cursors.Length));
        SetCursor();
    }

    void SetCursor()
    {
        if (CurrentCursor != null)
        {
            Destroy(CurrentCursor.gameObject);
        }
        if (CurrentTool != null)
        {
            CurrentCursor = GameObject.Instantiate(CurrentTool.Cursors[CursorIndex], this.transform);
        }
    }

}
