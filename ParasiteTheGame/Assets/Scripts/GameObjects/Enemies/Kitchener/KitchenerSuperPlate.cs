using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenerSuperPlate : KitchenerSuperProjectile
{
    /// <param name="rotationAngle">degrees per second</param>
    public override void SetParameters(DamageInfo dmjInf, Vector3 direction, float rotationAngle = 0)
    {
        SetParameters(dmjInf, direction, 8f, 1f, 2f);
        this.rotationAngle = (UnityEngine.Random.value - 0.5f) * 360;
    }
}
