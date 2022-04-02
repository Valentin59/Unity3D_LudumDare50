using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Castoor/Types/Bool")]
public class BoolVariable : ScriptableObject
{
    public bool value;

    public UnityEvent onChangeValue;

    public void ChangeValue(bool value)
    {
        this.value = value;
        onChangeValue?.Invoke();
    }

    public void ChangeValue(BoolVariable value)
    {
        this.value = value.value;
        onChangeValue?.Invoke();
    }
}
