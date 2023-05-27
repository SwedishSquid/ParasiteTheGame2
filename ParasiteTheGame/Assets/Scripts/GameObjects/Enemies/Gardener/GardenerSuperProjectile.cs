using UnityEngine;

public class GardenerSuperProjectile: AProjectile
{
    private bool isDead = false;
    private float animDeadTime = 0.7f;
    protected Animator animator;
    
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetParameters(DamageInfo dmjInf)
    {
        animator.SetBool("isRunning", true); 
        SetParameters(dmjInf, dmjInf.Direction, 7f, 1f, 1f);
    }

    protected override void Update()
    {
        if (isDead)
        {
            PlayLastAnimation();
            return;
        }
        animator.SetFloat("moveX", damageInfo.Direction.x);
        animator.SetFloat("moveY", damageInfo.Direction.y);
        transform.position += Time.deltaTime * velocity * direction;
        var obg = Physics2D.Raycast(transform.position, direction, rayLength, LayerConstants.DamageTakersLayer);
        if (obg)
        {
            var damagable = obg.collider.gameObject.GetComponent<IDamagable>();
            //walls can have no scripts and thus can be not a IDamagable instance
            if (damagable is null || damagable.TryTakeDamage(damageInfo))
            {
                animator.SetBool("isAttacking", true); 
                isDead = true;
                //Destroy(gameObject);
            }
        }

        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            isDead = true;
            animator.SetBool("isAttacking", true); 
            //Destroy(gameObject);
        }
    }

    protected void PlayLastAnimation()
    {
        if (animDeadTime <= 0)
        {
            Destroy(gameObject);
        }
        animDeadTime -= Time.deltaTime;
    }
}