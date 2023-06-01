using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttack : AMeleeWeapon
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    //[SerializeField] new private AEnemy user;

    protected override void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        damageAmount = 2;
        user = GetComponentInParent<AEnemy>();
        radiusAttack = 0.7f;
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
        foreach (var obj in GetObjectsAround())
        {
            obj.GetComponent<IDamagable>()?
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
