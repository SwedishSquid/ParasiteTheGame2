using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeWeapon : AWeapon
{
    private LayerMask damageTakerLayers;
    private bool didDamage;
    public Animator anim;
    private float animTime;
    public string hitName = "Hit";

    protected float attackRadius = 0.7f;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
    }

    protected override void Fire(InputInfo inpInf)
    {
        anim.SetBool(hitName, true);
        animTime = 0.16f;
        var enemies = Physics2D.OverlapCircleAll(transform.position, attackRadius,
            LayerConstants.DamageTakersLayer);
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<IDamagable>()?
                .TryTakeDamage(new DamageInfo(DamageType.Melee, damageSource, damageAmount, inpInf.GetMouseDir()));
        }
    }

    
    protected virtual void Update()
    {
        if (animTime > 0)
        {
            animTime -= Time.deltaTime;
            if (animTime <= 0)
                anim.SetBool(hitName, false);
        }
    }
}
