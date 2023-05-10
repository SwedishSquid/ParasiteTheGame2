using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LackeySmall : AEnemy
{
    Animator animator;
    protected override void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        radius = 2;
        animator = GetComponent<Animator>();
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
        player.GetComponent<SpriteRenderer>().enabled = false;
        animator.SetBool("isUncontious", false);
        GetComponent<LackeySmallAI>().enabled = false;
        //
    }

    public override void OnRelease(PlayerController player)
    {
        base.OnRelease(player);
        //
        player.GetComponent<SpriteRenderer>().enabled = true;
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
        if ((IsCaptured && dmgInf.Source == DamageSource.Enemy)
            || (!IsCaptured && dmgInf.Source == DamageSource.Player)
            || dmgInf.Source == DamageSource.Environment)
        {
            health -= dmgInf.Amount;
            Debug.Log($"Enemy hurt : health = {health}");
            //
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
                animator.SetBool("isUncontious", false);
            }
            //
            return true;
        }
        return false;
    }

    protected override void PickUp()
    {
        var t = Physics2D.OverlapCircle(transform.position, itemPickingRadius, Constants.PickableItems);
        if (t)
        {
            item = t.gameObject.GetComponent<IUsable>();
            item?.OnPickUp(this);
        }
        else
        {
            
            Debug.Log("nothing to pick up");
            
        }
    }
}
