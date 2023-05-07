using System;
using System.Collections.Generic;
using UnityEngine;

public class AppleBasket : AWeapon
{
    [SerializeField] protected AppleProjectile appleProjPrefab;
    protected AppleProjectile currentApple;
    protected override void HandleMovement(InputInfo inpInf)
    {
        if (currentApple == null)
        {
            currentApple = Instantiate(appleProjPrefab);
        }
        throw new NotImplementedException();
        var x = inpInf.GetMouseDir().x;
        x = x != 0 ? Mathf.Sign(x) : 1;
        var desiredRotation = new Vector2(x, 0);
        transform.position = user.GetUserPosition() + (desiredRotation * user.GetUserRadius());
    }

    protected override void Fire(InputInfo inpInf)
    {

    }
}
