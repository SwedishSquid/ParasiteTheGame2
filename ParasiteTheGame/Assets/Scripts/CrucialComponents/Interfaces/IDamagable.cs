using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public bool TryTakeDamage(DamageInfo dmgInf);
}
