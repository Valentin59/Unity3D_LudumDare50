using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Castoor/Types/Int")]
public class IntVariable : ScriptableObject
{
    public int value;

    public UnityEvent onChangeValue;

    public void ChangeValue(int value)
    {
        this.value = value;
        onChangeValue?.Invoke();
    }

    public void ChangeValue(IntVariable value)
    {
        this.value = value.value;
        onChangeValue?.Invoke();
    }

    public void ApplyChange(IntVariable amount)
    {
        value += amount.value;
        if (onChangeValue != null)
            onChangeValue.Invoke();
    }

    public void ApplyChange(int amount)
    {
        value += amount;
        if (onChangeValue != null)
            onChangeValue.Invoke();
    }

    public int CompareTo(IntVariable other)
    {
        return value.CompareTo(other.value);
    }
}
