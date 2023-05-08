using UnityEngine;

public class AProjectile : MonoBehaviour
{
    private DamageInfo damageInfo;
    private float velocity;
    private Vector3 direction;
    private float rayLength;
    private float lifetime;   //seconds

    /// <param name="rayLength">to raycast in front of the projectile</param>
    /// <param name="lifetime">seconds</param>
    public virtual void SetParameters(DamageInfo damageInfo, Vector3 direction, float velocity, float rayLength, float lifetime)
    {
        this.damageInfo = damageInfo;
        this.direction = direction;
        this.velocity = velocity;
        this.rayLength = rayLength;
        this.lifetime = lifetime;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Time.deltaTime * velocity * direction;
        var obg = Physics2D.Raycast(transform.position, direction, rayLength, Constants.DamageTakersLayer);
        if (obg)
        {
            var damagable = obg.collider.gameObject.GetComponent<IDamagable>();
            //walls can have no scripts and thus can be not a IDamagable instance
            if (damagable is null || damagable.TryTakeDamage(damageInfo))
            {
                Destroy(gameObject);
            }
        }

        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
