using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LackeySmallAI : MonoBehaviour
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
        var direction = new Vector2(Random.value*2 - 1, Random.value*2 - 1).normalized;
        var inpInf = new InputInfo(new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)), direction, false, !lackey.HaveItem, true, false, false);
        lackey.ControlledUpdate(inpInf);
    }
}
