using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntControllerUI : MonoBehaviour
{
    public IntVariable value;
    public Text ui;

    void Start()
    {
        ui = gameObject.GetComponent<Text>();
        UpdateText();
        value.onChangeValue.AddListener(UpdateText);
    }


    public void UpdateText()
    {
        if(ui != null)
            ui.text = "" + value.value;
    }
}
