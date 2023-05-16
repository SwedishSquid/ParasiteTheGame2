using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TypeObjectPair<TKey, TValue>
{
    public TKey Type;
    public TValue Object;

    public TypeObjectPair(TKey type, TValue @object)
    {
        Type = type;
        Object = @object;
    }
}
