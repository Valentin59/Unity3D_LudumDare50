using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XpControllerUI : MonoBehaviour
{
    public CharacterSO player;
    public IntVariable xp;
    public IntVariable level;
    public Text ui;

    void Start()
    {
        ui = gameObject.GetComponent<Text>();
        UpdateText();
        xp.onChangeValue.AddListener(UpdateText);
    }


    public void UpdateText()
    {
        if(ui != null)
            ui.text = "" + xp.value + " / " + player.NextLevelXP(level.value);
    }
}
