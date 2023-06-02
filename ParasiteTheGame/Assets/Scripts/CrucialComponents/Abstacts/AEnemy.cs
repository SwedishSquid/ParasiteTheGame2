using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
//using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
//using UnityEditor.Sprites;
using UnityEngine;

public abstract class AEnemy : MonoBehaviour, IControlable, IDamagable, IUser, ISavable, IEnemyInfoPlate, IKillable
{
    [SerializeField] protected string id;

    [ContextMenu("Generate GUID for id")]
    protected void GenerateGUID()
    {
        id = System.Guid.NewGuid().ToString();
    }

    
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D myRigidbody;
    protected float velocity = 10;
    
    protected IUsable item;
    protected string itemGUID = "";

    protected float radius = 1.06f;
    protected float itemPickingRadius = 2f;
    public bool IsCaptured;
    public PlayerController Capturer;

    public int MaxHealth { get; set; } = 30;
    protected int terminalHealth = 30 / 2;
    public int Health { get; set; } = 30;
    public virtual bool AlmostPassedOut => !PassedOut && (Health - terminalHealth) < MaxHealth / 5;

    protected DamageSource damageSource = DamageSource.Enemy;

    protected Vector2 damageDir;
    protected float freezeVelocity = 3;
    protected float maxFreezeTime = OtherConstants.CommonMaxFreezeTime;
    protected float maxImmunityTime = OtherConstants.CommonImmunityTime;
    protected float freezeTime;
    protected float immunityTime;

    protected bool immunityMomentForThrowingActive = false;
    protected float ThrowMomentDuration = 0.05f;

    [SerializeField]protected BaseAttack baseAttack;
    
    private Color hurtColor = new (1, 0.6f, 0.6f, 1);
    [SerializeField] private AudioSource hurt;

    protected AIntelligence intelligence;

    public virtual bool CanBeCaptured { get
        {
            return !Dead;
        } }

    public virtual bool PassedOut => Health < terminalHealth;
    public virtual bool Dead => Health <= 0;

    public Vector2 Position => transform.position;

    public bool CanBeHit => IsCaptured;

    public DamageType GetDamageType()
    {
        if (HaveItem)
        {
            return item.GetDamageType();
        }
        return DamageType.Melee;
    }

