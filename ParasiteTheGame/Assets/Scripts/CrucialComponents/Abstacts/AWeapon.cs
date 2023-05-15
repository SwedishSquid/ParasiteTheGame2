using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public abstract class AWeapon : MonoBehaviour, IUsable, ISavable
{
    [SerializeField] protected string id;

    [ContextMenu("Generate GUID for id")]
    protected void GenerateGUID()
    {
        id = System.Guid.NewGuid().ToString();
    }

    protected IUser user;
    protected int damageAmount = 7;
    protected float fireRate = 0.5f; //seconds between fire
    protected float cooldownLeft = 0;
    protected float throwSpeed = 30;

    [SerializeField] protected ThrowHandler throwHandlerPrefab;

    protected ThrowComponent throwComponent;
    [SerializeField] protected AudioSource audioSource;

    protected DamageSource _lastDamageSource;
    protected DamageSource damageSource { 
        get
        {
            if (user != null)
            {
                _lastDamageSource = user.GetDamageSource();
            }
            return _lastDamageSource;
        }
    }

    protected virtual void Awake()
    {
        throwComponent = GetComponent<ThrowComponent>();
        throwComponent.enabled = false; //just in case weapon creator forgets
    }

    protected virtual void Start()
    {
        
    }

    public virtual void HandleUpdate(InputInfo inpInf)
    {
        HandleMovement(inpInf);
        if (inpInf.FirePressed && cooldownLeft <= 0)
        {
            cooldownLeft = fireRate;
            Fire(inpInf);
        }
        if (cooldownLeft > 0)
        {
            cooldownLeft -= Time.deltaTime;
        }
    }

    protected virtual void HandleMovement(InputInfo inpInf)
    {
        var desiredRotation = inpInf.GetMouseDir();
        var angle = Mathf.Atan2(desiredRotation.y, desiredRotation.x) * (180 / Mathf.PI);
        transform.position = user.GetUserPosition() + (desiredRotation * user.GetUserRadius());
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    protected virtual void Fire(InputInfo inpInf)
    {
        //summon something
    }

    public virtual void OnPickUp(IUser user)
    {
        if (throwComponent.enabled)
        {
            throwComponent.EndThrow();
        }
        this.user = user;
        gameObject.GetComponent<Collider2D>().enabled = false;
    }

    public virtual void OnDropDown(IUser user)
    {
        this.user = null;
        gameObject.GetComponent<Collider2D>().enabled = true;
    }

    public virtual void Throw(InputInfo inpInf)
    {
        _ = damageSource; //update damageInfo in case it wasn't updated before
        var userVelocity = user.GetUserVelocity();
        OnDropDown(user);
        throwComponent.StartThrow(inpInf.GetMouseDir(), userVelocity * 0.2f);
    }

    public virtual void DealDamageByThrow(IDamagable damagable)
    {
        //this rigidbody is not permanent - it will be destroyed sooner or later
        if(TryGetComponent<Rigidbody2D>(out var rigidbody2))
        {
            damagable.TryTakeDamage(
                new DamageInfo(DamageType.Melee, damageSource, damageAmount, Vector2.zero, 0));
        }
    }

    public void SaveGame(GameData gameData)
    {
        var itemData = gameData.GetItemOnSceneByGUID(gameData.CurrentLevelName, id);

        itemData.ItemPosition = transform.position;
    }

    public void LoadData(GameData gameData)
    {
        if (gameData.TryGetItemOnLevelByGUID(gameData.CurrentLevelName, id, out var itemData))
        {
            transform.position = itemData.ItemPosition;
        }

        itemData.thisItem = this;
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
        //do nothing :D
    }
}
