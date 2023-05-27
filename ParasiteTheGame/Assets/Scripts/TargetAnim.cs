using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAnim : MonoBehaviour, IDamagable
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public bool TryTakeDamage(DamageInfo dmgInf)
    {
        animator.SetTrigger("isSuccess");
        return true;
    }
}
