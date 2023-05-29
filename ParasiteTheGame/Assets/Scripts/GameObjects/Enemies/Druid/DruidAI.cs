using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DruidAI : AIntelligence
{
    private Druid druid;
    // Update is called once per frame
    void Update()
    {
        if (druid == null)
        {
            druid = GetComponent<Druid>();
        }

        druid.ControlledUpdate(new InputInfo(Vector2.zero, Vector2.one, false, false, false, false));
    }
}
