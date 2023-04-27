using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExample : MonoBehaviour, IControlable, IDamagable
{
    private Rigidbody2D myRigidbody;
    private float velocity = 10;
    private IWeapon weapon;
    private LayerMask weaponLayer;
    private float radius = 1.06f;
    public bool IsCaptured;
    private int health;

    public bool CanBeCaptured { get; protected set; } = true;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        weaponLayer = LayerMask.GetMask("Weapons");
    }

    public void Update()
    {

    }

    public void ControlledUpdate(InputInfo inpInf)
    {
        myRigidbody.velocity = inpInf.Axis * velocity;
        if (weapon != null)
        {
            weapon.RotateAndMove(inpInf.GetMouseDir(), transform.position, radius);
            if (inpInf.FirePressed)
            {
                weapon.Use();
            }
        }
    }

    public void UpdatePlayerPos(Transform playerTransform)
    {
        playerTransform.position = transform.position;
    }

    //do we need this?
    public void AutomaticUpdate()
    {
        throw new System.NotImplementedException();
    }

    public void OnCapture(PlayerController player)
    {
        //just to see if working
        Debug.Log("Captured");
        IsCaptured = true;
    }

    public void OnRelease(PlayerController player)
    {
        //just to see if working
        myRigidbody.velocity = new Vector2(0, 0);
        Debug.Log("Released");
        IsCaptured = false;
    }

    public void ActOnPickOrDrop()
    {
        if (weapon == null)
        {
            PickUp();
        }
        else
        {
            DropDown();
        }
    }

    private void PickUp()
    {
        var t = Physics2D.OverlapCircle(transform.position, radius, weaponLayer);
        if (t)
        {
            Debug.Log("picking up");
            weapon = t.gameObject.GetComponent<IWeapon>();
            if (weapon is not null)
            {
                weapon.OnPickUp(this);
            }
        }
        else
        {
            Debug.Log("nothing to pick up");
        }
    }

    private void DropDown()
    {
        weapon.OnDropDown(this);
        weapon = null;
    }

    public bool TryTakeDamage(DamageInfo dmgInf)
    {
        if ((IsCaptured && dmgInf.Source == DamageSource.Enemy) 
            ||(!IsCaptured && dmgInf.Source == DamageSource.Player))
        {
            health -= dmgInf.Amount;
            Debug.Log($"Enemy hurt : health = {health}");
            return true;
        }
        return false;
    }
}
