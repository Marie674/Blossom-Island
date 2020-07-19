using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreativeSpore.SuperTilemapEditor;

public class TerrainSeasonChange : MonoBehaviour
{
    public List<STETilemap> Exclusions = new List<STETilemap>();

    void OnEnable()
    {
        TimeManager.OnMonthChanged += ChangeGraphics;
        ChangeGraphics(TimeManager.Instance.CurrentMonth);
    }

    void OnDisable()
    {
        TimeManager.OnMonthChanged -= ChangeGraphics;
    }

    void ChangeGraphics(Month pCurrentMonth)
    {
        List<STETilemap> layers = GetComponent<TilemapGroup>().Tilemaps;
        foreach (STETilemap layer in layers)
        {
            if (!Exclusions.Contains(layer))
            {
                if (TimeManager.Instance.CurrentMonth.TerrainTexture != null)
                {
                    layer.Tileset.AtlasTexture = TimeManager.Instance.CurrentMonth.TerrainTexture;
                }
            }
        }
    }
}
