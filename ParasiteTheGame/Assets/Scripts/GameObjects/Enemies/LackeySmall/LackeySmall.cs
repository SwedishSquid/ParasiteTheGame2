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
        myRigidbody.velocity = inpInf.Axis * velocity;
        //
        animator.SetFloat("moveX", inpInf.Axis.x);
        animator.SetFloat("moveY", inpInf.Axis.y);
        if (inpInf.Axis.x != 0 || inpInf.Axis.y != 0)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
        //
        if (item != null)
        {
            item.HandleUpdate(inpInf);
            if (inpInf.ThrowItemPressed)
            {
                item.Throw(inpInf);
                item = null;
            }
        }
    }

    public override void OnCapture(PlayerController player)
    {
        base.OnCapture(player);
        //
        animator.SetBool("isUncontious", false);
        GetComponent<LackeySmallAI>().enabled = false;
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
        else
        {
            GetComponent<LackeySmallAI>().enabled = true;
        }
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
                GetComponent<LackeySmallAI>().enabled = false;
                if (item != null)
                {
                    DropDown();
                }
            }
            else
            {
                healthBar.SetValue(health);
                animator.SetBool("isUncontious", false);
            }
        }
        return result;
    }
}
