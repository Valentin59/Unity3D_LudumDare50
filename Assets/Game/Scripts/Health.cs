using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public CharacterSO character;

    public IntReference currentHp;

    public bool isPlayer;

    //public int currentHp;

    public UnityEvent onDieCallback;
    public UnityEvent<int, Vector3> onDamageTaken;

    public GameObject deadSound;

    void Awake()
    {
        /*if(!isPlayer)
        {*/
            if(currentHp.UseConstant)
                currentHp.ConstantValue = character.MaxHealth();
            else
                currentHp.Variable.value = character.MaxHealth();
        /*}
        else
        {
            //currentHp.Value = character.MaxHealth();
        }*/


        /*if(currentHp == 0)
        {
            currentHp = 1;
        }*/
    }

    public void Damage(int damagePoints)
    {
        int damageDeal = (Mathf.Clamp(damagePoints - character.Armor(), 1, damagePoints));
        
        if (currentHp.UseConstant)
            currentHp.ConstantValue -= damageDeal;
        else
            currentHp.Variable.ApplyChange(- damageDeal);
        
        onDamageTaken?.Invoke(damageDeal, this.transform.position);

        //Debug.Log(gameObject.name + " a subit " + damageDeal);

        if(currentHp <= 0)
        {
            if (currentHp.UseConstant) // Ennemy
                currentHp.ConstantValue = 0;
            else// Player
                currentHp.Variable.ChangeValue(0);


            onDieCallback?.Invoke();

            if(deadSound != null)
            {
                Instantiate<GameObject>(deadSound, transform.position, Quaternion.identity);
            }

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
        if (currentHp.UseConstant)
        {
            currentHp.ConstantValue += healPoints;
            currentHp.ConstantValue = Mathf.Clamp(currentHp.ConstantValue, 0, character.MaxHealth());
        }
        else
        {
            currentHp.Variable.ApplyChange( healPoints );
            currentHp.Variable.ChangeValue(Mathf.Clamp(currentHp.Variable.value, 0, character.MaxHealth()));

            //currentHp.Variable.ApplyChange(-damageDeal);
        }

        /*if (currentHp <= 0)
        {
            //dead
        }*/
    }
}
