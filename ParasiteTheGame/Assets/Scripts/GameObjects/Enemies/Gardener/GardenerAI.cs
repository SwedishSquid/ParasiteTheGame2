using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class GardenerAI : AIntelligence
{
    AEnemy body;
    private AllSeeingEye eye;

    private float updateInterval = 0.5f;
    private float updateTimeLeft = 0;

    private IKillable aim;

    private AWeapon itemAim;

    private AIMode mode = AIMode.AimSearch;

    private float retreatRadius = 4;

    private float approachRadius = 10;

    private InputInfo previousInput;

    private float modeTimeLeft;

    private float subModeTimeLeft;


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<AEnemy>();
        eye = GetComponent<AllSeeingEye>();
        eye.Radius = 30;
        var direction = new Vector2(Random.value * 2 - 1, Random.value * 2 - 1).normalized;
        previousInput = new InputInfo(new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)), direction, false, !body.HaveItem, true, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (modeTimeLeft >= 0)
        {
            modeTimeLeft -= Time.deltaTime;
        }
        if (subModeTimeLeft>= 0)
        {
            subModeTimeLeft -= Time.deltaTime;
        }
        

        var direction = new Vector2(Random.value * 2 - 1, Random.value * 2 - 1).normalized;
        var inpInf = new InputInfo(new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)), direction, false, !body.HaveItem, true, false);

        if (aim == null || mode == AIMode.AimSearch)
        {
            inpInf = AimlessMode();
        }
        else if (mode == AIMode.PassedOut)
        {
            inpInf = PassedOutMode();
        }
        else if (mode == AIMode.StrategyMaking)
        {
            inpInf = StrategyMaking();
        }
        else if (mode == AIMode.StandartItemAttack)
        {
            inpInf = StandartItemAttack();
        }
        else if (mode == AIMode.Retreat)
        {
            inpInf = Retreat();
        }
        else if (mode == AIMode.AimApproaching)
        {
            inpInf = AimApproaching();
        }
        else if (mode == AIMode.Waiting)
        {
            inpInf = WaitMode();
        }
        else if (mode == AIMode.SuperAttack)
        {
            inpInf = UseSuperAttack();
        }
        else if (mode == AIMode.ThrowItemAttack)
        {
            inpInf = ThrowAttack();
        }
        else if (mode == AIMode.ItemSearch)
        {
            inpInf = ItemSearch();
        }
        else if (mode == AIMode.ItemCapture)
        {
            inpInf = ItemCapture();
        }
        else if (mode == AIMode.DistantAttack)
        {
            inpInf = DistantAttack();
        }
        else if (mode == AIMode.MeleeAttack)
        {
            inpInf = MeleeAttack();
        }
        else
        {
            Debug.LogError($"cannot process AIMode {mode}");
        }

        previousInput = inpInf;
        body.ControlledUpdate(inpInf);
    }

    private InputInfo AimlessMode()
    {
        if(SearchAim() != null)
        {
            mode = AIMode.StrategyMaking;
        }

        var direction = new Vector2(Random.value * 2 - 1, Random.value * 2 - 1).normalized;
        return new InputInfo(new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)), direction, false, !body.HaveItem, true, false);
    }

    /*protected override InputInfo PassedOutMode()
    {
        if (modeTimeLeft <= 0)
        {
            mode = AIMode.StrategyMaking;
        }
        return new InputInfo(Vector2.zero, Vector2.zero, false, gardener.HaveItem, false, false);
    }*/

    private InputInfo StrategyMaking()
    {
        if (body.PassedOut)
        {
            modeTimeLeft = 1;
            mode = AIMode.PassedOut;
            return CopyLastInputRoughly();
        }


        var dist = aimDist;
        Debug.Log($"distance is {dist}");
        if (aim.Dead || !aim.CanBeHit)
        {
            mode = AIMode.AimSearch;
        }else if (!body.HaveItem && Random.value > 0.7)
        {
            mode = AIMode.ItemSearch;
        }
        else if (body.AlmostPassedOut && Random.value > 0.5 && dist > body.GetUserRadius())
        {
            mode = AIMode.ThrowItemAttack;
        }
        else if (dist < retreatRadius)
        {
            if (Random.value > 0.5)
            {
                mode = AIMode.SuperAttack;
            }
            else
            {
                modeTimeLeft = 0.3f;
                mode = AIMode.Retreat;
            }
        }
        else if (dist > approachRadius)
        {
            modeTimeLeft = 0.6f;
            mode = AIMode.AimApproaching;
        }
        else if (Random.value > 0.3)
        {
            modeTimeLeft = 0.6f;
            mode = AIMode.Waiting;
        }
        else
        {
            //modeTimeLeft = 2;
            mode = AIMode.StandartItemAttack;
        }
        Debug.Log($"mode = {mode}");
        return CopyLastInputRoughly();
    }

    private InputInfo CopyLastInputRoughly()
    {
        return new InputInfo(previousInput.Axis, previousInput.MouseDirection, false, false, previousInput.FirePressed, false);
    }

    private InputInfo ThrowAttack()
    {
        mode = AIMode.StrategyMaking;
        return new InputInfo(previousInput.Axis, aimDir.normalized, false, false, false, true);
    }

    private InputInfo WaitMode()
    {
        if (modeTimeLeft <= 0)
        {
            mode = AIMode.StrategyMaking;
        }
        return CopyLastInputRoughly();
    }

    //change please
    private InputInfo StandartItemAttack()
    {
        if (body.GetDamageType() == DamageType.Distant)
        {
            modeTimeLeft = 1f;
            mode = AIMode.DistantAttack;
        }
        else
        {
            modeTimeLeft = 1.5f;
            mode = AIMode.MeleeAttack;
        }

        return CopyLastInputRoughly();
    }

    private InputInfo MeleeAttack()
    {
        if (modeTimeLeft <= 0)
        {
            mode = AIMode.StrategyMaking;
        }

        var speedMult = Mathf.Min(1, aimDist / 3);

        /*if (aimDist < 3 && Random.value > 0.3f)
        {
            subModeTimeLeft = 0.2f;
            mode = AIMode.CloseDistanceMeleeAttack;
        }
        else
        {
            subModeTimeLeft = 0.3f;
            mode = AIMode.FarDistanceMeleeAttack;
        }*/

        var walkDirection = -((Vector2)transform.position - aim.Position).normalized * speedMult;
        var aimDirection = (walkDirection + new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)) * 0.3f);
        return new InputInfo(walkDirection, aimDirection, false, !body.HaveItem, true, false);
    }

    private InputInfo FarDistanceMeleeAttack()
    {
        if (subModeTimeLeft <= 0)
        {
            mode = AIMode.MeleeAttack;
        }

        var walkDirection = -((Vector2)transform.position - aim.Position).normalized;
        var aimDirection = (walkDirection + new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)) * 0.3f);
        return new InputInfo(walkDirection, aimDirection, false, !body.HaveItem, true, false);
    }

    private InputInfo CloseDistanceMeleeAttack()
    {
        if (subModeTimeLeft <= 0)
        {
            mode = AIMode.MeleeAttack;
        }

        var walkDirection = -((Vector2)transform.position - aim.Position).normalized / 3;
        var aimDirection = (walkDirection + new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)) * 0.3f);
        return new InputInfo(walkDirection, aimDirection, false, false, true, false);
    }

    private InputInfo DistantAttack()
    {
        if (modeTimeLeft <= 0)
        {
            mode = AIMode.StrategyMaking;
        }

        var walkDirection = -((Vector2)transform.position - aim.Position).normalized;
        var aimDirection = (walkDirection + new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)) * 0.1f);
        return new InputInfo(walkDirection, aimDirection, false, false, true, false);
    }

    private InputInfo UseSuperAttack()
    {
        modeTimeLeft = 0.2f;
        mode = AIMode.Waiting;
        return new InputInfo(new Vector2(0, 0), previousInput.MouseDirection, false, false, false, false, true);
    }

    private InputInfo Retreat()
    {
        if (modeTimeLeft <= 0)
        {
            mode = AIMode.StrategyMaking;
        }

        var walkDirection = -aimDir.normalized;
        var aimDirection = aimDir.normalized;
        return new InputInfo(walkDirection, aimDirection, false, false, false, false);
    }
    
    private InputInfo AimApproaching()
    {
        if (modeTimeLeft <= 0)
        {
            mode = AIMode.StrategyMaking;
        }

        var walkDirection = aimDir.normalized;
        var aimDirection = aimDir.normalized;
        return new InputInfo(walkDirection, aimDirection, false, false, false, false);
    }

    private InputInfo ItemSearch()
    {
        itemAim= null;
        foreach (var obj in eye.GetAllObjectsInView(LayerConstants.PickableItems))
        {
            if (obj.TryGetComponent(out AWeapon item))
            {
                itemAim = item;
                break;
            }
        }

        if (itemAim != null)
        {
            modeTimeLeft = 1.3f;
            mode = AIMode.ItemCapture;
        }
        else
        {
            mode = AIMode.StrategyMaking;
        }
        
        return CopyLastInputRoughly();
    }

    private InputInfo ItemCapture()
    {
        if (modeTimeLeft <= 0 || body.HaveItem)
        {
            modeTimeLeft= 0;
            mode = AIMode.StrategyMaking;
        }

        return new InputInfo(ItemDir.normalized, ItemDir.normalized, false, !body.HaveItem, false, false);
    }

    private IKillable SearchAim()
    {
        updateTimeLeft -= Time.deltaTime;
        if (updateTimeLeft > 0)
        {
            return null;
        }
        updateTimeLeft = updateInterval;

        //Debug.Log("doing research");

        aim = null;

        foreach (var obj in eye.GetAllObjectsInView(LayerConstants.ControllablesLayer))
        {
            if (obj.TryGetComponent(out IKillable creature) && creature.CanBeHit)
            {
                aim = creature;
                break;
            }
        }

        //Debug.Log($"found {(aim == null ? "nothing" : aim)}");

        return aim;
    }

    private Vector2 ItemDir => (Vector2)(itemAim.transform.position - transform.position);

    private Vector2 aimDir => (aim.Position - (Vector2)transform.position);

    private float aimDist => aimDir.magnitude;
}
