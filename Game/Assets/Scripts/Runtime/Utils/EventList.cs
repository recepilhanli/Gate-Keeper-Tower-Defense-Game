using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A list that has events for adding and removing items
/// </summary>
/// <typeparam name="T"></typeparam>
public class EventList<T> : List<T>
{
    public UnityAction<T> OnAdd;
    public UnityAction<T> OnRemove;

    public new void Add(T item)
    {
        base.Add(item);
        OnAdd?.Invoke(item);
    }

    public new void Remove(T item)
    {
        base.Remove(item);
        OnRemove?.Invoke(item);
    }

    public new void Clear()
    {
        base.Clear();
        OnRemove?.Invoke(default);
    }

}
