using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttack : AWeapon
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float radiusAttack = 0.7f;
    //[SerializeField] new private AEnemy user;

    protected override void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        damageAmount = 2;
        user = GetComponentInParent<AEnemy>();
    }

    protected override void HandleMovement(InputInfo inpInf)
    {
        var desiredRotation = inpInf.GetMouseDir();
        var angle = Mathf.Atan2(desiredRotation.y, desiredRotation.x) * (180 / Mathf.PI);
        transform.position = user.GetUserPosition() + desiredRotation;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        spriteRenderer.flipY = angle < 0;
    }

    protected override void Fire(InputInfo inpInf)
    {
        animator.SetTrigger("Attack");
        var enemies = Physics2D.OverlapCircleAll(transform.position, radiusAttack,
            LayerConstants.DamageTakersLayer);
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<IDamagable>()?
                .TryTakeDamage(new DamageInfo(DamageType.Melee, damageSource, damageAmount, inpInf.GetMouseDir()));
        }
    }

    public override void SaveGame(GameData gameData) {}
    
    public override void LoadData(GameData gameData){}

/*    public override string GetGUID()
    {
        return "nonono";
    }*/
}
