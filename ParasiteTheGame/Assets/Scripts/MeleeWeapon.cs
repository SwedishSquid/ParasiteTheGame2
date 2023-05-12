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

    protected override void Start()
    {
        anim = GetComponent<Animator>();
        throwComponent = GetComponent<ThrowComponent>();
        throwComponent.enabled = false; //just in case weapon creator forgets
    }

    protected override void Fire(InputInfo inpInf)
    {
        anim.SetBool(hitName, true);
        animTime = 0.16f;
        if (user.GetDamageSource() is DamageSource.Player)
            damageTakerLayers = LayerMask.GetMask("Controllables");
        else
            damageTakerLayers = LayerMask.GetMask("Player");
        var enemies = Physics2D.OverlapCircleAll(transform.position, 0.7f, damageTakerLayers);
        for (int i = 0; i < enemies.Length; i++)
        {
            didDamage = enemies[i].GetComponent<AEnemy>()
                .TryTakeDamage(new DamageInfo(DamageType.Melee, damageSource, 4, transform.position.normalized));
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
