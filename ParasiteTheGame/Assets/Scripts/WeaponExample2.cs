using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponExample2 : AWeapon
{
    [SerializeField] protected BulletExample bulletPrefab;
    protected override void Fire(InputInfo inpInf)
    {
        audioSource.Play();
        Vector3 currentDirection = inpInf.GetMouseDir();
        var bullet = Instantiate(bulletPrefab, transform.position + currentDirection * 0.6f, transform.rotation);
        bullet.SetParameters(new DamageInfo(DamageType.Distant, damageSource, 1, currentDirection),
            currentDirection, 10, 0.2f, 2000);
    }
}
