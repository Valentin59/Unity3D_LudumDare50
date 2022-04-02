using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Castoor/Map")]
public class Map : ScriptableObject
{
    public bool[,] map;

    public UnityEvent onInitializeComplete;
    public UnityEvent onGenerationComplete;

    public void Initialize(Vector2Int size)
    {
        map = new bool[size.x, size.y];
        onInitializeComplete?.Invoke();
    }

    public void MapGenerated()
    {
        onGenerationComplete?.Invoke();
    }

}
