using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TextDamage : MonoBehaviour
{
    public RectTransform rect;
    public Text ui;

    public int sizeCrit = 50;
    public int sizeNormal = 38;

    public float duration = 1f;

    public UnityEvent<TextDamage> onCompleteAnimation;

    public void Init(string text, Vector2 position, Color color, bool isCrit = false)
    {
        this.gameObject.SetActive(true);
        if(isCrit)
        {
            ui.fontSize = sizeCrit;
        }
        else
        {
            ui.fontSize = sizeNormal;
        }
        ui.color = color;
        ui.text = "";
        Debug.Log(text);
        ui.text = text;
        rect.anchoredPosition = position;
    }

    public void Disable()
    {
        this.gameObject.SetActive(false);
        onCompleteAnimation?.Invoke(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        _timer = 0f;
        rect = this.GetComponent<RectTransform>();
        ui = this.GetComponentInChildren<Text>();
    }

    private float _timer;

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        rect.anchoredPosition += Vector2.up * Time.deltaTime * 100f;
        if(_timer >= duration)
        {
            Disable();
            _timer = 0f;
        }
    }
}
