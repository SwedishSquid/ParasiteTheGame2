using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    [NonSerialized] public IControlable thisEnemy; //must contain a pointer to constructed object after LoadData called

    public Vector3 EnemyPosition;

    public string PickedItemGUID = "";

    public bool CanBeCaptured;

    public int Health;

    public string TypeName = "";
}
