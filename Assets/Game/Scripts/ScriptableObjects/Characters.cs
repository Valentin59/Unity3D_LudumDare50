using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Castoor/Array/Characters")]
public class Characters : TArray<CharacterBehaviour>
{
    public CharacterBehaviour GetNearest(Vector3 point)
    {
        if (items.Count <= 0)
            return null;
        else if (items.Count == 1)
            return items[0];
        else
        {
            int closest = 0;
            Vector3 tmp = items[0].transform.position;

            for (int i = 1; i < items.Count; i++)
            {

                if ((tmp - point).sqrMagnitude > (items[i].transform.position - point).sqrMagnitude)
                {
                    closest = i;
                    tmp = items[i].transform.position;
                }
            }
            return items[closest];
        }
    }


}
