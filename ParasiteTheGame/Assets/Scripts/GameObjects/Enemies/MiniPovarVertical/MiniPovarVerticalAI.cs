using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniPovarVerticalAI : MonoBehaviour
{
    MiniPovarVertical miniPovarV;
    bool isMovingUp = true;

    private readonly InputInfo standStill = new InputInfo(new Vector2(0, 0), new Vector3(0, 0, 0), false, false, false, false);
    private readonly InputInfo moveUp = new InputInfo(new Vector2(0, 1), new Vector3(-1, 0, 0), false, false, false, false);
    private readonly InputInfo moveLeft = new InputInfo(new Vector2(0, -1), new Vector3(1, 0, 0), false, false, false, false);

    private AIMode mode = AIMode.AimApproaching;
    private float modeTimeLeft = 0.3f;
    private float movingTime = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        miniPovarV = GetComponent<MiniPovarVertical>();
    }

    // Update is called once per frame
    void Update()
    {
        if (modeTimeLeft >= 0)
        {
            modeTimeLeft -= Time.deltaTime;
        }

        var inpInf = standStill;

        if (mode == AIMode.Waiting)
        {
            if (modeTimeLeft <= 0)
            {
                modeTimeLeft = movingTime;
                mode = AIMode.AimApproaching;
                if (isMovingUp)
                {
                    isMovingUp = false;
                }
                else
                {
                    isMovingUp = true;
                }
            }
        }
        else if (mode == AIMode.AimApproaching)
        {
            if (isMovingUp)
            {
                inpInf = moveUp;
            }
            else
            {
                inpInf = moveLeft;
            }
            if (modeTimeLeft <= 0)
            {
                modeTimeLeft = 0.75f;
                mode = AIMode.Waiting;
            }
        }
        miniPovarV.ControlledUpdate(inpInf);
    }
}