using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AMeleeWeapon : AWeapon
{
    protected float radiusAttack;

    protected Collider2D[] GetObjectsAround()
    {
        return Physics2D.OverlapCircleAll(transform.position, radiusAttack,
            LayerConstants.DamageTakersLayer);
    }
}