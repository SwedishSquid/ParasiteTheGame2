using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MiniPovar" || collision.gameObject.layer == 8)
            transform.position = new Vector2(0.11f, -0.18f);
    }
}
