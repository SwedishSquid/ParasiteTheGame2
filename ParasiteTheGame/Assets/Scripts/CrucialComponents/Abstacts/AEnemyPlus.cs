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
        healthBar.SetMaxHealth(health, true);
    }

    public override bool TryTakeDamage(DamageInfo dmgInf)
    {
        var answer = base.TryTakeDamage(dmgInf);
        if (answer && health >= 0 && healthBar is not null)
        {
            if (IsCaptured)
                Capturer.HealthBarEnemy.SetValue(health);
            else
                healthBar.SetValue(health);
        }
        return answer;
    }
}
