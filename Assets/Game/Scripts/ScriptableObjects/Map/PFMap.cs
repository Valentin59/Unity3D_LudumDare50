using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Castoor/PathFinding/Map")]
public class PFMap : ScriptableObject
{
    public int[,] map;
}
