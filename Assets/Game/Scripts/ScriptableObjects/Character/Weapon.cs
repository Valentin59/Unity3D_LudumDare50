using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Castoor/Stuff/Weapon")]
public class Weapon : ScriptableObject
{
    public string title;
    [Multiline]
    public string description;

    public Sprite icon;
    public GameObject prefab;

    public WEAPONTYPE type;
    public QUALITY quality;

    public float attackRange;
    public float attackSpeed;
    public Vector2Int damage;
}

public enum WEAPONTYPE
{
    TWOHAND,
    ONEHAND,
    LEFTHAND,
    RIGHTHAND
}

