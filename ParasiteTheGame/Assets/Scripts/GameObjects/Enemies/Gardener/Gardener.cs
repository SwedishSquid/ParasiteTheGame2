using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gardener : AEnemyPlus
{
    float lastX;
    float lastY;
    private float maxAttackCooldown = 4f; 
    private float attackCooldown;
    private float maxSuperAnimation = 1.28f; 
    private float superAnimation;
    private bool isSuper = false;
    [SerializeField] GardenerSuperAttack superAttack;

    public override bool CanBeCaptured
    {
        get
        {
            return PassedOut && !Dead;
        }
    }

    public override void ControlledUpdate(InputInfo inpInf)
    {
        base.ControlledUpdate(inpInf);
        //
        attackCooldown -= Time.deltaTime;
        //animator.SetBool("isSuper", isSuper);
        if (isSuper)
        {
            superAnimation -= Time.deltaTime;
            if (superAnimation <= 0) //|| animator.GetBool("isMoving"))
            {
                isSuper = false;
            }
            
            animator.SetBool("isSuper", isSuper);
        }
        //
        if (!PauseController.gameIsPaused)
        {
            animator.SetFloat("moveX", inpInf.Axis.x);
            animator.SetFloat("moveY", inpInf.Axis.y);
            lastX = inpInf.Axis.x;
            lastY = inpInf.Axis.y;
        }
        else
        {
            animator.SetFloat("moveX", lastX);
            animator.SetFloat("moveY", lastY);
            return;
        }
        //
        if (freezeTime <= 0 && (inpInf.Axis.x != 0 || inpInf.Axis.y != 0))
        {
            //
            isSuper = false;
            animator.SetBool("isSuper", isSuper);
            //
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
        //
        if (inpInf.SuperAttackPressed
            && attackCooldown <= 0)
        {
            isSuper = true;
            attackCooldown = maxAttackCooldown;
            superAnimation = maxSuperAnimation;
            superAttack.Attack(damageSource, this);
            animator.SetBool("isSuper", isSuper);
        }
    }

    public override void OnCapture(PlayerController player)
    {
        base.OnCapture(player);
        //
        animator.SetBool("isUncontious", false);
        //GetComponent<GardenerAI>().enabled = false;
        //
    }

    public override void OnRelease(PlayerController player)
    {
        base.OnRelease(player);
        //
        if (health <= 0)
        {
            animator.SetBool("isUncontious", true);
        }
/*        else
        {
            GetComponent<GardenerAI>().enabled = true;
        }*/
        animator.SetBool("isMoving", false);
        //
    }

    public override bool TryTakeDamage(DamageInfo dmgInf)
    {
        var result = base.TryTakeDamage(dmgInf);
        if (result)
        {
            if (health <= 0)
            {
                animator.SetBool("isUncontious", true);
            }
            else
            {
                healthBar.SetValue(health);
                animator.SetBool("isUncontious", false);
                animator.SetBool("isMoving", false);
            }



            if (BossfightController.Instance != null &&
                PassedOut && BossfightController.Instance.BossfightState == BossfightState.Continued
                && BossfightController.Instance.GetBossGUID() == GetGUID())
            {
                BossfightController.Instance.EndBossfight();
            }
        }
        return result;
    }
}
