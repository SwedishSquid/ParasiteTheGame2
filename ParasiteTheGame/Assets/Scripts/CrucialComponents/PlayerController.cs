using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
//using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;

public class PlayerController : MonoBehaviour, IDamagable, ISavable, IPlayerInfoPlate, IKillable
{
    private Rigidbody2D thisRigidbody2d;
    private SpriteRenderer thisSpriteRenderer;
    private Vector2 input;
    private float velocity = 3.09f;
    public IControlable controlled;
    private string controlledGUID = "";

    [SerializeField] private PlayerInfoPlate infoPlate;
    private int maxHealth = 10;
    private int health;
    
    [SerializeField] private Animator animator;

    private float jumpVelocity = 20;
    private bool isActJump;
    private float maxJumpTime = 0.3f;
    private float jumpOnTimer;
    private Vector2 jumpDirection;
    private float maxJumpTimeOut = 0.08f;
    private float jumpTimeOut;
    private float maxJumpCooldown = 0.8f;
    private float jumpCooldown;

    //ITSigma
    private bool isPause;
    private Vector2 jumpPauseDirection;
    //

    [SerializeField]private Canvas arrowJumpOn;
    private bool isChooseDirJump;

    [SerializeField] private PlayerHintE hintE;

    [SerializeField] private Sprite grave;

    public bool AlmostPassedOut => false;

    public bool Dead => health <= 0;

    public bool PassedOut => false;

    public Vector2 Position => transform.position;

    public bool CanBeHit => controlled == null;

    void Awake()
    {
        thisRigidbody2d = GetComponent<Rigidbody2D>();
        thisSpriteRenderer = GetComponent<SpriteRenderer>();
        health = maxHealth;
        infoPlate.AwakeData(this);
    }

    void Start()
    {
        arrowJumpOn.transform.position = transform.position;
    }

    public void HandleUpdate(InputInfo inpInf)
    {
        if (Dead) return;

        infoPlate.UpdateData(this);
        if (TryHandleJump(inpInf.JumpoutPressed, inpInf.GetMouseDir()))
            return;
        UpdateAnimation(inpInf);
        
        if (controlled is null)
        {
            input = inpInf.Axis;
            thisRigidbody2d.velocity = input * velocity;
        }
        else
        {
            controlled.UpdatePlayerPos(transform);
            controlled.ControlledUpdate(inpInf);
        }
        
    }

    public bool TryHandleJump(bool jumpoutPressed, Vector2 direction)
    {
        if (jumpCooldown > 0)
        {
            if (jumpoutPressed)
                Debug.Log($"jumpCooldown: {jumpCooldown}");
            jumpCooldown -= Time.deltaTime;
            return false;
        }
        if (isActJump)
        {
            ActJumpOn();
            return true;
        }
        if (isChooseDirJump && !jumpoutPressed)
        {
            ActOnJumpout(direction);
            isChooseDirJump = false;
            return true;
        }
        if (isChooseDirJump)
        {
            ActChooseDirJump(direction);
        }

        isChooseDirJump = jumpoutPressed;

        return false;
    }

    public bool TryDie()
    {
        if (!Dead)
        {
            return false;
        }

        ApplyDeathEffects();

        gameObject.GetComponent<Collider2D>().enabled = false;
        thisRigidbody2d.simulated = false;

        return true;
    }

    protected void ApplyDeathEffects()
    {
        if (grave != null)
        {
            GetComponent<Animator>().enabled = false;
            GetComponent<SpriteRenderer>().sprite = grave;
        }
    }

    private void UpdateAnimation(InputInfo inpInf)
    {
        if (controlled is null && (inpInf.Axis.x != 0 || inpInf.Axis.y != 0))
        {
            animator.SetFloat("moveX", inpInf.Axis.x);
            animator.SetFloat("moveY", inpInf.Axis.y);
            animator.SetBool("isMoving", true);   
        }
        else
            animator.SetBool("isMoving", false);
    }

    private void ActOnJumpout(Vector3 direction)
    {
        if (controlled != null)
        {
            LetItGo();
            jumpTimeOut = maxJumpTimeOut;
        }

        jumpDirection = direction;
        StartJumpOn();
    }
    
