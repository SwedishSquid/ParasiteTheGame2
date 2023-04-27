using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public abstract class AWeapon : MonoBehaviour, IUsable
{
    protected IUser user;
    protected DamageSource damageSource;
    protected int damageAmount = 7;
    protected int fireRate = 20; //frames per firing
    protected int frameCount = 0;
    protected float throwSpeed = 30;
    protected Rigidbody2D rigidbody2;
    protected Vector3 forward = new Vector3(0, 0, 1);
    [SerializeField] protected ThrowHandler throwHandlerPrefab;


    virtual protected void Start()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
    }

    virtual public void HandleUpdate(InputInfo inpInf)
    {
        var desiredRotation = inpInf.GetMouseDir();
        var angle = Mathf.Atan2(desiredRotation.y, desiredRotation.x) * (180 / Mathf.PI);
        transform.position = user.GetUserPosition() + (desiredRotation * user.GetUserRadius());
        transform.rotation = Quaternion.AngleAxis(angle, forward);
        frameCount++;
        if (inpInf.FirePressed && frameCount >= fireRate)
        {
            frameCount = 0;
            Fire(inpInf);
        }
    }

    virtual protected void Fire(InputInfo inpInf)
    {
        //summon something
    }

    virtual public void OnPickUp(IUser user)
    {
        this.user = user;
        damageSource = user.GetDamageSource();
        gameObject.GetComponent<Collider2D>().enabled = false;
    }

    virtual public void OnDropDown(IUser user)
    {
        this.user = null;
        gameObject.GetComponent<Collider2D>().enabled = true;
    }

    virtual public void Throw(InputInfo inpInf)
    {
        OnDropDown(user);
        var rigid = gameObject.AddComponent<Rigidbody2D>();
        rigid.gravityScale = 0;
        rigid.simulated = true;
        rigid.velocity = inpInf.GetMouseDir() * throwSpeed;
        gameObject.layer = LayerMask.NameToLayer("CollidableItems"); ;
        var th = Instantiate(throwHandlerPrefab, transform.position, transform.rotation);
        th.InitIt(gameObject, 0.997f);
        /*if (this.TryGetComponent<Rigidbody2D>(out var rigid))
        {
            rigid.simulated = true;
            rigid.velocity = inpInf.GetMouseDir() * throwSpeed;
            var th = Instantiate(throwHandlerPrefab, transform.position, transform.rotation);
            th.InitIt(gameObject);
        }*/
    }

    virtual protected void OnCollisionEnter(Collision collision)
    {
        if (rigidbody2.simulated && collision.gameObject.TryGetComponent<IDamagable>(out var damagabe))
        {
            damagabe.TryTakeDamage(new DamageInfo(DamageType.Melee, damageSource, damageAmount));
        }
    }
}
