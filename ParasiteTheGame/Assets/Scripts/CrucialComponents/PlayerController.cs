using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamagable
{
    private Rigidbody2D thisRigidbody2d;
    private Vector2 input;
    private float velocity = 3.09f;
    public IControlable controlled;
    private int health;

    void Start()
    {
        thisRigidbody2d = GetComponent<Rigidbody2D>();
        health = 100;
    }

    public void HandleUpdate(InputInfo inpInf)
    {
        if (inpInf.JumpoutPressed)
        {
            ActOnJumpout();
        }
        if (controlled is null)
        {
            input = inpInf.Axis;
            thisRigidbody2d.velocity = input * velocity;
        }
        else
        {
            if (inpInf.PickOrDropPressed)
            {
                controlled.ActOnPickOrDrop();
            }

            controlled.ControlledUpdate(inpInf);
            controlled.UpdatePlayerPos(transform);
        }
        
    }

    public void ActOnJumpout()
    {
        if (controlled == null)
        {
            Capture();
        }
        else
        {
            LetItGo();
        }
    }

    private void Capture()
    {
        var t = Physics2D.OverlapPoint(transform.position, Constants.ControllablesLayer);
        if (t)
        {
            controlled = t.gameObject.GetComponent<IControlable>();
            if (!controlled.CanBeCaptured)
            {
                controlled = null;
                return;
            }
            thisRigidbody2d.simulated = false;
            controlled.OnCapture(this);
        }
        else
        {
            Debug.Log("Nope");
        }
    }

    private void LetItGo()
    {
        thisRigidbody2d.simulated = true;
        controlled.OnRelease(this);
        controlled = null;
    }

    public bool TryTakeDamage(DamageInfo dmgInf)
    {
        if (controlled is null && dmgInf.Source != DamageSource.Player)
        {
            health -= dmgInf.Amount;
            Debug.Log($"Player hurt : health = {health}");
            return true;
        }
        return false;
    }
}
