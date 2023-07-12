using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
//using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
//using UnityEditor.Sprites;
using UnityEngine;


public abstract partial class AEnemy : IUser
{
    protected float radius = 1.06f;

    public bool HaveItem => item != null;

    public virtual Vector2 GetUserPosition()
    {
        return transform.position;
    }

    public virtual Vector2 GetUserVelocity()
    {
        return myRigidbody.velocity;
    }

    public virtual float GetUserHeight()
    {
        throw new System.NotImplementedException();
    }
    public virtual float GetUserWidth()
    {
        throw new System.NotImplementedException();
    }
    public virtual float GetUserRadius()
    {
        return radius;
    }

    public virtual DamageSource GetDamageSource()
    {
        return DamageSource;
    }

    public DamageType GetDamageType()
    {
        if (HaveItem)
        {
            return item.GetDamageType();
        }
        return DamageType.Melee;
    }
}
