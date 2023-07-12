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
    private bool isSuper;
    [SerializeField] GardenerSuperAttack superAttack;

    protected override void Awake()
    {
        base.Awake();
        radius = 1.5f;
        Health = 100;
        MaxHealth = 100;
        terminalHealth = 35;
        isBoss = true;
    }

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
        if (!PauseController.gameIsPaused && isSuper)
        {
            superAnimation -= Time.deltaTime;
            if (superAnimation <= 0) 
            {
                isSuper = false;
            }
            
            animator.SetBool("isSuper", isSuper);
        }
        //
        if (!PauseController.gameIsPaused)
        {
            attackCooldown -= Time.deltaTime;
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
            // ITSIgma - Cancel SuperAttack when move
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
        if (!PauseController.gameIsPaused
            && inpInf.SuperAttackPressed
            && attackCooldown <= 0)
        {
            PlaySound(AudioClips[2]);
            isSuper = true;
            attackCooldown = maxAttackCooldown;
            superAnimation = maxSuperAnimation;

            superAttack.Attack(DamageSource, this, inpInf);
            //superAttack.Attack(damageSource, this);
            PlaySound(AudioClips[1]);

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
        if (Health <= 0)
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
            if (Health <= 0)
            {
                animator.SetBool("isUncontious", true);
            }
            else
            {
                if (BossfightController.Instance != null 
                    && BossfightController.Instance.BossfightState == BossfightState.Finished)
                    healthBar.SetValue(Health);
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
