using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Postman : AEnemy
{
    // Start is called before the first frame update
    Animator animator;

    protected override void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
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
        IsCaptured = true;
        player.GetComponent<SpriteRenderer>().enabled = false;
    }

    public override void OnRelease(PlayerController player)
    {
        myRigidbody.velocity = new Vector2(0, 0);
        IsCaptured = false;
        animator.SetBool("isMoving", false);
        player.GetComponent<SpriteRenderer>().enabled = true;
    }
}
