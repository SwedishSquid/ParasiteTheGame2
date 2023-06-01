using UnityEngine;
using System;

public class PanWithPancake : AWeapon
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    [SerializeField] protected PancakeProjectile pancakeProjectile;
    
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    protected override void HandleMovement(InputInfo inpInf)
    {
        var desiredRotation = inpInf.GetMouseDir();
        var angle = Mathf.Atan2(desiredRotation.y, desiredRotation.x) * (180 / Mathf.PI);
        transform.position = user.GetUserPosition() + (desiredRotation * user.GetUserRadius());
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        spriteRenderer.flipY = Math.Abs(angle) >= 90;
    }

    protected override void Fire(InputInfo inpInf)
    {
        animator.SetTrigger("isAttack");
        Vector3 currentDirection = inpInf.GetMouseDir();
        var pancake = Instantiate(pancakeProjectile, transform.position + (currentDirection * 0.6f),
            transform.rotation);
        pancake.SetParameters(new DamageInfo(DamageType.Distant, damageSource, 1, currentDirection),
            currentDirection);
    }
}