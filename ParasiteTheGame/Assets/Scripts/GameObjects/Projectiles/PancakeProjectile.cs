using UnityEngine;

public class PancakeProjectile : AProjectile
{
    public void SetParameters(DamageInfo dmjInf, Vector2 dir)
    {
        SetParameters(dmjInf, dir, 5, 0.2f, 2);
    }
}