using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatsReference
{
    public bool UseConstant = true;
    
    public Stats ConstantValue;

    public Stats Stats()
    {
        if (ConstantValue == null)
            ConstantValue = new Stats();
        return ConstantValue;
    }

    //public StatsReference Variable;
}
