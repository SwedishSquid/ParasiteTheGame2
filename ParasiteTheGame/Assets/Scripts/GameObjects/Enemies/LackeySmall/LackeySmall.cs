using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LackeySmall : AEnemyPlus
{
    protected override void Start()
    {
        base.Start();
        radius = 2;
    }

    public override void ControlledUpdate(InputInfo inpInf)
    {
        base.ControlledUpdate(inpInf);
        //
        animator.SetFloat("moveX", inpInf.Axis.x);
        animator.SetFloat("moveY", inpInf.Axis.y);
        if (freezeTime <= 0 && (inpInf.Axis.x != 0 || inpInf.Axis.y != 0))
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
        //
    }

    public override void OnCapture(PlayerController player)
    {
        base.OnCapture(player);
        //
        animator.SetBool("isUncontious", false);
        //
    }

    public override void OnRelease(PlayerController player)
    {
        base.OnRelease(player);
        //
        animator.SetBool("isMoving", false);
        //
    }

    public override bool TryTakeDamage(DamageInfo dmgInf)
    {
        var result = base.TryTakeDamage(dmgInf);
        if (result)
        {
            if (!Dead)
            {
                healthBar.SetValue(health);
                //animator.SetBool("isUncontious", false);
                animator.SetBool("isMoving", false);
                animator.SetTrigger("damage");
            }
        }
        return result;
    }
}