    private void StartJumpOn()
    {
        arrowJumpOn.gameObject.SetActive(false);
        isActJump = true;
        thisRigidbody2d.velocity += jumpVelocity * jumpDirection;
        jumpOnTimer = maxJumpTime;
    }

    private void ActJumpOn()
    {
        jumpOnTimer -= Time.deltaTime;
        if (TryCapture() || jumpOnTimer <= 0)
        {
            isActJump = false;
            jumpCooldown = maxJumpCooldown;
            thisRigidbody2d.velocity = Vector2.zero;
        }
    }

    private bool TryCapture()
    {
        if (jumpTimeOut > 0)
        {
            jumpTimeOut -= Time.deltaTime;
            return false;
        }
        var t = Physics2D.Raycast(transform.position, jumpDirection, 0.2f,
            LayerConstants.ControllablesLayer);
        if (t)
        {
            controlled = t.collider.gameObject.GetComponent<IControlable>();
            if (!controlled.CanBeCaptured)
            {
                controlled = null;
                return false;
            }
            thisRigidbody2d.simulated = false;
            thisSpriteRenderer.enabled = false;
            controlled.OnCapture(this);

            if (t.collider.gameObject.TryGetComponent<ISavable>(out var savable))
            {
                controlledGUID = savable.GetGUID();
            }
            
            return true;
        }
        
        return false;
    }

    private void ActChooseDirJump(Vector3 direction)
    {
        if (!arrowJumpOn.gameObject.activeSelf)
            arrowJumpOn.gameObject.SetActive(true);
        var angle = Mathf.Atan2(direction.y, direction.x) * (180 / Mathf.PI);
        arrowJumpOn.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void LetItGo()
    {
        thisRigidbody2d.simulated = true;
        thisSpriteRenderer.enabled = true;
        controlled.OnRelease(this);
        controlled = null;
        controlledGUID = "";
    }

    public bool TryTakeDamage(DamageInfo dmgInf)
    {
        if (controlled is null && dmgInf.Source != DamageSource.Player)
        {
            health -= dmgInf.Amount;
            Debug.Log($"Player hurt : health = {health}");
            TryDie();
            return true;
        }
        return false;
    }

    public void SaveGame(GameData gameData)
    {
        gameData.PlayerInfo.IsInitialised = true;
        gameData.PlayerInfo.Health = health;
        gameData.PlayerInfo.ControlledGUID = controlledGUID;
        gameData.SetPlayerPosition(transform.position, SceneManager.GetActiveScene().name);
    }

    public void LoadData(GameData gameData)
    {
        if (!gameData.PlayerInfo.IsInitialised)
        {
            return;
        }
        health = gameData.PlayerInfo.Health;

        controlledGUID = gameData.PlayerInfo.ControlledGUID;

        var level = gameData.GetLevel(SceneManager.GetActiveScene().name);

        if (level.IsPlayerPosInitialised)
        {
            transform.position = level.PlayerPosition;
        }

        TryDie();
    }

    public string GetGUID()
    {
        //not very useful in player script
        //much more useful for Enemies and Items
        return "the-only-one-player-olala";
    }

    public void AfterAllObjectsLoaded(GameData gameData)
    {
        if (controlledGUID == "")
        {
            return;
        }

        if (!gameData.Enemies.ContainsKey(controlledGUID))
        {
            Debug.LogError($"cannot control enemy with GUID={controlledGUID} - no such enemy found in gameData");
            return;
        }

        var enemyData = gameData.Enemies[controlledGUID];

        controlled = enemyData.thisEnemy;

        (controlled as ISavable)?.SetPosition(transform.position);

        thisRigidbody2d.simulated = false;
        thisSpriteRenderer.enabled = false;

        Debug.Log($"controlled = {enemyData.thisEnemy}");

        controlled.OnCapture(this);
    }

    public void SetGUID(string GUID)
    {
        Debug.LogError("should never be called setGUID on the player - it is player!");
    }

    public void DestroyIt()
    {
        //nope :C
    }

    public void SetPosition(Vector2 position)
    {
        //no no no
        Debug.LogError("why is this called - SetPosition has no effect on player");
    }

    public int GetMaxHealth() => maxHealth;

    public int GetHealth() => health;

    public void ShowHintE()
    {
        hintE.ShowHint();
    }
}
