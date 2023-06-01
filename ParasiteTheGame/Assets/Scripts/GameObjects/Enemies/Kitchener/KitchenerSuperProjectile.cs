using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenerSuperProjectile : AProjectile
{
    protected float rotationAngle;

    /// <param name="rotationAngle">degrees per second</param>
    public virtual void SetParameters(DamageInfo dmjInf, Vector3 direction, float angle)
    {
        rotationAngle = (float)((angle - Math.PI / 2) * 180 / Math.PI);
        transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
        SetParameters(dmjInf, direction, 5f, 1f, 4f);
    }
}
