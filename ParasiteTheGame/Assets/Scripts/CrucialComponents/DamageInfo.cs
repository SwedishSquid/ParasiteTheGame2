using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageInfo
{
    public int Amount;
    public DamageType Type;
    public DamageSource Source;

    public DamageInfo(DamageType type, DamageSource source, int amount)
    {
        Type = type;
        Source = source;
        Amount = amount;
    }
}
