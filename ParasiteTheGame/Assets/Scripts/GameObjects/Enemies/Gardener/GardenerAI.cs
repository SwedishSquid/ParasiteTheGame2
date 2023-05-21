using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenerAI : MonoBehaviour
{
    Gardener gardener;

    // Start is called before the first frame update
    void Start()
    {
        gardener = GetComponent<Gardener>();
    }

    // Update is called once per frame
    void Update()
    {
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
