using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThrowComponent : MonoBehaviour
{
    private float initialSpeed;

    private Queue<float> speedQueue;
    private int speedQueueDepth = 3;

    protected float timeLeft = 2f; //seconds
    protected Rigidbody2D rigidbody2d;
    
    protected virtual void FixedUpdate()
    {
        rigidbody2d.velocity *= GetSlowdownFactor(timeLeft);

        speedQueue.Dequeue();
        speedQueue.Enqueue(rigidbody2d.velocity.magnitude);
    }

    protected virtual void LateUpdate()
    {
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
    public virtual void StartThrow(Vector2 direction, Vector2 additionalVelocity = new Vector2(), float lifetime = 1,
        float speed = 18f, bool rotateRandomly = true, float rotationSpeed = 340)
    {
        timeLeft = lifetime;
        initialSpeed= speed * direction.magnitude;
        InitiateSpeedQueue(speed);

        gameObject.layer = LayerConstants.CollidableItemsLayer;

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

        gameObject.layer = LayerConstants.ItemsLayer; 
    }

    protected virtual float GetSlowdownFactor(float timeLeft)
    {
        float factor = 500f;
        return (timeLeft * factor) / (1 + factor * timeLeft);
    }

    public virtual float GetDamageMultiplyer()
    {
        return speedQueue.Peek() / initialSpeed;
    }

    private void InitiateSpeedQueue(float speed)
    {
        if (speedQueue is null)
        {
            speedQueue = new Queue<float>();
        }
        for (int i = 0; i < speedQueueDepth; i++)
        {
            speedQueue.Enqueue(speed);
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<IDamagable>(out var damagable))
        {
            gameObject.GetComponent<IUsable>().DealDamageByThrow(damagable);
        }
    }
}
