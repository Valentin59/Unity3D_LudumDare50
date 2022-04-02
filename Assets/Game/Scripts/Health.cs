using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public CharacterSO character;

    public int currentHp;

    public UnityEvent onDieCallback;

    void Start()
    {
        currentHp = character.MaxHealth();
        if(currentHp == 0)
        {
            currentHp = 1;
        }
    }

    public void Damage(int damagePoints)
    {
        int damageDeal = (Mathf.Clamp(damagePoints - character.Armor(), 1, damagePoints));
        currentHp -= damageDeal;

        Debug.Log(gameObject.name + " a subit " + damageDeal);

        if(currentHp <= 0)
        {
            currentHp = 0;

            onDieCallback?.Invoke();

            //dead
            if (this.gameObject.CompareTag("Player"))
            {
                // On a perdu
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
    public void Heal(int healPoints)
    {
        currentHp += healPoints;
        currentHp = Mathf.Clamp(currentHp, 0, character.MaxHealth());
        /*if (currentHp <= 0)
        {
            //dead
        }*/
    }
}
