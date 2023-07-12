using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
//using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
//using UnityEditor.Sprites;
using UnityEngine;

public abstract partial class AEnemy : ASoundable, IControlable, IDamagable, IUser, ISavable, IEnemyInfoPlate, IKillable
{
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D myRigidbody;
    protected float velocity = 10;

    protected IUsable item;

    public bool IsCaptured;
    public PlayerController Capturer;

    [SerializeField] protected BaseAttack baseAttack;

    protected AIntelligence intelligence;

    public virtual bool CanBeCaptured
    {
        get
        {
            return !Dead;
        }
    }

    protected DamageSource DamageSource
    {
        get
        {
            if (IsCaptured)
            {
                return DamageSource.Player;
            }
            return DamageSource.Enemy;
        }
    }

    protected virtual void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        intelligence = GetComponent<AIntelligence>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
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
            inpInf = inpInf.ConstructForFrozen();
        }else
        {
            myRigidbody.velocity = inpInf.Axis * velocity;
        }

        UpdateDamageTimers();

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

    public virtual void OnCapture(PlayerController player)
    {
        IsCaptured = true;
        Capturer = player;
        intelligence.enabled = false;
    }

    public virtual void OnRelease(PlayerController player)
    {
        myRigidbody.velocity = new Vector2(0, 0);
        IsCaptured = false;
        TryPassOut();
        if (!TryDie())
        {
            intelligence.enabled = true;
            intelligence.OnRelease();
        }
    }

    #region SomeStrangeMethods

    public virtual void UpdatePlayerPos(Transform playerTransform)
    {
        playerTransform.position = transform.position;
    }

    public ISavable GetISavableItem()
    {
        if (item != null && item is AWeapon)
        {
            return item as ISavable;
        }
        return null;
    }

    protected IEnumerator RedSprite()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }

    #endregion SomeStrangeMethods
}
