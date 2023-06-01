using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackOfeMail : AWeapon
{
    [SerializeField] protected eMailProjectile mailPrefab;

    protected override void Fire(InputInfo inpInf)
    {
        audioSource.Play();
        Vector3 currentDirection = inpInf.GetMouseDir();
        var eMail = Instantiate(mailPrefab, transform.position + currentDirection * 0.6f, transform.rotation);
        eMail.SetParameters(new DamageInfo(DamageType.Distant, damageSource, 1, currentDirection),
            currentDirection);
    }
}
