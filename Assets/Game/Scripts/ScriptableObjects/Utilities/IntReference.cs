using System;


[Serializable]
public class IntReference : IComparable<IntReference>
{
    public bool UseConstant = true;
    public int ConstantValue;
    public IntVariable Variable;

    public IntReference()
    { }

    public IntReference(int value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public int Value
    {
        get { return UseConstant ? ConstantValue : Variable.value; }
    }

    public int CompareTo(IntReference other)
    {
        return Variable.CompareTo(other.Variable);
    }

    public static implicit operator int(IntReference reference)
    {
        return reference.Value;
    }
}
