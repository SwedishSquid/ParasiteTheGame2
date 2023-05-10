using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Postman : AEnemyPlus
{
    // Start is called before the first frame update

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

    public override void OnRelease(PlayerController player)
    {
        base.OnRelease(player);
        animator.SetBool("isMoving", false);
    }
}
