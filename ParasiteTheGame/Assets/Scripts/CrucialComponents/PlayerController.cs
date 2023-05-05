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
    
    private const float jumpVelocity = 20f;
    private bool isActJumpOn;
    private const int maxJumpOnTime = 100;
    private int jumpOnTimer;
    private Vector3 jumpOnDirection;

    [SerializeField]private Canvas arrowJumpOn;
    private bool isChooseDirJumpOn;
    

    void Start()
    {
        arrowJumpOn.transform.position = transform.position;
        thisRigidbody2d = GetComponent<Rigidbody2D>();
        health = 100;
    }

    public void HandleUpdate(InputInfo inpInf)
    {
        if (isActJumpOn)
        {
            ActJumpOn();
            return;
        }
        if (inpInf.JumpoutPressed)
        {
            ActOnJumpout(inpInf);
            return;
        }
        if (controlled is null)
        {
            if (inpInf.ChooseDirJumpOnPressed)
            {
                ActChooseDirJumpOn(inpInf);
                return;
            }
            if (arrowJumpOn.gameObject.activeSelf)
                arrowJumpOn.gameObject.SetActive(false);
            
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

    public void ActOnJumpout(InputInfo inpInf)
    {
        if (controlled == null)
        {
            if (inpInf.ChooseDirJumpOnPressed)
                StartJumpOn(inpInf);
        }
        else
        {
            LetItGo();
        }
    }
    
    private void StartJumpOn(InputInfo inpInf)
    {
        arrowJumpOn.gameObject.SetActive(false);
        jumpOnDirection = inpInf.GetMouseDir();
        isActJumpOn = true;
        jumpOnTimer = maxJumpOnTime;
    }

    private void ActJumpOn()
    {
        thisRigidbody2d.velocity = jumpOnDirection * jumpVelocity;
        jumpOnTimer--;
        if (TryCapture() || jumpOnTimer == 0)
        {
            isActJumpOn = false;
            thisRigidbody2d.velocity = Vector2.zero;
        }
    }

    private bool TryCapture()
    {
        var t = Physics2D.Raycast(transform.position, jumpOnDirection, 0.2f,
            Constants.ControllablesLayer);
        if (t)
        {
            controlled = t.collider.gameObject.GetComponent<IControlable>();
            if (!controlled.CanBeCaptured)
            {
                controlled = null;
                return false;
            }
            thisRigidbody2d.simulated = false;
            controlled.OnCapture(this);
            return true;
        }
        
        return false;
    }

    private void ActChooseDirJumpOn(InputInfo inpInf)
    {
        if (!arrowJumpOn.gameObject.activeSelf)
            arrowJumpOn.gameObject.SetActive(true);
        var desiredRotation = inpInf.GetMouseDir();
        var angle = Mathf.Atan2(desiredRotation.y, desiredRotation.x) * (180 / Mathf.PI);
        arrowJumpOn.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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
