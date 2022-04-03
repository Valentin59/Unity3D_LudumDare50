using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextDamageController : MonoBehaviour
{
    //public List<Text>
    public GameObject prefabUI;

    public Camera uiCamera;

    public Canvas uiCanvas;

    public Characters friends;
    public Characters ennemies;


    public Color colorDeal = Color.white;
    public Color colorHit = Color.red;

    public int poolSize = 30;

    public List<TextDamage> textsDamage;


    public void CreateDamageUi()
    {

    }

    public void DamageDealText(int damage, Vector3 position)
    {
        TextDamage t = FindFreeText();

        if(t != null)
        {
            Vector3 screenposition = uiCamera.WorldToScreenPoint(position);
            //Debug.Log("" + damage + "| position " + screenposition);

            t.gameObject.SetActive(true);
            t.Init("" + damage, new Vector2(screenposition.x, screenposition.y), Color.white);
        }
    }

    public void DamageTakenText(int damage, Vector3 position)
    {
        //Debug.Log("" + damage + "| position " + position);

        TextDamage t = FindFreeText();
        if (t != null)
        {
            Vector3 screenposition = uiCamera.WorldToScreenPoint(position);
            t.gameObject.SetActive(true);
            t.Init("" + damage, new Vector2(screenposition.x, screenposition.y), Color.red);
        }
    }



    public TextDamage FindFreeText()
    {
        foreach(var t in textsDamage)
        {
            if(!t.gameObject.activeSelf)
            {
                return t;
            }
        }
        return null;
    }


    // Start is called before the first frame update
    void Start()
    {
        textsDamage = new List<TextDamage>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject tdgo = Instantiate<GameObject>(prefabUI);
            tdgo.transform.SetParent(uiCanvas.transform);
            textsDamage.Add(tdgo.GetComponent<TextDamage>());
            textsDamage[i].Disable();
        }
        foreach(var c in friends.items)
        {
            if(c.health != null)
                c.health.onDamageTaken.AddListener(DamageTakenText);
        }

        foreach (var c in ennemies.items)
        {
            if (c.health != null)
                c.health.onDamageTaken.AddListener(DamageDealText);
        }
    }

    private void OnDestroy()
    {
        foreach (var c in friends.items)
        {
            if (c.health != null)
                c.health.onDamageTaken.RemoveListener(DamageTakenText);
        }

        foreach (var c in ennemies.items)
        {
            if (c.health != null)
                c.health.onDamageTaken.RemoveListener(DamageDealText);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
