using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenerAI : MonoBehaviour
{
    Gardener gardener;
    Animator animator;

    void Start()
    {
        gardener = GetComponent<Gardener>();
        animator = gardener.GetAnimator();
    }

    void Update()
    {
        if (PauseController.gameIsPaused)
            return;
        var direction = new Vector2(Random.value*2 - 1, Random.value*2 - 1).normalized;
        //
        if (!gardener.HaveItem)
        {
            gardener.ActOnPickOrDrop();
        }
        //
        var inpInf = new InputInfo(new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)), direction, false, !gardener.HaveItem, true, false);
        gardener.ControlledUpdate(inpInf);
    }
}
