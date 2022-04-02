using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Castoor/Stuff/Armor")]
public class Armor : ScriptableObject
{
    public Sprite icon;
    public GameObject prefab;

    public ARMORTYPE type;
    public QUALITY quality;

    public int points;

    public StatsReference stats;
    //public Stats stats;
}

public enum QUALITY
{
    NORMAL,
    MAGIC,
    EPIC,
    LEGENDARY
}

public enum ARMORTYPE
{
    HEAD,
    BODY,
    SHOULDER,
    HAND
}