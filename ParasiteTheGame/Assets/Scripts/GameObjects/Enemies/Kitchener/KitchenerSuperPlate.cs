using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenerSuperPlate : KitchenerSuperProjectile
{
    protected float currentAngle = 0;

    /// <param name="rotationAngle">degrees per second</param>
    public override void SetParameters(DamageInfo dmjInf, Vector3 direction, float angle)
    {
        this.rotationAngle = (UnityEngine.Random.value - 0.5f) * 360;
        base.SetParameters(dmjInf, direction, rotationAngle);
    }

    protected override void Update()
    {
        currentAngle += (rotationAngle * Time.deltaTime) % 360;
        transform.rotation = Quaternion.AngleAxis(currentAngle, Vector3.forward);
        base.Update();
    }
}
