using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeWeapon : AWeapon
{
    private LayerMask damageTakerLayers;
    private bool didDamage = false;

    protected override void Fire(InputInfo inpInf)
    {
        if (user.GetDamageSource() is DamageSource.Player)
            damageTakerLayers = LayerMask.GetMask("Controllables");
        else
            damageTakerLayers = LayerMask.GetMask("Player");
        var enemies = Physics2D.OverlapCircleAll(transform.position, 0.6f, damageTakerLayers);
        for (int i = 0; i < enemies.Length; i++)
        {
            didDamage = enemies[i].GetComponent<AEnemy>()
                .TryTakeDamage(new DamageInfo(DamageType.Melee, damageSource, 4),
                    inpInf.GetMouseDir());
        }
    }
}
