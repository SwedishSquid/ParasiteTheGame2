using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : ASoundable
{
    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MiniPovar" || collision.gameObject.layer == 8)
        {
            PlaySound(AudioClips[0], pitchChanged: false);
            transform.position = new Vector2(0.11f, -0.18f);
        }
    }
}
