using System;
using System.Collections;
using System.Collections.Generic;
using CrucialComponents.Interfaces;
using UnityEngine;

public class TargetAnim : MonoBehaviour, IHitEffectable
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void DoHitEffect(Vector2 direction)
    {
        animator.SetBool("isSuccess", true);
        Invoke(nameof(TurnOffEffects), 0.5f);
    }

    private void TurnOffEffects() => animator.SetBool("isSuccess", false);
    
}
