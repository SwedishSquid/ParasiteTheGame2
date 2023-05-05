using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThrowComponent : MonoBehaviour
{
    protected int ticksLeft = 1000;
    protected Rigidbody2D rigidbody2d;
    
    protected virtual void Update()
    {
        rigidbody2d.velocity *= GetSlowdownFactor(ticksLeft);
        ticksLeft--;
        if (ticksLeft <= 0)
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
    public virtual void StartThrow(Vector2 direction, Vector2 additionalVelocity = new Vector2(), int lifetime = 1000,
        float speed = 8f, bool rotateRandomly = true, float rotationSpeed = 40)
    {
        ticksLeft = lifetime;

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

    protected virtual float GetSlowdownFactor(int timeLeft)
    {
        float factor = 1f;
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
