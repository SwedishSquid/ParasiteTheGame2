using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniPovarHorizontalAI : MonoBehaviour
{
    MiniPovarHorizontal miniPovar;
    int moveDirection = 0;
    bool isMovingRight = true;
    int freezeTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        miniPovar = GetComponent<MiniPovarHorizontal>();
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
        var inpInf = new InputInfo(new Vector2(0, 0), new Vector3(0, 0, 0), false, !miniPovar.HaveItem, true, false);
        if (isMovingRight == true && freezeTime == 0)
        {
            inpInf = new InputInfo(new Vector2(1, 0), new Vector3(-1, 0, 0), false, !miniPovar.HaveItem, true, false);
            moveDirection -= 1;
            if (moveDirection == -100)
            {
                isMovingRight = false;
                freezeTime = 75;
            }
            miniPovar.ControlledUpdate(inpInf);
        }
        else if (freezeTime == 0)
        {
            inpInf = new InputInfo(new Vector2(-1, 0), new Vector3(1, 0, 0), false, !miniPovar.HaveItem, true, false);
            moveDirection += 1;
            if (moveDirection == 100)
            {
                isMovingRight = true;
                freezeTime = 75;
            }
            miniPovar.ControlledUpdate(inpInf);
        }
        else
        {
            freezeTime -= 1;
            miniPovar.ControlledUpdate(inpInf);
        }
        //miniPovar.ControlledUpdate(inpInf);
    }
}
