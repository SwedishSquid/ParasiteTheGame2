using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllSeeingEye : MonoBehaviour
{
    public float Radius { get; set; } = 12;

    public float RayAmount = 16;

    private void Start()
    {

    }

    public IEnumerable<GameObject> GetAllObjectsInView(LayerMask mask)
    {
        var results = Physics2D.OverlapCircleAll(transform.position, Radius)
            .Select(c => c.gameObject);
        return results;
    }

    public float DistanceToObstacleByRay(Vector2 direction, LayerMask mask)
    {
        var p = Physics2D.Raycast(transform.position, direction, Radius * 2, mask);
        if (p)
        {
            return p.distance;
        }
        else
        {
            return Radius;
        }
        
    }

    public Vector2 GetFreeDirection(float inaccuracy = 1)
    {
        var bestDirection = Vector2.left;
        var bestDistance = 0f;
        var anglePart = 360f / RayAmount;

        for (int x = 0; x < RayAmount; x++)
        {
            var dir = new Vector2(Mathf.Sin(anglePart * x), Mathf.Cos(anglePart * x));
            var dist = DistanceToObstacleByRay(dir, LayerConstants.ObstaclesLayer);
            if (dist > 0 && Random.value * inaccuracy > bestDistance / dist)
            {
                bestDirection = dir;
                bestDistance = dist;
            }
        }

        return bestDirection.normalized;
    }
}
