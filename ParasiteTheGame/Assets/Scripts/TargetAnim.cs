using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAnim : ASoundable, IDamagable
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public bool TryTakeDamage(DamageInfo dmgInf)
    {
        animator.SetTrigger("isSuccess");
        PlaySound(AudioClips[0]);
        return true;
    }
}
