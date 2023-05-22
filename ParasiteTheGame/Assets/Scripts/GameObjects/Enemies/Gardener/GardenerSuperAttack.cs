using UnityEngine;
using System;
using System.Linq;

public class GardenerSuperAttack : MonoBehaviour
{
    [SerializeField] protected GardenerSuperProjectile gardenerSuperProjPrefab; 

    private float maxAttackCooldown = 0.8f; //ITSigma - TODO : Change value
    private float attackCooldown;
    private static int countProj = 3;
    private DamageSource damageSource;
    private static Vector3[] directions;

    protected virtual void Awake()
    {
        if (directions is not null)
            return;
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
                        new Vector3(s1 * (float)x.Item1, s2 * (float)x.Item2, 0))))
            .ToArray();
    }

    public void Attack(DamageSource damageSource)
    { 
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
            return;
        }
        attackCooldown = maxAttackCooldown;
        GardenerSuperProjectile proj;
        foreach (var dir in directions)
        {
            proj = Instantiate(gardenerSuperProjPrefab, transform.position + dir, transform.rotation);
            proj.SetParameters(new DamageInfo(DamageType.Distant, damageSource, 1, dir));
        }
    }
}