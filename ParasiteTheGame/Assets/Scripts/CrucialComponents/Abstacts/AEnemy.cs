using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor.Sprites;
using UnityEngine;

public abstract class AEnemy : MonoBehaviour, IControlable, IDamagable, IUser, ISavable
{
    [SerializeField] protected string id;

    [ContextMenu("Generate GUID for id")]
    protected void GenerateGUID()
    {
        id = System.Guid.NewGuid().ToString();
    }

    protected Rigidbody2D myRigidbody;
    protected float velocity = 10;
    
    protected IUsable item;
    protected string itemGUID = "";

    protected float radius = 1.06f;
    protected float itemPickingRadius = 2f;
    public bool IsCaptured;
    protected int health = 100;
    protected DamageSource damageSource = DamageSource.Enemy;
    
    protected Vector2 damageDir;
    protected float freezeVelocity = 3;
    protected float maxFreezeTime = OtherConstants.CommonMaxFreezeTime;
    protected float maxImmunityTime = OtherConstants.CommonImmunityTime;
    protected float freezeTime;
    protected float immunityTime;

    public virtual bool CanBeCaptured { get; protected set; } = true;

    protected virtual void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        if (id == null)
        {
            Debug.LogError($"To enable saving system operation on this item ({this.gameObject}) GUID must be generated");
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

        if (immunityTime > 0)
        {
            immunityTime -= Time.deltaTime;
        }
        
        if (item != null)
        {
            item.HandleUpdate(inpInf);
            if (inpInf.ThrowItemPressed)
            {
                item.Throw(inpInf);
                item = null;
                itemGUID = "";
            }
        }
    }

    public virtual void OnCapture(PlayerController player)
    {
        IsCaptured = true;
        damageSource = DamageSource.Player;
    }

    public virtual void OnRelease(PlayerController player)
    {
        myRigidbody.velocity = new Vector2(0, 0);
        if (item != null)
        {
            DropDown();
        }
        IsCaptured = false;
        damageSource = DamageSource.Enemy;
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
            if (immunityTime > 0)
            {
                return true;
            }
            health -= dmgInf.Amount;
            GetDamageEffect(dmgInf);
            
            Debug.Log($"Creature hurt : health = {health}");
            return true;
        }
        return false;
    }

    public void GetDamageEffect(DamageInfo dmgInf)
    {
        freezeTime = maxFreezeTime;
        immunityTime = maxImmunityTime;
        myRigidbody.velocity += dmgInf.Direction * (freezeVelocity * dmgInf.DamageVelocityMultiplier);
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

    public void SaveGame(GameData gameData)
    {
        var enemyData = gameData.GetEnemyOnSceneByGUID(gameData.CurrentLevelName, id);

        enemyData.EnemyPosition = transform.position;
        enemyData.CanBeCaptured = CanBeCaptured;
        enemyData.PickedItemGUID = itemGUID;
        enemyData.Health = health;
    }

    public void LoadData(GameData gameData)
    {
        if (gameData.TryGetEnemyOnLevelByGUID(gameData.CurrentLevelName, id, out var enemyData))
        {
            transform.position = enemyData.EnemyPosition;
            CanBeCaptured = enemyData.CanBeCaptured;
            itemGUID = enemyData.PickedItemGUID;
            health = enemyData.Health;
        }

        enemyData.thisEnemy = this;
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

        if (itemGUID == null)
        {
            Debug.Log("what the heck?!!");
        }

        item = gameData.GetItemOnSceneByGUID(gameData.CurrentLevelName, itemGUID).thisItem;

        item.OnPickUp(this);
    }

    public bool HaveItem => item != null;
}
