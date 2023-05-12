using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageInfo
{
    public int Amount;
    public DamageType Type;
    public DamageSource Source;
    public Vector2 Direction { get; }

    public DamageInfo(DamageType type, DamageSource source, int amount, Vector2 damageDir)
    {
        Type = type;
        Source = source;
        Amount = amount;
        Direction = damageDir;
    }
}
