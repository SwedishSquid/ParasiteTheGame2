using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenerSuperPlate : KitchenerSuperProjectile
{
    protected float rotationAngle;
    protected float currentAngle = 0;

    /// <param name="rotationAngle">degrees per second</param>
    public override void SetParameters(DamageInfo dmjInf, Vector3 direction)
    {
        SetParameters(dmjInf, direction, 5f, 1f, 4f);
        this.rotationAngle = (UnityEngine.Random.value - 0.5f) * 360;
    }

    protected override void Update()
    {
        transform.position += Time.deltaTime * velocity * direction;
        currentAngle += (rotationAngle * Time.deltaTime * velocity) % 360;
        transform.rotation = Quaternion.AngleAxis(currentAngle, Vector3.forward);
        var obg = Physics2D.Raycast(transform.position, direction, rayLength, LayerConstants.DamageTakersLayer);
        if (obg)
        {
            var damagable = obg.collider.gameObject.GetComponent<IDamagable>();
            //walls can have no scripts and thus can be not a IDamagable instance
            if (damagable is null || damagable.TryTakeDamage(damageInfo))
            {
                Destroy(gameObject);
            }
        }

        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
