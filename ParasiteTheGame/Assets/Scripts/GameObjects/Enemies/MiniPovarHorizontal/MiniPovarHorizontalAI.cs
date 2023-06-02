using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniPovarHorizontalAI : MonoBehaviour
{
    MiniPovarHorizontal miniPovar;
    //int moveDirection = 0;
    bool isMovingRight = false;
    //int freezeTime = 0;

    private readonly InputInfo standStill = new InputInfo(new Vector2(0, 0), new Vector3(0, 0, 0), false, false, false, false);
    private readonly InputInfo moveRight = new InputInfo(new Vector2(1, 0), new Vector3(-1, 0, 0), false, false, false, false);
    private readonly InputInfo moveLeft = new InputInfo(new Vector2(-1, 0), new Vector3(1, 0, 0), false, false, false, false);

    private AIMode mode = AIMode.Waiting;
    private float modeTimeLeft = 1;

    // Start is called before the first frame update
    void Start()
    {
        miniPovar = GetComponent<MiniPovarHorizontal>();
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
                modeTimeLeft = 1f;
                mode = AIMode.AimApproaching;
                if (isMovingRight)
                {
                    isMovingRight= false;
                }
                else
                {
                    isMovingRight= true;
                }
            }
        }
        else if(mode == AIMode.AimApproaching)
        {
            if (isMovingRight)
            {
                inpInf = moveRight;
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
        miniPovar.ControlledUpdate(inpInf);
    }
}
