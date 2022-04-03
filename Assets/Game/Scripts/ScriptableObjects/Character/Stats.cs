using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Castoor/Character/Stats")]
public class Stats : ScriptableObject
{
    public IntReference health;
    public IntReference mana;
    public IntReference agility;
    public IntReference strength;
    public IntReference healthRegeneration; // health heal per 5/10 sec ?
    public IntReference manaRegeneration;
    public IntReference attackSpeed;
}
