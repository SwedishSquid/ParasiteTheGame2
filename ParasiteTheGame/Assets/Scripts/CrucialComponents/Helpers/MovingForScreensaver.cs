using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingForScreensaver : MonoBehaviour
{
    private float speed = 3f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Time.deltaTime * speed, 0, 0);
    }
}
