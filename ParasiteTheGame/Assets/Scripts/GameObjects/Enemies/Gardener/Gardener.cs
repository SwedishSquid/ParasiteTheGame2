using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gardener : AEnemyPlus
{
    float lastX;
    float lastY;
    private float maxAttackCooldown = 4f; 
    private float attackCooldown;
    [SerializeField] GardenerSuperAttack superAttack;

    protected override void Start()
    {
        base.Start();
        radius = 2;
    }

    public override void ControlledUpdate(InputInfo inpInf)
    {
        base.ControlledUpdate(inpInf);
        //
        attackCooldown -= Time.deltaTime;
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
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
        //
        if (damageSource == DamageSource.Player 
            && Input.GetButtonDown("SuperAttack") 
            && attackCooldown <= 0)
        {
            attackCooldown = maxAttackCooldown;
            superAttack.Attack(damageSource, this);
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
/*                GetComponent<GardenerAI>().enabled = false;
                if (item != null)
                {
                    DropDown();
                }*/
            }
            else
            {
                healthBar.SetValue(health);
                animator.SetBool("isUncontious", false);
                animator.SetBool("isMoving", false);
            }
        }
        return result;
    }
}
