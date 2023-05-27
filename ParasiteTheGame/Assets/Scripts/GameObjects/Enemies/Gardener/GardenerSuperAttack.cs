using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GardenerSuperAttack : MonoBehaviour
{
    [SerializeField] protected GardenerSuperProjectile gardenerSuperProjPrefab; 

    private static int countProj = 4; //U can do more/less rats
    private static List<Vector3> directions;
    
    protected virtual void Start()
    {
        var dirsTemp = new Tuple<double, double>[countProj]; 
        var angle = Math.PI / 2 / countProj;
        var signs = new int[] { -1, 1 };
        for (var i = 0; i < countProj; i++)
        {
            dirsTemp[i] = Tuple.Create(Math.Cos(angle * i), Math.Sin(angle * i));
        }
        directions = dirsTemp
            .SelectMany(x => 
                signs.SelectMany(s1 => 
                    signs.Select(s2 => 
                        new Vector3(s1 * (float)x.Item1, s2 * (float)x.Item2, 0).normalized)))
            .ToList();
        //directions.Append(Vector3.up * 3); //doesn't work
        //directions.Append(Vector3.down * 3); //doesn't work
    }

    public void Attack(DamageSource damageSource, Gardener gardener)
    { 
        GardenerSuperProjectile proj;
        foreach (var dir in directions)
        {
            proj = Instantiate(gardenerSuperProjPrefab, gardener.transform.position, transform.rotation);
            proj.SetParameters(new DamageInfo(DamageType.Distant, damageSource, 1, dir));
        }
    }
}