using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TArray<T> : ScriptableObject
{
    public List<T> items = new List<T>();

    public int Count => items.Count;

    public UnityEvent onRemoveSetCallback;
    public UnityEvent onEmptySetCallback;

    public void Add(T item)
    {
        if(!items.Contains(item))
            items.Add(item);
    }

    public void Remove(T item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            onRemoveSetCallback?.Invoke();
        }
        if(Count == 0)
        {
            onEmptySetCallback?.Invoke();
        }
    }

}
