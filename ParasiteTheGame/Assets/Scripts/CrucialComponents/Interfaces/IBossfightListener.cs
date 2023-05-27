using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public interface IBossfightListener
{
    public void OnBossfightStart();

    public void OnLoadDuringBossfight();

    public void OnBossfightEnd();

    public void OnLoadAfterBossfight();

}
