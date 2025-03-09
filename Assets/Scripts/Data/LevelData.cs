using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class LevelData
{
    public int level;
    public bool[] stars;
    public Dictionary<string, int> targets;
}

[Serializable]
public class LevelList
{
    public List<LevelData> levels;
}