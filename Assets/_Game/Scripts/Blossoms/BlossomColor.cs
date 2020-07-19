﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Blossoms/BlossomColor")]

public class BlossomColor : ScriptableObject
{
    public string Name;
    public int Probability;
    public Sprite AdultPortrait;
    public Sprite BabyPortrait;
}
