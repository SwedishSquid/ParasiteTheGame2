using UnityEngine;

public class FrozenFish : AMeleeWeapon
{
    private Animator animator;

    protected override void Start()
    {
        animator = GetComponent<Animator>();
        radiusAttack = 0.7f;
        damageType = DamageType.Melee;
    }
    
    protected override void Fire(InputInfo inpInf)
    {
        audioSource.Play();
        animator.SetTrigger("isAttack");
        foreach (var obj in GetObjectsAround())
        {
            obj.GetComponent<IDamagable>()?
                .TryTakeDamage(new DamageInfo(DamageType.Melee, damageSource, damageAmount, inpInf.GetMouseDir()));
        }
    }
}