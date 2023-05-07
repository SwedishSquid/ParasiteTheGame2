using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleProjectile : MonoBehaviour
{
    protected bool isHeld = true;
    protected float timeLeft = 1f;

    protected void Update()
    {
        if (isHeld)
        {
            return;
        }
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public void SetParameters(float speed, float lifetime = 1f, bool isHeld = true)
    {

    }

    public void OnThrow()
    {
        isHeld = false;
    }

    public void ChangePosition(Vector2 position, Quaternion rotation = new Quaternion())
    {
        transform.position= position;
        transform.rotation= rotation;
    }
}
