using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AEnemyPlus : AEnemy
{
    protected Animator animator;
    [SerializeField] protected HealthBar healthBar;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        healthBar.SetMaxHealth(health);
    }

    public override bool TryTakeDamage(DamageInfo dmgInf)
    {
        var answer = base.TryTakeDamage(dmgInf);
        if (answer && health >= 0 && healthBar != null) healthBar.SetValue(health);
        return answer;
    }
}
