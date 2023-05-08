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


    protected virtual void Start()
    {
        throwComponent = GetComponent<ThrowComponent>();
        throwComponent.enabled = false; //just in case weapon creator forgets
    }

    public virtual void HandleUpdate(InputInfo inpInf)
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

    protected virtual void Fire(InputInfo inpInf)
    {
        //summon something
    }

    public virtual void OnPickUp(IUser user)
    {
        if (throwComponent.enabled)
        {
            throwComponent.EndThrow();
        }
        this.user = user;
        damageSource = user.GetDamageSource();
        gameObject.GetComponent<Collider2D>().enabled = false;
    }

    public virtual void OnDropDown(IUser user)
    {
        this.user = null;
        gameObject.GetComponent<Collider2D>().enabled = true;
    }

    public virtual void Throw(InputInfo inpInf)
    {
        var userVelocity = user.GetUserVelocity();
        OnDropDown(user);
        throwComponent.StartThrow(inpInf.GetMouseDir(), userVelocity * 0.2f);
    }

    public virtual void DealDamageByThrow(IDamagable damagable)
    {
        damagable.TryTakeDamage(new DamageInfo(DamageType.Melee, damageSource, damageAmount));
    }
}
