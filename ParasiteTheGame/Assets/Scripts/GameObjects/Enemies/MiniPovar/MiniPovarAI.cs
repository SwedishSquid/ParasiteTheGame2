/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniPovarAI : AIntelligence
{
    MiniPovar miniPovar;
    // Start is called before the first frame update
    void Start()
    {
        miniPovar = GetComponent<MiniPovar>();
    }

    // Update is called once per frame
    void Update()
    {
        var direction = new Vector2(Random.value*2 - 1, Random.value*2 - 1).normalized;
        //
        if (!miniPovar.HaveItem)
        {
            miniPovar.ActOnPickOrDrop();
        }
        //
        var inpInf = new InputInfo(new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)), direction, false, !miniPovar.HaveItem, true, false);
        miniPovar.ControlledUpdate(inpInf);
    }
}
*/