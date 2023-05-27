using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BossfightData : ISerializationCallbackReceiver
{
    [SerializeField] private int bs;

    [NonSerialized] public BossfightState bossfightState;

    public void OnAfterDeserialize()
    {
        bossfightState = (BossfightState)bs;
    }

    public void OnBeforeSerialize()
    {
        bs = (int)bossfightState;
    }
}
