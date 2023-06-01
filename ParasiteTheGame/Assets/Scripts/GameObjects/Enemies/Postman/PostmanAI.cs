using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PostmanAI : AIntelligence
{
    //[SerializeField] protected Postman postman;

    private AEnemy body;

    private AIMode mode = AIMode.JustDance;

    private Vector2 preferedDirection = Vector2.left * 0.5f;

    private float modeTimeLeft;

    private InputInfo previousInput;

    private void Start()
    {
        body = GetComponent<AEnemy>();
        previousInput = PassedOutMode();
    }

    // Update is called once per frame
    void Update()
    {
        if (modeTimeLeft >= 0)
        {
            modeTimeLeft -= Time.deltaTime;
        }

        var inpInf = JustDance();

        if (mode == AIMode.PassedOut)
        {
            //Debug.Log("LackeyPassedOut");
            inpInf = PassedOutMode();
        }
        else if (mode == AIMode.JustDance)
        {
            inpInf = JustDance();
        }
        else if (mode == AIMode.StrategyMaking)
        {
            inpInf = StrategyMaking();
        }

        previousInput = inpInf;
        body.ControlledUpdate(inpInf);
    }

    private InputInfo StrategyMaking()
    {
        if (body.PassedOut)
        {
            modeTimeLeft = 1;
            mode = AIMode.PassedOut;
        }else
        {
            modeTimeLeft = 0.4f;
            preferedDirection *= -1;
            mode = AIMode.JustDance;
        }

        return CopyLastInputRoughly();
    }

    private InputInfo JustDance()
    {
        if (modeTimeLeft <= 0)
        {
            mode = AIMode.StrategyMaking;
        }
        return new InputInfo(preferedDirection, preferedDirection, false, !body.HaveItem, false, false);
    }

    private InputInfo CopyLastInputRoughly()
    {
        return new InputInfo(previousInput.Axis, previousInput.MouseDirection, false, false, previousInput.FirePressed, false);
    }
}
