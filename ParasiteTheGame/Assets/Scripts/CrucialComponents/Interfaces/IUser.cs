using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is primary for IUsable to get information about user
public interface IUser
{
    public bool HaveItem { get; }

    public Vector2 GetUserPosition();

    public Vector2 GetUserVelocity();

    public float GetUserHeight();
    public float GetUserWidth();
    public float GetUserRadius();

    public DamageSource GetDamageSource();

    public DamageType GetDamageType();
}
