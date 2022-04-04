using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class CardAbility
{
    public string title;
    public Sprite icon;
    public string description;
    public UnityAction action;

    public CardAbility(string title, Sprite icon, string description, UnityAction action)
    {
        this.title = title;
        this.icon = icon;
        this.description = description;
        this.action = action;
    }
}


public class LevelUpHandler : MonoBehaviour
{
    public IntVariable level;

    public GameObject cardsCanvasUi;

    public Stats playerStats;

    public List<IncreaseSizeWhenHover> card;

    public List<CardAbility> cards;

    public List<CardAbility> roolPull;

    public GameController gameManager;
    // Start is called before the first frame update
    void Start()
    {
        CardAbility health = new CardAbility("health", null, "Increase your max health", InscreaseHp);
        CardAbility mana = new CardAbility("mana", null, "Increase your max mana", InscreaseMana);
        CardAbility strength = new CardAbility("strength", null, "Increase your damage", InscreaseStrength);
        CardAbility attackSpeed = new CardAbility("attackSpeed", null, "Increase your attack speed by 10 %", InscreaseAttackSpeed);
        CardAbility agility = new CardAbility("Agility", null, "Increase your critick probability", InscreaseAgility);
        CardAbility healthRegeneration = new CardAbility("Health Generation", null, "Increase your health generation", InscreaseHealthRegeneration);
        CardAbility manaRegeneration = new CardAbility("Mana Generation", null, "Increase your mana generation", InscreaseManaRegeneration);

        cards = new List<CardAbility>();
        cards.Add(health);
        cards.Add(mana);
        cards.Add(strength);
        cards.Add(attackSpeed);
        cards.Add(agility);
        cards.Add(healthRegeneration);
        cards.Add(manaRegeneration);

        roolPull = new List<CardAbility>();

        level.onChangeValue.AddListener(ShowCardsAndRollChoice);
        foreach (var item in card)
        {
            item.onClickCallback.AddListener(CloseCard);
            item.onClickCallback.AddListener(gameManager.ResumeGame);
        }
    }



    public void InscreaseHp()
    {
        playerStats.health.Value += 10;
    }
    public void InscreaseMana()
    {
        playerStats.mana.Value += 10;
    }
    public void InscreaseStrength()
    {
        playerStats.strength.Value += 10;
    }

    public void InscreaseAttackSpeed()
    {
        playerStats.attackSpeed.Value += 0.1f;
    }

    public void InscreaseAgility()
    {
        playerStats.agility.Value += 5;
    }

    public void InscreaseHealthRegeneration()
    {
        playerStats.healthRegeneration.Value += 2;
    }

    public void InscreaseManaRegeneration()
    {
        playerStats.manaRegeneration.Value += 2;
    }

    public void InitRoolTab()
    {
        roolPull.Clear();
        foreach(var c in cards)
        {
            roolPull.Add(c);
        }
    }


    public void ShowCardsAndRollChoice()
    {
        InitRoolTab();

        for (int i = 0; i < 3; i++)
        {
            int rngindex = UnityEngine.Random.Range(0, roolPull.Count);
            card[i].SetCard(roolPull[rngindex]);
            roolPull.RemoveAt(rngindex);
        }

        cardsCanvasUi.SetActive(true);
    }

    public void CloseCard()
    {
        cardsCanvasUi.SetActive(false);
    }
    
}
