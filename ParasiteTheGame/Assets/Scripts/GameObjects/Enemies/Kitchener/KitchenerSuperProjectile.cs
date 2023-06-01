using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenerSuperProjectile : AProjectile
{
    protected float rotationAngle;
    protected float currentAngle = 0;

    /// <param name="rotationAngle">degrees per second</param>
    public virtual void SetParameters(DamageInfo dmjInf, Vector3 direction, float rotationAngle = 0)
    {
        SetParameters(dmjInf, direction, 8f, 1f, 2f);
        this.rotationAngle = rotationAngle;
    }

    protected override void Update()
    {
        transform.position += Time.deltaTime * velocity * direction;
        currentAngle += (rotationAngle * Time.deltaTime) % 360;
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
