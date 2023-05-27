using UnityEngine;

public class GardenerSuperProjectile: AProjectile
{
    private bool isDead = false;
    private float animDeadTime = 0.16f;
    protected Animator animator;
    
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isRunning", true); //ITSigma - TODO : animation  "isRunning"
    }

    public void SetParameters(DamageInfo dmjInf)
    {
        SetParameters(dmjInf, dmjInf.Direction, 10f, 1f, 0.5f);
    }

    protected override void Update()
    {
        if (isDead)
            PlayLastAnimation();
        transform.position += Time.deltaTime * velocity * direction;
        var obg = Physics2D.Raycast(transform.position, direction, rayLength, LayerConstants.DamageTakersLayer);
        if (obg)
        {
            var damagable = obg.collider.gameObject.GetComponent<IDamagable>();
            //walls can have no scripts and thus can be not a IDamagable instance
            if (damagable is null || damagable.TryTakeDamage(damageInfo))
            {
                animator.SetBool("isAttacking", true); //ITSigma - TODO : animation  "isAttacking"
                isDead = true;
                //Destroy(gameObject);
            }
        }

        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            isDead = true;
            animator.SetBool("isAttacking", true); //ITSigma - TODO : animation  "isAttacking"
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