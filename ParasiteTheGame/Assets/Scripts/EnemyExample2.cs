using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExample2 : AEnemy
{
    protected override void Start()
    {
        base.Start();
        health = 150;
    }
}
