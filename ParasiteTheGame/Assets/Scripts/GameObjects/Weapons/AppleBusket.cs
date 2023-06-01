//using System;
//using System.Collections.Generic;
using UnityEngine;

public class AppleBusket : AWeapon
{
    [SerializeField] protected AppleProjectile appleProjPrefab;

    protected static float busketWidth = 0.8f;

    protected new DamageType? damageType = DamageType.Distant;

    protected override void Start()
    {
        fireRate = 0.2f;
        damageAmount = 2;
        damageType = DamageType.Distant;
    }

    protected override void HandleMovement(InputInfo inpInf)
    {
        var desiredRotation = inpInf.GetMouseDir();
        var angle = 0f;
        transform.position = user.GetUserPosition() + (desiredRotation * user.GetUserRadius());
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    protected override void Fire(InputInfo inpInf)
    {
        Vector3 currentDirection = inpInf.GetMouseDir();
        var apple = Instantiate(appleProjPrefab, transform.position + (currentDirection * 0.6f), transform.rotation);
        apple.SetParameters(
            new DamageInfo(DamageType.Distant, damageSource, 1, currentDirection, 1, 0.1f),
            currentDirection, (Random.value - 0.5f) * 360);
    }
}
