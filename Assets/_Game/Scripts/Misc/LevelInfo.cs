using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreativeSpore.SuperTilemapEditor;
public class LevelInfo : MonoBehaviour
{
    public TilemapGroup CurrentMap;

    public string Name;
    public enum LevelType
    {
        Outside,
        Inside
    }

    public LevelType Type = LevelType.Outside;
}
