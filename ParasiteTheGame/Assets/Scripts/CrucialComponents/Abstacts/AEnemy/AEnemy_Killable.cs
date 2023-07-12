using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
//using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
//using UnityEditor.Sprites;
using UnityEngine;


public abstract partial class AEnemy : IKillable, IEnemyInfoPlate
{
    //why all hardcoded in here??
    public int MaxHealth { get; set; } = 30;

    protected int terminalHealth = 30 / 2;
    
    public int Health { get; set; } = 30;
    public virtual bool AlmostPassedOut => !PassedOut && (Health - terminalHealth) < MaxHealth / 5;
    public virtual bool PassedOut => Health < terminalHealth;
    public virtual bool Dead => Health <= 0;

    public Vector2 Position => transform.position;

    public bool CanBeHit => IsCaptured;

    public float RelativeTerminalHealth => terminalHealth / (float)MaxHealth;

    //is it ok ??
    public int GetHealth() => Health;
    public int GetMaxHealth() => MaxHealth;

    public virtual bool TryPassOut()
    {
        if (!PassedOut || Dead)
        {
            return false;
        }

        if (item != null && !IsCaptured)
        {
            DropDown();
        }

        return true;
    }

    public bool TryDie()
    {
        if (!Dead)
        {
            return false;
        }

        ApplyDeathEffects();

        gameObject.GetComponent<Collider2D>().enabled = false;
        myRigidbody.simulated = false;

        return true;
    }

    protected virtual void ApplyDeathEffects()
    {
        if (item != null)
        {
            DropDown();
        }

        intelligence.TryTurnOff();
        if (IsCaptured)
        {
            IsCaptured = false;
            Capturer.TryHandleJump(true, new Vector2(UnityEngine.Random.value, UnityEngine.Random.value).normalized);
        }
    }
}
