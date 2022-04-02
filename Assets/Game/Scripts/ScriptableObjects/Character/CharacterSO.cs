using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Castoor/CharacterSO")]
public class CharacterSO : ScriptableObject
{
    public int level;

    public Stats stats;

    public Armor head;
    public Armor body;
    public Armor hand;
    public Armor shoulder;

    public Weapon weapon;

    public int MaxHealth()
    {
        int a = stats.health;
        if (head != null)
            if(head.stats != null)
                a += head.stats.Stats().health;

        if (body != null)
            if (body.stats != null)
                a += body.stats.Stats().health;

        if (hand != null)
            if (hand.stats != null)
                a += hand.stats.Stats().health;

        if (shoulder != null)
            if (shoulder.stats != null)
                a += shoulder.stats.Stats().health;

        return a;
    }

    public int Armor()
    {
        int a = 0;
        if (head != null)
            a += head.points;
        if (body != null)
            a += body.points;
        if (hand != null)
            a += hand.points;
        if (shoulder != null)
            a += shoulder.points;

        return a;
    }

    public int Damage()
    {
        var d = 0;
        
        d = stats.strength;
        if(weapon != null)
            d += Random.Range(weapon.damage.x, weapon.damage.y);
        if(RollCrit())
        {
            d += d / 2;
        }
        return d;
    }

    public bool RollCrit()
    {
        bool crit = Random.Range(0, 100) < Mathf.Clamp(stats.agility, 0, 60) ? true : false;
        return crit;
    }

}
