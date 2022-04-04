using System;


[Serializable]
public class FloatReference : IComparable<FloatVariable>
{
    public bool UseConstant = true;
    public float ConstantValue;
    public FloatVariable Variable;

    public FloatReference()
    { }

    public FloatReference(float value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public float Value
    {
        get { return UseConstant ? ConstantValue : Variable.value; }
        set { if (UseConstant) ConstantValue = value; else Variable.value = value; }
    }

    public int CompareTo(FloatVariable other)
    {
        return Value.CompareTo(other.value);
    }

    public static implicit operator float(FloatReference reference)
    {
        return reference.Value;
    }
}
