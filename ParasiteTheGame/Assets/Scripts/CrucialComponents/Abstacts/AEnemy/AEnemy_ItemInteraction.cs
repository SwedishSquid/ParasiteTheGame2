using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
//using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
//using UnityEditor.Sprites;
using UnityEngine;


public abstract partial class AEnemy : ASoundable, IControlable, IDamagable, IUser, ISavable, IEnemyInfoPlate, IKillable
{
    protected string itemGUID = "";
    protected float itemPickingRadius = 2f;

    protected bool immunityMomentForThrowingActive = false;
    protected float ThrowMomentDuration = 0.05f;

    public virtual void ActOnPickOrDrop()
    {
        if (item == null)
        {
            PickUp();
        }
        else
        {
            DropDown();
        }
    }

    protected virtual void PickUp()
    {
        //ITSigma - Pause
        if (PauseController.gameIsPaused)
            return;
        //

        var t = Physics2D.OverlapCircle(transform.position, itemPickingRadius, LayerConstants.PickableItems);
        if (t)
        {
            item = t.gameObject.GetComponent<IUsable>();
            item?.OnPickUp(this);
            if (t.gameObject.TryGetComponent<ISavable>(out var savable))
            {
                itemGUID = savable.GetGUID();
            }
        }
        else
        {
            //Debug.Log("nothing to pick up");
        }
    }

    protected virtual void DropDown()
    {
        //ITSigma - Pause
        if (PauseController.gameIsPaused)
            return;
        //

        item.OnDropDown(this);
        item = null;
        itemGUID = "";
    }

    protected virtual void PerformThrow(InputInfo inpInf)
    {
        immunityMomentForThrowingActive = true;
        item.Throw(inpInf);
        item = null;
        itemGUID = "";
        Invoke(nameof(RestoreImmunityMoment), ThrowMomentDuration);
    }

    private void RestoreImmunityMoment()
    {
        immunityMomentForThrowingActive = false;
    }
}
