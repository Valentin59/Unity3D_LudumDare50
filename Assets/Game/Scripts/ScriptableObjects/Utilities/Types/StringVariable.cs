using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="Castoor/Types/String")]
public class StringVariable : ScriptableObject
{
    public string value;

    public UnityEvent onChangeValue;

    public void ChangeValue(string value)
    {
        this.value = value;
        onChangeValue?.Invoke();
    }

    public void ChangeValue(StringVariable value)
    {
        this.value = value.value;
        onChangeValue?.Invoke();
    }
}
