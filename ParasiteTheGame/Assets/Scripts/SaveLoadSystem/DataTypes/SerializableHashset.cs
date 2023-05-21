using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableHashset<T> : HashSet<T>, ISerializationCallbackReceiver
{
    [SerializeField] private List<T> items = new List<T>();

    public void OnAfterDeserialize()
    {
        foreach(var item in items)
        {
            Add(item);
        }
    }

    public void OnBeforeSerialize()
    {
        items.Clear();
        foreach(var item in this)
        {
            items.Add(item);
        }
    }
}
