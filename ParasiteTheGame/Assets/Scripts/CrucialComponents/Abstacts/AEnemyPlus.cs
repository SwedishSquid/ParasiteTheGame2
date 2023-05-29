using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AEnemyPlus : AEnemy
{
    protected Animator animator;
    [SerializeField] protected HealthBar healthBar;
<<<<<<< HEAD
    
=======
    [SerializeField] protected Cloud cloudObject;
    //cloud
>>>>>>> BossAndOther

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        healthBar.SetMaxHealth(maxHealth, true);
        healthBar.SetValue(health);
        UpdateCloud();
    }

    public override bool TryTakeDamage(DamageInfo dmgInf)
    {
        var answer = base.TryTakeDamage(dmgInf);
        if (answer && health >= 0 && healthBar is not null)
        {
            if (!IsCaptured) healthBar.SetValue(health);
        }

        UpdateCloud();

        return answer;
    }

    public override void OnCapture(PlayerController player)
    {
        base.OnCapture(player);
        UpdateCloud();
    }

    public override void OnRelease(PlayerController player)
    {
        base.OnRelease(player);
        UpdateCloud();
    }

    protected override void AfterDataLoaded()
    {
        base.AfterDataLoaded();
        healthBar.SetValue(health);
        UpdateCloud();
    }

    protected void UpdateCloud()
    {
        if (cloudObject == null)
        {
            return;
        }

        if (PassedOut && !Dead && !IsCaptured)
        {
            cloudObject.gameObject.SetActive(true);
        }
        else
        {
            cloudObject.gameObject.SetActive(false);
        }
    }
}
