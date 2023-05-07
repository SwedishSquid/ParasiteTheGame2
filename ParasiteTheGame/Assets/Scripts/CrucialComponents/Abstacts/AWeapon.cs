using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public abstract class AWeapon : MonoBehaviour, IUsable
{
    protected IUser user;
    protected DamageSource damageSource;
    protected int damageAmount = 7;
    protected float fireRate = 0.5f; //seconds between fire
    protected float cooldownLeft = 0;
    protected float throwSpeed = 30;
    protected Rigidbody2D rigidbody2;
    [SerializeField] protected ThrowHandler throwHandlerPrefab;

    protected ThrowComponent throwComponent;
    [SerializeField] protected AudioSource audioSource;


    virtual protected void Start()
    {
        throwComponent = GetComponent<ThrowComponent>();
        throwComponent.enabled = false; //just in case weapon creator forgets
    }

    virtual public void HandleUpdate(InputInfo inpInf)
    {
        HandleMovement(inpInf);
        if (inpInf.FirePressed && cooldownLeft <= 0)
        {
            cooldownLeft = fireRate;
            Fire(inpInf);
        }
        if (cooldownLeft > 0)
        {
            cooldownLeft -= Time.deltaTime;
        }
    }

    protected virtual void HandleMovement(InputInfo inpInf)
    {
        var desiredRotation = inpInf.GetMouseDir();
        var angle = Mathf.Atan2(desiredRotation.y, desiredRotation.x) * (180 / Mathf.PI);
        transform.position = user.GetUserPosition() + (desiredRotation * user.GetUserRadius());
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    virtual protected void Fire(InputInfo inpInf)
    {
        //summon something
    }

    virtual public void OnPickUp(IUser user)
    {
        if (throwComponent.enabled)
        {
            throwComponent.EndThrow();
        }
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
        var userVelocity = user.GetUserVelocity();
        OnDropDown(user);
        throwComponent.StartThrow(inpInf.GetMouseDir(), userVelocity * 0.2f);
    }

    virtual public void DealDamageByThrow(IDamagable damagable)
    {
        damagable.TryTakeDamage(new DamageInfo(DamageType.Melee, damageSource, damageAmount));
    }
}
