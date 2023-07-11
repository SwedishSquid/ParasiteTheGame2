using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AEnemyPlus : AEnemy
{
    protected Animator animator;
    [SerializeField] protected HealthBar healthBar;
    [SerializeField] protected Cloud cloudObject;
    [SerializeField] protected Sprite grave;
    protected bool isBoss;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        healthBar.SetMaxHealth(this ,MaxHealth, true);
        healthBar.SetValue(Health);
        UpdateCloud();
    }

    public override bool TryTakeDamage(DamageInfo dmgInf)
    {
        var answer = base.TryTakeDamage(dmgInf);
        if (answer && Health >= 0 && healthBar is not null)
        {
            if (!isBoss && !IsCaptured) healthBar.SetValue(Health);
        }

        UpdateCloud();

        return answer;
    }

    protected override void ApplyDeathEffects()
    {
        base.ApplyDeathEffects();

        if (grave != null)
        {
            GetComponent<Animator>().enabled = false;
            GetComponent<SpriteRenderer>().sprite = grave;
        }
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
        healthBar.SetValue(Health);
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
