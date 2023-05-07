using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AEnemy : MonoBehaviour, IControlable, IDamagable, IUser
{
    protected Rigidbody2D myRigidbody;
    protected float velocity = 10;
    protected IUsable item;
    protected float radius = 1.06f;
    protected float itemPickingRadius = 2f;
    public bool IsCaptured;
    protected int health = 100;
    [SerializeField] protected HealthBar healthBar;

    public virtual bool CanBeCaptured { get; protected set; } = true;

    protected virtual void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        healthBar?.SetMaxHealth(health);
    }

    public virtual void ControlledUpdate(InputInfo inpInf)
    {
        myRigidbody.velocity = inpInf.Axis * velocity;
        if (item != null)
        {
            item.HandleUpdate(inpInf);
            if (inpInf.ThrowItemPressed)
            {
                item.Throw(inpInf);
                item = null;
            }
        }
    }

    public virtual void OnCapture(PlayerController player)
    {
        IsCaptured = true;
    }

    public virtual void OnRelease(PlayerController player)
    {
        myRigidbody.velocity = new Vector2(0, 0);
        IsCaptured = false;
    }

    public virtual void UpdatePlayerPos(Transform playerTransform)
    {
        playerTransform.position = transform.position;
    }

    public virtual bool TryTakeDamage(DamageInfo dmgInf)
    {
        if ((IsCaptured && dmgInf.Source == DamageSource.Enemy)
            || (!IsCaptured && dmgInf.Source == DamageSource.Player)
            || dmgInf.Source == DamageSource.Environment)
        {
            health -= dmgInf.Amount;
            if (health >= 0) healthBar?.SetValue(health);
            Debug.Log($"Enemy hurt : health = {health}");
            return true;
        }
        return false;
    }

    public virtual void ActOnPickOrDrop()
    {
        if (item == null)
        {
            PickUp();
        }
        else
        {
            DropDown();
        }
    }

    protected virtual void PickUp()
    {
        var t = Physics2D.OverlapCircle(transform.position, itemPickingRadius, Constants.PickableItems);
        if (t)
        {
            item = t.gameObject.GetComponent<IUsable>();
            item?.OnPickUp(this);
        }
        else
        {
            Debug.Log("nothing to pick up");
        }
    }

    protected virtual void DropDown()
    {
        item.OnDropDown(this);
        item = null;
    }

    public virtual Vector2 GetUserPosition()
    {
        return transform.position;
    }

    public virtual Vector2 GetUserVelocity()
    {
        return myRigidbody.velocity;
    }

    public virtual float GetUserHeight()
    {
        throw new System.NotImplementedException();
    }
    public virtual float GetUserWidth()
    {
        throw new System.NotImplementedException();
    }
    public virtual float GetUserRadius()
    {
        return radius;
    }

    public virtual DamageSource GetDamageSource()
    {
        if (IsCaptured)
        {
            return DamageSource.Player;
        }
        return DamageSource.Enemy;
    }
}
