using UnityEngine;

public class FrozenFish : AMeleeWeapon
{
    private Animator animator;

    protected new DamageType? damageType = DamageType.Melee;

    protected override void Start()
    {
        animator = GetComponent<Animator>();
        radiusAttack = 0.7f;
    }
    
    protected override void Fire(InputInfo inpInf)
    {
        PlaySound(AudioClips[0]);
        animator.SetTrigger("isAttack");
        foreach (var obj in GetObjectsAround())
        {
            obj.GetComponent<IDamagable>()?
                .TryTakeDamage(new DamageInfo(DamageType.Melee, damageSource, damageAmount, inpInf.GetMouseDir()));
        }
    }
}