using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ForceField : ASoundable, IDamagable
{
    private Animator animator;
    private bool isDamaged = false;
    private float resetTime = 0.7f;


    public bool IsDamaged { get { return isDamaged; } }

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public bool TryTakeDamage(DamageInfo dmgInf)
    {
        PlaySound(AudioClips[0]);
        if (!isDamaged)
        {
            isDamaged = true;
            StartCoroutine(AppearDamaged());
        }
        PlaySound(AudioClips[1]);
        return true;
    }

    IEnumerator AppearDamaged()
    {
        animator.SetBool("isDamaged", true);

        yield return new WaitForSeconds(resetTime);

        animator.SetBool("isDamaged", false);
        isDamaged = false;
    }
}
