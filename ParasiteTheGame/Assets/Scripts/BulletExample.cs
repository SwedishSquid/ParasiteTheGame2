using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletExample : MonoBehaviour
{
    private DamageInfo damageInfo;
    private float velocity;
    private Vector3 direction;
    private float rayLength;
    private int lifetime;
    private LayerMask damageTakersLayer;

    private void Start()
    {
        damageTakersLayer = LayerMask.GetMask("Player") | LayerMask.GetMask("Controllables") | LayerMask.GetMask("Walls");
    }

    public void SetParameters(DamageInfo damageInfo, Vector3 direction, float velocity, float rayLenght, int lifetime)
    {
        this.damageInfo = damageInfo;
        this.velocity = velocity;
        this.rayLength = rayLenght;
        //should we normalize it? i mean it probably will be normalized already
        this.direction = direction.normalized;
        this.lifetime = lifetime;
        //this one rotates in weapon
        //transform.rotation = new Quaternion(direction.x, direction.y, 0, 0);
    }

    void Update()
    {
        transform.position += direction * velocity * Time.deltaTime;
        var obg = Physics2D.Raycast(transform.position, direction, rayLength, damageTakersLayer);
        if (obg)
        {
            var idmg = obg.collider.gameObject.GetComponent<IDamagable>();
            //walls can have no scripts and thus can be not a IDamagable instance
            if (idmg is null || idmg.TryTakeDamage(damageInfo))
            {
                Destroy(gameObject);
            }
        }
        lifetime--;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
