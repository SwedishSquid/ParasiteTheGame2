using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAnim : MonoBehaviour, IDamagable
{
    private Animator animator;
    private AudioSource audioSource;
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public bool TryTakeDamage(DamageInfo dmgInf)
    {
        animator.SetTrigger("isSuccess");
        audioSource.Play();
        return true;
    }
}
