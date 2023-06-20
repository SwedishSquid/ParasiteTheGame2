using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GardenerSuperAttack : MonoBehaviour
{
    [SerializeField] protected GardenerSuperProjectile gardenerSuperProjPrefab; 

    //private static int countProj = 5; //U can do more/less rats
    private static Vector3[] directions;
    private static float[] rotate_dirs;
    
    protected virtual void Start()
    {
        rotate_dirs = new float[] { -30, -15, 0, 13, 30 };
        directions = new Vector3[5];
        for (var i = 0; i < 3; i++)
        {
            var angle = rotate_dirs[i] * Math.PI / 360;
            directions[i] = new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle), 0);
        }
    }

    public void Attack(DamageSource damageSource, Gardener gardener, InputInfo inpInf)
    { 
        GardenerSuperProjectile proj;
        Vector3 mouseVector;
        mouseVector = inpInf.GetMouseDir();
        foreach (var dir in rotate_dirs)
        {
            proj = Instantiate(gardenerSuperProjPrefab, gardener.transform.position, transform.rotation);
            proj.SetParameters(new DamageInfo(DamageType.Distant, damageSource, 3, 
                Quaternion.Euler(0f, 0f, dir) * mouseVector));
        }
    }
}