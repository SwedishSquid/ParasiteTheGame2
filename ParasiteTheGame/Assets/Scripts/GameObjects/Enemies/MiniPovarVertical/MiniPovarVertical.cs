using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniPovarVertical : AEnemyPlus
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
        GetComponent<MiniPovarVerticalAI>().enabled = false;
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
        else
        {
            GetComponent<MiniPovarVerticalAI>().enabled = true;
        }
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
                GetComponent<MiniPovarVerticalAI>().enabled = false;
                if (item != null)
                {
                    DropDown();
                }
            }
            else
            {
                healthBar.SetValue(Health);
                animator.SetBool("isUncontious", false);
                animator.SetBool("isMoving", false);
            }
        }
        return result;
    }

    public override void SaveGame(GameData gameData)
    {
        //nope
    }

    public override void LoadData(GameData gameData)
    {
        //nope
    }
}
