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
    protected Vector2 damageDir;
    protected float freezeVelocity = 3;

    protected float freezeTime;
    protected float immunityTime;

    public virtual bool TryTakeDamage(DamageInfo dmgInf)
    {
        //Debug.Log(immunityTime);

        if ((IsCaptured && dmgInf.Source == DamageSource.Enemy)
            || (!IsCaptured && dmgInf.Source == DamageSource.Player)
            || dmgInf.Source == DamageSource.Environment)
        {
            if (immunityMomentForThrowingActive && dmgInf.Source == DamageSource.Environment)
            {
                return false;
            }

            if (immunityTime > 0)
            {
                return true;
            }
            Health -= dmgInf.Amount;
            PlaySound(AudioClips[0]);
            GetDamageEffect(dmgInf);
            StartCoroutine(RedSprite());

            Debug.Log($"Creature hurt : health = {Health}");

            TryPassOut();
            TryDie();
            return true;
        }
        return false;
    }

    public void GetDamageEffect(DamageInfo dmgInf)
    {
        freezeTime = dmgInf.FreezeTime;
        immunityTime = OtherConstants.CommonImmunityTime;
        myRigidbody.velocity += dmgInf.Direction * (freezeVelocity * dmgInf.DamageVelocityMultiplier);
    }

    private void UpdateDamageTimers()
    {
        if (immunityTime >= 0)
        {
            immunityTime -= Time.deltaTime;
        }
        if (freezeTime > 0)
        {
            freezeTime -= Time.deltaTime;
        }
    }
}
