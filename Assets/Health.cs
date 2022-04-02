using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public CharacterSO character;

    public int currentHp;

    void Start()
    {
        
    }

    public void Damage(int damagePoints)
    {

        currentHp -= ( Mathf.Clamp(damagePoints - character.Armor(), 1, damagePoints));

        if(currentHp <= 0)
        {
            //dead
        }
    }
    public void Heal(int healPoints)
    {
        currentHp += healPoints;

        if (currentHp <= 0)
        {
            //dead
        }
    }
}
