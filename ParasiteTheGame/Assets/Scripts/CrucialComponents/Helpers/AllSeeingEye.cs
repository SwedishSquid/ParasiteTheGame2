using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllSeeingEye : MonoBehaviour
{
    public float Radius { get; set; } = 9;

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
        return p.distance;
    }
}
