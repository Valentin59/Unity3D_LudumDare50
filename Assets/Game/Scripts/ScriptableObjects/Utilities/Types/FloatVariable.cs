using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Castoor/Types/Float")]

public class FloatVariable : ScriptableObject
{
    public float value;

    public UnityEvent onChangeValue;

    public void ChangeValue(float value)
    {
        this.value = value;
        onChangeValue?.Invoke();
    }

    public void ChangeValue(FloatVariable value)
    {
        this.value = value.value;
        onChangeValue?.Invoke();
    }

    public void ApplyAmount(float amount)
    {
        this.value -= amount;
    }


    public float CompareTo(FloatVariable other)
    {
        return value.CompareTo(other.value);
    }
}
