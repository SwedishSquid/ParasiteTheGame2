using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThrowComponent : MonoBehaviour
{
    protected float timeLeft = 2f; //seconds
    protected Rigidbody2D rigidbody2d;
    
    protected virtual void Update()
    {
        rigidbody2d.velocity *= GetSlowdownFactor(timeLeft);
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            EndThrow();
        }
    }

    /// <summary>
    /// warning! this method transports object to layer "CollidableItems"
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="speed"></param>
    ///  <param name="rotationSpeed"> Degrees per second </param>
    public virtual void StartThrow(Vector2 direction, Vector2 additionalVelocity = new Vector2(), float lifetime = 2,
        float speed = 8f, bool rotateRandomly = true, float rotationSpeed = 40)
    {
        timeLeft = lifetime;

        gameObject.layer = Constants.CollidableItemsLayer;

        rigidbody2d = gameObject.AddComponent<Rigidbody2D>();
        rigidbody2d.velocity = direction * speed + additionalVelocity;
        rigidbody2d.angularVelocity = rotateRandomly ? (2 * Random.value - 1) * rotationSpeed : rotationSpeed;
        rigidbody2d.gravityScale = 0;
        
        enabled = true;
    }

    /// <summary>
    /// warning! this method transports object to layer "Weapons"
    /// </summary>
    public virtual void EndThrow()
    {
        enabled = false;
        
        Destroy(rigidbody2d); rigidbody2d = null;

        gameObject.layer = Constants.ItemsLayer; 
    }

    protected virtual float GetSlowdownFactor(float timeLeft)
    {
        float factor = 500f;
        return (timeLeft * factor) / (1 + factor * timeLeft);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<IDamagable>(out var damagable))
        {
            gameObject.GetComponent<IUsable>().DealDamageByThrow(damagable);
        }
    }
}
