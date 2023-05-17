using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UIElements;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamagable, ISavable
{
    private Rigidbody2D thisRigidbody2d;
    private SpriteRenderer thisSpriteRenderer;
    private Vector2 input;
    private float velocity = 3.09f;
    public IControlable controlled;
    private string controlledGUID = "";
    
    private int health;
    [SerializeField] private HealthBar healthBar;
    
    private float jumpVelocity = 20;
    private bool isActJump;
    private float maxJumpTime = 0.3f;
    private float jumpOnTimer;
    private Vector2 jumpDirection;
    private float maxJumpTimeOut = 0.08f;
    private float jumpTimeOut;
    private float maxJumpCooldown = 0.8f;
    private float jumpCooldown;

    [SerializeField]private Canvas arrowJumpOn;
    private bool isChooseDirJump;
    
    void Awake()
    {
        thisRigidbody2d = GetComponent<Rigidbody2D>();
        thisSpriteRenderer = GetComponent<SpriteRenderer>();
        health = 100;
    }

    void Start()
    {
        arrowJumpOn.transform.position = transform.position;
    }

    public void HandleUpdate(InputInfo inpInf)
    {
        if (TryHandleJump(inpInf))
            return;
        
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

    private bool TryHandleJump(InputInfo inpInf)
    {
        if (jumpCooldown > 0)
        {
            if (inpInf.JumpoutPressed)
                Debug.Log($"jumpCooldown: {jumpCooldown}");
            jumpCooldown -= Time.deltaTime;
            return false;
        }
        if (isActJump)
        {
            ActJumpOn();
            return true;
        }
        if (isChooseDirJump && !inpInf.JumpoutPressed)
        {
            ActOnJumpout(inpInf.GetMouseDir());
            isChooseDirJump = false;
            return true;
        }
        if (isChooseDirJump)
        {
            ActChooseDirJump(inpInf.GetMouseDir());
            return controlled is null;
        }
        if (inpInf.JumpoutPressed)
        {
            thisRigidbody2d.velocity = Vector2.zero;
            isChooseDirJump = true;
            return true;
        }

        return false;
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
            return true;
        }
        return false;
    }

    public void SaveGame(GameData gameData)
    {
        gameData.PlayerInfo.IsInitialised = true;
        gameData.PlayerInfo.Health = health;
        gameData.PlayerInfo.ControlledGUID = controlledGUID;
        gameData.GetLevel(gameData.CurrentLevelName).PlayerPosition = transform.position;
    }

    public void LoadData(GameData gameData)
    {
        if (!gameData.PlayerInfo.IsInitialised)
        {
            return;
        }
        health = gameData.PlayerInfo.Health;
        controlledGUID = gameData.PlayerInfo.ControlledGUID;
        transform.position = gameData.GetLevel(gameData.CurrentLevelName).PlayerPosition;
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
}
