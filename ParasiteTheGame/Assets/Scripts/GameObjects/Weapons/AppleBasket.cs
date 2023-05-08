//using System;
//using System.Collections.Generic;
using UnityEngine;

public class AppleBasket : AWeapon
{
    [SerializeField] protected AppleProjectile appleProjPrefab;

    protected static float busketWidth = 0.8f;

    protected override void Start()
    {
        throwComponent = GetComponent<ThrowComponent>();
        throwComponent.enabled = false; //just in case weapon creator forgets
        fireRate = 0.2f;
        damageAmount = 2;
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
        apple.SetParameters(new DamageInfo(DamageType.Distant, damageSource, 1), currentDirection, (Random.value - 0.5f) * 360);
    }
}
