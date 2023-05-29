using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIntelligence : MonoBehaviour
{
    public virtual bool TryTurnOff()
    {
        if (enabled)
        {
            enabled = false;
            return true;
        }
        return false;
    }

    protected virtual InputInfo PassedOutMode()
    {
        return new InputInfo();
    }
}
