using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Castoor/Character/Stats")]
public class Stats : ScriptableObject
{
    public int health;
    public int mana;
    public int agility;
    public int strength;
    public int healthRegeneration; // health heal per 5/10 sec ?
    public int manaRegeneration;
    public int attackSpeed;
}
