using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class IncreaseSizeWhenHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public float sizeMax = 1.3f;
    public RectTransform rect;
    public Text uiText;
    public Image uiImage;

    public UnityEvent onClickCallback;

    // Start is called before the first frame update
    void Start()
    {
        _timer = 0f;
        rect = gameObject.GetComponent<RectTransform>();
        uiText = gameObject.GetComponentInChildren<Text>();
    }

    public void SetCard(CardAbility card)
    {
        if(card.icon == null)
        {
            uiImage.gameObject.SetActive(false);
        }
        else
        {
            uiImage.sprite = card.icon;
        }

        uiText.text = card.description;
        onClickCallback.AddListener(card.action);
    }


    public void SetText(string text)
    {
        if(text != null)
        {
            this.uiText.text = text;
        }
    }

    private float _timer;
    public bool animate;
    public bool play;
    // Update is called once per frame
    void Update()
    {
        if (!play)
            return;

        if(animate)
            _timer += Time.deltaTime * 3f;
        else
            _timer -= Time.deltaTime * 3f;
        _timer = Mathf.Clamp01(_timer);
        rect.localScale = Vector3.one * Mathf.Lerp(1f, sizeMax, _timer);

        if(Mathf.Clamp01(_timer) == 0f)
        {
            play = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animate = true;
        play = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animate = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClickCallback?.Invoke();
    }
}
