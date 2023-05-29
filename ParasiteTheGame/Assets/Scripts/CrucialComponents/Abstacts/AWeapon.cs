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
        //ITSigma - Pause
        if (PauseController.gameIsPaused)
            return;
        //

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
        //ITSigma - Pause
        if (PauseController.gameIsPaused)
            return;
        //

        if (throwComponent.enabled)
        {
            throwComponent.EndThrow();
        }
        this.user = user;
        gameObject.GetComponent<Collider2D>().enabled = false;
    }

    public virtual void OnDropDown(IUser user)
    {
        //ITSigma - Pause
        if (PauseController.gameIsPaused)
            return;
        //

        this.user = null;
        gameObject.GetComponent<Collider2D>().enabled = true;
    }

    public virtual void Throw(InputInfo inpInf)
    {
        //ITSigma - Pause
        if (PauseController.gameIsPaused)
            return;
        //
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

    public virtual void SaveGame(GameData gameData)
    {
        var itemData = gameData.GetItemToSave(id);

        itemData.ItemPosition = transform.position;

        itemData.TypeName = this.GetType().Name;//typeName;
    }

    public virtual void LoadData(GameData gameData)
    {
        if (gameData.Items.ContainsKey(id))
        {
            transform.position = gameData.Items[id].ItemPosition;
            gameData.Items[id].thisItem = this;
        }
    }

    public string GetGUID()
    {
        if (id == "")
        {
            Debug.LogError($"GUID for {this} is not set");
        }
        return id;
    }

    public void SetGUID(string GUID)
    {
        id = GUID;
    }

    public void AfterAllObjectsLoaded(GameData gameData)
    {
        //do nothing :D
    }

    public void DestroyIt()
    {
        Destroy(gameObject);
    }

    public void SetPosition(Vector2 position)
    {
        //no no no
        Debug.LogError("why is this called - SetPosition has no effect on item");
    }
}
