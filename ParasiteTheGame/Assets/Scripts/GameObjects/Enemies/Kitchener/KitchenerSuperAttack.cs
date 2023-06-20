using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class KitchenerSuperAttack : MonoBehaviour
{
    [SerializeField] protected KitchenerSuperProjectile[] kitchenerSuperProjPrefab; 

    private static int countProj = 12; //U can do more/less projs
    private static Vector3[] directions;
    private static float[] rotate_dirs;
    
    protected virtual void Start()
    {
        directions = new Vector3[countProj];
        rotate_dirs = new float[countProj];
        var angle = Math.PI * 2 / countProj;
        for (var i = 0; i < countProj; i++)
        {
            rotate_dirs[i] = (float)angle * i;
            directions[i] = new Vector3((float)Math.Cos(rotate_dirs[i]), (float)Math.Sin(rotate_dirs[i]), 0);
        }
    }

    public void Attack(DamageSource damageSource, Kitchener kitchener)
    { 
        KitchenerSuperProjectile proj;
        Quaternion rotation;
        float angle;
        for (var i = 0; i < countProj; i++)
        {
            angle = rotate_dirs[i];
            rotation = Quaternion.AngleAxis(rotate_dirs[i], Vector3.forward);
            proj = Instantiate(kitchenerSuperProjPrefab[UnityEngine.Random.Range(0, kitchenerSuperProjPrefab.Length)], 
                kitchener.transform.position, rotation);
            proj.SetParameters(new DamageInfo(DamageType.Distant, damageSource, 2, directions[i]), directions[i], rotate_dirs[i]);
        }
    }
}