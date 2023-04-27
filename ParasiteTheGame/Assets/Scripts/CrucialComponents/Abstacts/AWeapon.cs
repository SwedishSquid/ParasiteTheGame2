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

    protected ThrowComponent throwComponent;


    virtual protected void Start()
    {
        throwComponent = GetComponent<ThrowComponent>();
        throwComponent.enabled = false; //just in case weapon creator forgets
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
