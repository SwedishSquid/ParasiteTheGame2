using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LackeySmallAI : AIntelligence
{
    LackeySmall lackey;
    // Start is called before the first frame update
    void Start()
    {
        lackey = GetComponent<LackeySmall>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lackey.PassedOut)
        {
            lackey.ControlledUpdate(PassedOutMode());
            return;
        }

        var direction = new Vector2(Random.value*2 - 1, Random.value*2 - 1).normalized;
        //
        if (!lackey.HaveItem)
        {
            lackey.ActOnPickOrDrop();
        }
        //
        var inpInf = new InputInfo(new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)), direction, false, !lackey.HaveItem, true, false);
        lackey.ControlledUpdate(inpInf);
    }
}
