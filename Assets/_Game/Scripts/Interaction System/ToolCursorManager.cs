using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreativeSpore.SuperTilemapEditor;
using ItemSystem;
public class ToolCursorManager : Singleton<ToolCursorManager>
{

    public ToolCursorBase CurrentCursor;
    ItemTool CurrentTool;

    public Vector2 DownPosition;
    public Vector2 LeftPosition;
    public Vector2 RightPosition;
    public Vector2 UpPosition;

    public int CursorIndex = 0;

    public GameObject TileUnderMouse;

    bool ShowSingleTile = false;

    public float MaxMouseDistance = 1.75f;

    void OnEnable()
    {
        SetPosition();
        ToolManager.Instance.OnSelectedToolChanged += ToolChange;
        GameManager.Instance.Player.OnPlayerDirectionChange += SetPosition;

    }

    void Disable()
    {
        ToolManager.Instance.OnSelectedToolChanged += ToolChange;
        GameManager.Instance.Player.OnPlayerDirectionChange -= SetPosition;
    }

    void ToolChange()
    {
        CurrentTool = ToolManager.Instance.CurrentTool;
        CursorIndex = 0;
        SetCursor();
    }

    void Update()
    {
        if (CurrentTool == null)
        {
            return;
        }
        if (Input.GetButtonDown("Cycle Cursor"))
        {
            CycleCursor();
        }
        if (ShowSingleTile == true && CurrentTool.ToolController.ShowCursor)
        {
            TileUnderMouse.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (GetDistance() <= MaxMouseDistance)
            {
                TileUnderMouse.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else
            {
                TileUnderMouse.GetComponent<SpriteRenderer>().color = Color.gray;
            }
        }
        else
        {
            TileUnderMouse.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

        }

    }

    void SetPosition()
    {

        switch (GameManager.Instance.Player.Direction)
        {
            case PlayerCharacter.CharacterDirection.Down:
                this.transform.localPosition = DownPosition;
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case PlayerCharacter.CharacterDirection.Left:
                this.transform.localPosition = LeftPosition;
                this.transform.rotation = Quaternion.Euler(0, 0, -90);
                break;
            case PlayerCharacter.CharacterDirection.Right:
                this.transform.localPosition = RightPosition;
                this.transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case PlayerCharacter.CharacterDirection.Up:
                this.transform.localPosition = UpPosition;
                this.transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            default:
                this.transform.localPosition = DownPosition;
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
        }
    }

    public List<Vector2> GetTiles()
    {
        // if (CurrentCursor.GetComponent<ToolCursorTileBased>() == null)
        // {
        //     return null;
        // }
        ToolCursorTileBased cursor = CurrentCursor.GetComponent<ToolCursorTileBased>();
        List<Vector2> tiles = new List<Vector2>();
        List<CursorTile> cursorTiles = new List<CursorTile>();



        foreach (Transform tileRow in cursor.TileRows)
        {
            CursorTile[] rowTiles = tileRow.GetComponentsInChildren<CursorTile>();
            foreach (CursorTile tile in rowTiles)
            {
                cursorTiles.Add(tile);
            }
        }
        foreach (CursorTile tile in cursorTiles)
        {
            tiles.Add(tile.transform.position);
        }
        if (tiles.Count == 1)
        {
            float mouseDistance = 0;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseDistance = Vector2.Distance(GameManager.Instance.Player.transform.position, mousePos);
            if (mouseDistance <= MaxMouseDistance)
            {
                tiles = new List<Vector2>();
                TileUnderMouse.transform.position = mousePos;
                TileUnderMouse.GetComponent<ObjectPosition>().AdjustPositions();
                tiles.Add(TileUnderMouse.transform.position);
            }
        }
        return tiles;
    }

    public List<GameObject> GetObjects(List<string> pAllowedTags)
    {
        List<Vector2> tiles = GetTiles();
        List<GameObject> affectedObjects = new List<GameObject>();
        foreach (Vector2 tile in tiles)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(tile, Vector2.zero);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null)
                {
                    print(hit.transform.name);
                    if (hit.collider.isTrigger == false)
                    {
                        continue;
                    }

                    GameObject obj = hit.collider.gameObject;
                    if (affectedObjects.Contains(obj.gameObject))
                    {
                        continue;
                    }
                    if (pAllowedTags.Contains(obj.tag))
                    {
                        affectedObjects.Add(obj);

                    }

                }
            }

        }
        return affectedObjects;
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
            SetPosition();
            if (CursorIndex > 0)
            {
                ShowCursor();
                TileUnderMouse.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                ShowSingleTile = false;

            }
            else
            {
                HideCursor();
                ShowSingleTile = true;
                if (CurrentTool.ToolController.ShowCursor)
                {
                    TileUnderMouse.GetComponent<SpriteRenderer>().color = Color.white;

                }
                else
                {
                    TileUnderMouse.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

                }
            }
        }
    }

    void ShowCursor()
    {
        CursorTile[] tiles = CurrentCursor.GetComponentsInChildren<CursorTile>();
        foreach (CursorTile tile in tiles)
        {
            print("show tile" + tile.gameObject.name);

            SpriteRenderer sprite = tile.transform.GetComponent<SpriteRenderer>();
            sprite.color = new Color(1, 1, 1, 1);
        }

    }

    void HideCursor()
    {
        CursorTile[] tiles = CurrentCursor.GetComponentsInChildren<CursorTile>();
        foreach (CursorTile tile in tiles)
        {
            print("hide tile" + tile.gameObject.name);
            SpriteRenderer sprite = tile.transform.GetComponent<SpriteRenderer>();
            sprite.color = new Color(1, 1, 1, 0);
        }

    }

}
