using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpHandler : MonoBehaviour
{
    public IntVariable level;

    public GameObject cardsCanvasUi;

    public Stats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ShowCardsAndRollChoice()
    {
        cardsCanvasUi.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
