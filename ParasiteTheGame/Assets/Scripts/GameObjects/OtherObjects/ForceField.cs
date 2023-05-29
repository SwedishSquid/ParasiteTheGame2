using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ForceField : MonoBehaviour, IDamagable
{
    private Animator animator;
    private bool isDamaged = false;
    private float resetTime = 0.7f;

    public bool IsDamaged { get { return isDamaged; } }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public bool TryTakeDamage(DamageInfo dmgInf)
    {
        if (!isDamaged)
        {
            isDamaged = true;
            StartCoroutine(AppearDamaged());
        }
        
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