    protected virtual void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        intelligence = GetComponent<AIntelligence>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //GetGUID(); //to check if has any
    }

    protected virtual void Start()
    {
        if (id == null || id == "")
        {
            Debug.LogError($"To enable saving system operations on this object" +
                $" ({gameObject}) GUID must be generated");
        }
    }

    public virtual void ControlledUpdate(InputInfo inpInf)
    {
        if (freezeTime > 0)
        {
            freezeTime -= Time.deltaTime;
            inpInf = inpInf.ConstructForFrozen();
        }else
        {
            myRigidbody.velocity = inpInf.Axis * velocity;
        }

        if (immunityTime >= 0)
        {
            immunityTime -= Time.deltaTime;
        }

        if (inpInf.PickOrDropPressed)
        {
            ActOnPickOrDrop();
        }

        if (item != null)
        {
            item.HandleUpdate(inpInf);
            if (inpInf.ThrowItemPressed)
            {
                PerformThrow(inpInf);
            }
        }
        else if (inpInf.ThrowItemPressed)
        {
            ActOnPickOrDrop();
        }
        else if (baseAttack != null)
            baseAttack.HandleUpdate(inpInf);
    }

    protected virtual void PerformThrow(InputInfo inpInf)
    {
        immunityMomentForThrowingActive = true;
        item.Throw(inpInf);
        item = null;
        itemGUID = "";
        Invoke(nameof(RestoreImmunityMoment), ThrowMomentDuration);
    }

    private void RestoreImmunityMoment()
    {
        immunityMomentForThrowingActive = false;
    }

    public virtual bool TryPassOut()
    {
        if (!PassedOut || Dead)
        {
            return false;
        }

        if (item != null && !IsCaptured)
        {
            DropDown();
        }

        return true;
    }

    public bool TryDie()
    {
        if (!Dead)
        {
            return false;
        }

        ApplyDeathEffects();

        gameObject.GetComponent<Collider2D>().enabled= false;
        myRigidbody.simulated = false;

        return true;
    }

    protected virtual void ApplyDeathEffects()
    {
        if (item != null)
        {
            DropDown();
        }

        intelligence.TryTurnOff();
        if (IsCaptured)
        {
            IsCaptured = false;
            Capturer.TryHandleJump(true, new Vector2(Random.value, Random.value).normalized);
        }
    }

    public virtual void OnCapture(PlayerController player)
    {
        IsCaptured = true;
        Capturer = player;
        damageSource = DamageSource.Player;
        intelligence.enabled = false;
    }

    public virtual void OnRelease(PlayerController player)
    {
        myRigidbody.velocity = new Vector2(0, 0);
        IsCaptured = false;
        damageSource = DamageSource.Enemy;
        TryPassOut();
        if (!TryDie())
        {
            intelligence.enabled = true;
            intelligence.OnRelease();
        }
    }

    public virtual void UpdatePlayerPos(Transform playerTransform)
    {
        playerTransform.position = transform.position;
    }

    public virtual bool TryTakeDamage(DamageInfo dmgInf)
    {
        //Debug.Log(immunityTime);

        if ((IsCaptured && dmgInf.Source == DamageSource.Enemy)
            || (!IsCaptured && dmgInf.Source == DamageSource.Player)
            || dmgInf.Source == DamageSource.Environment)
        {
            if (immunityMomentForThrowingActive && dmgInf.Source == DamageSource.Environment)
            {
                return false;
            }

            if (immunityTime > 0)
            {
                return true;
            }
            Health -= dmgInf.Amount;
            hurt.Play();
            GetDamageEffect(dmgInf);
            StartCoroutine(RedSprite());
            
            Debug.Log($"Creature hurt : health = {Health}");

            TryPassOut();
            TryDie();
            return true;
        }
        return false;
    }

    public void GetDamageEffect(DamageInfo dmgInf)
    {
        freezeTime = dmgInf.FreezeTime;
        immunityTime = maxImmunityTime;
        myRigidbody.velocity += dmgInf.Direction * (freezeVelocity * dmgInf.DamageVelocityMultiplier);
    }

    protected IEnumerator RedSprite()
    {
        spriteRenderer.color = hurtColor;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
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
        //ITSigma - Pause
        if (PauseController.gameIsPaused)
            return;
        //

        var t = Physics2D.OverlapCircle(transform.position, itemPickingRadius, LayerConstants.PickableItems);
        if (t)
        {
            item = t.gameObject.GetComponent<IUsable>();
            item?.OnPickUp(this);
            if (t.gameObject.TryGetComponent<ISavable>(out var savable))
            {
                itemGUID = savable.GetGUID();
            }
        }
        else
        {
            //Debug.Log("nothing to pick up");
        }
    }

    protected virtual void DropDown()
    {
        //ITSigma - Pause
        if (PauseController.gameIsPaused)
            return;
        //

        item.OnDropDown(this);
        item = null;
        itemGUID = "";
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
        return damageSource;
    }

    public virtual void SaveGame(GameData gameData)
    {
        var enemyData = gameData.GetEnemyToSave(id);

        enemyData.EnemyPosition = transform.position;
        enemyData.CanBeCaptured = CanBeCaptured;
        enemyData.PickedItemGUID = itemGUID;
        enemyData.Health = Health;

        enemyData.TypeName = this.GetType().Name;//typeName;
    }

    public virtual void LoadData(GameData gameData)
    {
        if (gameData.Enemies.ContainsKey(id))
        {
            var enemyData = gameData.Enemies[id];
            transform.position = enemyData.EnemyPosition;
            //CanBeCaptured = enemyData.CanBeCaptured;
            itemGUID = enemyData.PickedItemGUID;
            Health = enemyData.Health;

            Debug.Log("load something to enemy");

            enemyData.thisEnemy = this;
        }
        AfterDataLoaded();
        TryPassOut();
        TryDie();
    }

    protected virtual void AfterDataLoaded()
    {

    }

    public string GetGUID()
    {
        if (id == "")
        {
            Debug.LogError($"GUID for {this} is not set");
        }
        return id;
    }

    public void AfterAllObjectsLoaded(GameData gameData)
    {
        if (itemGUID == "")
        {
            return;
        }

        if (!gameData.Items.ContainsKey(itemGUID))
        {
            Debug.LogError($"cannot pick up item with GUID {itemGUID} - no such item found");
            return;
        }

        item = gameData.Items[itemGUID].thisItem;

        item.OnPickUp(this);
    }

    public void SetGUID(string GUID)
    {
        id = GUID;
    }

    public void DestroyIt()
    {
        Destroy(gameObject);
    }

    public ISavable GetISavableItem()
    {
        if (item != null && item is AWeapon)
        {
            return item as ISavable;
        }
        return null;
    }

    public void SetPosition(Vector2 position)
    {
        transform.position= position;
    }

    public bool HaveItem => item != null;

    

    public int GetHealth() => Health;
    public int GetMaxHealth() => MaxHealth;
}
