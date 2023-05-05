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
    private bool isActJump;
    private const int maxJumpTime = 100;
    private int jumpOnTimer;
    private Vector3 jumpDirection;
    private int jumpTimeOut;

    [SerializeField]private Canvas arrowJumpOn;
    private bool isChooseDirJump;
    

    void Start()
    {
        arrowJumpOn.transform.position = transform.position;
        thisRigidbody2d = GetComponent<Rigidbody2D>();
        health = 100;
    }

    public void HandleUpdate(InputInfo inpInf)
    {
        if (isActJump)
        {
            ActJumpOn();
            return;
        }
        if (isChooseDirJump && !inpInf.JumpoutPressed)
        {
            ActOnJumpout(inpInf);
            isChooseDirJump = false;
        }
        if (isChooseDirJump)
        {
            ActChooseDirJump(inpInf);
            return;
        }
        if (inpInf.JumpoutPressed)
        {
            isChooseDirJump = true;
            return;
        }
        if (controlled is null)
        {
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
            jumpTimeOut = 0;
            StartJumpOn(inpInf);
        }
        else
        {
            LetItGo();
            jumpTimeOut = 20;
            StartJumpOn(inpInf);
        }
    }
    
    private void StartJumpOn(InputInfo inpInf)
    {
        arrowJumpOn.gameObject.SetActive(false);
        jumpDirection = inpInf.GetMouseDir();
        isActJump = true;
        jumpOnTimer = maxJumpTime;
    }

    private void ActJumpOn()
    {
        thisRigidbody2d.velocity = jumpDirection * jumpVelocity;
        jumpOnTimer--;
        if (TryCapture() || jumpOnTimer == 0)
        {
            isActJump = false;
            thisRigidbody2d.velocity = Vector2.zero;
        }
    }

    private bool TryCapture()
    {
        if (jumpTimeOut > 0)
        {
            jumpTimeOut--;
            return false;
        }
        var t = Physics2D.Raycast(transform.position, jumpDirection, 0.2f,
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

    private void ActChooseDirJump(InputInfo inpInf)
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
