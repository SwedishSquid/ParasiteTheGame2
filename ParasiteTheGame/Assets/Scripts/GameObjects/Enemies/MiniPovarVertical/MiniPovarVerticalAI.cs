using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniPovarVerticalAI : MonoBehaviour
{
    MiniPovarVertical miniPovarV;
    int moveDirection = 0;
    bool isMovingUp = true;
    int freezeTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        miniPovarV = GetComponent<MiniPovarVertical>();
    }

    // Update is called once per frame
    void Update()
    {
        //var direction = new Vector2(Random.value*2 - 1, Random.value*2 - 1).normalized;
        //
        // if (!miniPovar.HaveItem)
        // {
        //     miniPovar.ActOnPickOrDrop();
        // }
        //
        var inpInf = new InputInfo(new Vector2(0, 0), new Vector3(0, 0, 0), false, !miniPovarV.HaveItem, true, false);
        if (isMovingUp == true && freezeTime == 0)
        {
            inpInf = new InputInfo(new Vector2(0, 1), new Vector3(-1, 0, 0), false, !miniPovarV.HaveItem, true, false);
            moveDirection -= 1;
            if (moveDirection == -100)
            {
                isMovingUp = false;
                freezeTime = 75;
            }
            miniPovarV.ControlledUpdate(inpInf);
        }
        else if (freezeTime == 0)
        {
            inpInf = new InputInfo(new Vector2(0, -1), new Vector3(1, 0, 0), false, !miniPovarV.HaveItem, true, false);
            moveDirection += 1;
            if (moveDirection == 100)
            {
                isMovingUp = true;
                freezeTime = 75;
            }
            miniPovarV.ControlledUpdate(inpInf);
        }
        else
        {
            freezeTime -= 1;
            miniPovarV.ControlledUpdate(inpInf);
        }
        //miniPovar.ControlledUpdate(inpInf);
    }
}