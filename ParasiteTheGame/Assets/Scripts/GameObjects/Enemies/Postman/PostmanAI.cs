using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostmanAI : MonoBehaviour
{
    [SerializeField] protected Postman postman;
    // Update is called once per frame
    void Update()
    {
        //it should just stand
        //to test bouncing when damaged
        postman.ControlledUpdate(new InputInfo(Vector2.zero, Vector2.one, false, false, false, false));
    }
}
