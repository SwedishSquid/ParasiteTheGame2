using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageInfo
{
    public int Amount;
    public DamageType Type;
    public DamageSource Source;
    public Vector2 Direction { get; }

    public float DamageVelocityMultiplier;

    public DamageInfo(DamageType type, DamageSource source, int amount, Vector2 damageDir, float damageVelocityMultiplier = 1)
    {
        Type = type;
        Source = source;
        Amount = amount;
        Direction = damageDir;
        DamageVelocityMultiplier = damageVelocityMultiplier;
    }
}
