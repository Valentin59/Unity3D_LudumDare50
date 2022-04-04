using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Castoor/CharacterSO")]
public class CharacterSO : ScriptableObject
{
    public IntReference level;
    public IntReference currentXp;

    public Stats stats;

    public Armor head;
    public Armor body;
    public Armor hand;
    public Armor shoulder;

    public Weapon weapon;

    public UnityEvent onLevelUpCallback;


    public float AttackSpeed()
    {
        float attackSpeed = 1f;

        if(weapon != null)
        {
            attackSpeed = weapon.attackSpeed - stats.attackSpeed * weapon.attackSpeed;
        }

        return attackSpeed;
    }

    public int MaxHealth()
    {
        int a = stats.health.Value;
        if (head != null)
            if(head.stats != null)
                if(head.stats.Stats().health != null)
                    a += head.stats.Stats().health.Value;

        if (body != null)
            if (body.stats != null)
                if (head.stats.Stats().health != null)
                    a += body.stats.Stats().health.Value;

        if (hand != null)
            if (hand.stats != null)
                if (head.stats.Stats().health != null)
                    a += hand.stats.Stats().health.Value;

        if (shoulder != null)
            if (shoulder.stats != null)
                if (head.stats.Stats().health != null)
                    a += shoulder.stats.Stats().health.Value;

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
            d += Random.Range(weapon.damage.x, weapon.damage.y+1);
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

    public void CheckIfLevelUp()
    {
        if (level == 9)
            return;

        if(currentXp >= NextLevelXP(level))
        {
            int tmpXp = currentXp - NextLevelXP(level);
            level.Variable.ChangeValue(level + 1);
            currentXp.Variable.ChangeValue(tmpXp);
            
            onLevelUpCallback?.Invoke();
        }
    }

    public void GainXp(int level)
    {
        currentXp.Variable.ApplyChange(GetXP(level));
        //Debug.Log("GetXP(level) " + GetXP(level));
        CheckIfLevelUp();
    }

    public int GetXP(int level )
    {
        int[] xpTab =
        {
            1,
            2,
            5,
            11,
            31,
            78,
            150,
            350,
            500,
            700,
            900,
            1000
        };

        return xpTab[Mathf.Clamp(level, 0, xpTab.Length-1)];
    }

    public int NextLevelXP(int level)
    {
        int[] levelsMaxXp =
        {
            1,
            2,
            4,
            6,
            8,
            16,
            30,
            60,
            80,
            120,
            160,
            250
        };
        return levelsMaxXp[Mathf.Clamp(level, 0, levelsMaxXp.Length - 1)];
    }

}
