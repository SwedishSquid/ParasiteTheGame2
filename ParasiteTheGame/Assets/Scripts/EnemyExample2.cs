using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExample2 : AEnemy
{
    new protected string typeName = "EnemyExample2";
    protected override void Start()
    {
        health = 150;
        base.Start();
    }
}
