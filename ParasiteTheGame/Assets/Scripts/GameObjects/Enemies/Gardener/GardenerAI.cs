using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class GardenerAI : AIntelligence
{
    Gardener gardener;
    private AllSeeingEye eye;

    private float updateInterval = 0.5f;
    private float updateTimeLeft = 0;

    private AEnemy aim;

    private AIMode mode = AIMode.AimSearch;

    private float retreatRadius = 4;

    private float approachRadius = 10;

    private InputInfo previousInput;

    private float modeTimeLeft;


    // Start is called before the first frame update
    void Start()
    {
        gardener = GetComponent<Gardener>();
        eye = GetComponent<AllSeeingEye>();
        var direction = new Vector2(Random.value * 2 - 1, Random.value * 2 - 1).normalized;
        previousInput = new InputInfo(new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)), direction, false, !gardener.HaveItem, true, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (modeTimeLeft >= 0)
        {
            modeTimeLeft -= Time.deltaTime;
        }
        

        var direction = new Vector2(Random.value * 2 - 1, Random.value * 2 - 1).normalized;
        var inpInf = new InputInfo(new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)), direction, false, !gardener.HaveItem, true, false);

        if (aim == null || mode == AIMode.AimSearch)
        {
            inpInf = AimlessMode();
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
        }else if (mode == AIMode.Waiting)
        {
            inpInf = WaitMode();
        }
        else
        {
            Debug.LogError($"cannot process AIMode {mode}");
        }

        previousInput = inpInf;
        gardener.ControlledUpdate(inpInf);
    }

    private InputInfo AimlessMode()
    {
        if(SearchAim() != null)
        {
            mode = AIMode.StrategyMaking;
        }

        var direction = new Vector2(Random.value * 2 - 1, Random.value * 2 - 1).normalized;
        return new InputInfo(new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)), direction, false, !gardener.HaveItem, true, false);
    }

    private InputInfo StrategyMaking()
    {
        var dist = aimDist;
        Debug.Log($"distance is {dist}");
        if (aim.Dead)
        {
            mode = AIMode.AimSearch;
        }
        else if (dist < retreatRadius)
        {
            modeTimeLeft = 0.3f;
            mode = AIMode.Retreat;
        }
        else if (dist > approachRadius)
        {
            modeTimeLeft = 0.6f;
            mode = AIMode.AimApproaching;
        }/*else if (eye.DistanceToObstacleByRay(aimDir, LayerConstants.ObstaclesLayer) < dist)
        {
            mode = AIMode.AvoidObstacle;
        }*/
        else if (Random.value > 0.3)
        {
            modeTimeLeft = 0.6f;
            mode = AIMode.Waiting;
        }
        else
        {
            modeTimeLeft = 2;
            mode = AIMode.StandartItemAttack;
        }
        Debug.Log($"mode = {mode}");
        return new InputInfo(previousInput.Axis, previousInput.MouseDirection, false, false, false, false);
    }

    private InputInfo WaitMode()
    {
        if (modeTimeLeft <= 0)
        {
            mode = AIMode.StrategyMaking;
        }
        return new InputInfo(previousInput.Axis, previousInput.MouseDirection, false, false, false, false);
    }

    //change please
    private InputInfo StandartItemAttack()
    {
        if (modeTimeLeft <= 0)
        {
            mode = AIMode.StrategyMaking;
        }

        var walkDirection = -((Vector2)(transform.position - aim.gameObject.transform.position)).normalized;
        var aimDirection = (walkDirection + new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)) * 0.3f);
        return new InputInfo(walkDirection, aimDirection, false, !gardener.HaveItem, true, false);
    }

    /*private InputInfo ManhuntMode()
    {
        if (aim.Dead)
        {
            aim = null;
            return AimlessMode();
        }

        var walkDirection = -((Vector2)(transform.position - aim.gameObject.transform.position)).normalized;
        var aimDirection = (walkDirection + new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)) * 0.3f);
        return new InputInfo(walkDirection, aimDirection, false, !gardener.HaveItem, true, false);
    }*/

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

    private AEnemy SearchAim()
    {
        updateTimeLeft -= Time.deltaTime;
        if (updateTimeLeft > 0)
        {
            return null;
        }
        updateTimeLeft = updateInterval;

        Debug.Log("doing research");

        foreach (var obj in eye.GetAllObjectsInView(LayerConstants.ControllablesLayer))
        {
            if (obj.TryGetComponent(out AEnemy creature) && creature.IsCaptured && creature is not Gardener)
            {
                aim = creature;
                break;
            }
        }

        Debug.Log($"found {aim}");

        return aim;
    }

    private Vector2 aimPos => aim.gameObject.transform.position;

    private Vector2 aimDir => (aimPos - (Vector2)transform.position);

    private float aimDist => aimDir.magnitude;
}
