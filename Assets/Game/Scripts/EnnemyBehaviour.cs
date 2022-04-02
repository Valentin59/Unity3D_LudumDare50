using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyBehaviour : CharacterBehaviour
{
    public Player player;

    void Start()
    {
        health.onDieCallback.AddListener(AddXp);
    }

    public void AddXp()
    {
        Debug.Log("Add XP ! ");
        if (player != null)
            player.character.GainXp(character.level.Value);
        //health.onDieCallback.RemoveListener(AddXp);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        /*if(player != null)
        {
            player.GetANewPath();
        }*/
    }
}
