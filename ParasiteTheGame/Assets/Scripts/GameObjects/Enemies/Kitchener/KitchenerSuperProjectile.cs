using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenerSuperProjectile : AProjectile
{
    /// <param name="rotationAngle">degrees per second</param>
    public virtual void SetParameters(DamageInfo dmjInf, Vector3 direction)
    {
        SetParameters(dmjInf, direction, 7f, 1f, 4f);
    }
}
