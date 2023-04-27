using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowHandler : MonoBehaviour
{
    protected GameObject thrown;
    protected float minSpeed = 0.02f;
    protected float slowDownFactor; //0 - 0.999999..
    protected bool turnOffRigidbody = true;
    protected int timeout = 1000;   //ticks
    protected int ticks = 0;
    // Start is called before the first frame update
    public void InitIt(GameObject thrown, float slowDownFactor = 0.9996f, int timeOut = 1000, float minSpeed = 0.02f)
    {
        this.thrown = thrown;
        this.minSpeed = minSpeed;
        timeout = timeOut;
        this.slowDownFactor = slowDownFactor;
    }

    void Update()
    {
        if (thrown == null || !thrown.TryGetComponent<Rigidbody2D>(out var rigid))
        {
            Destroy(gameObject);
            return;
        }
        rigid.velocity *= slowDownFactor;
        ticks++;
        if (ticks > timeout)
        {
            rigid.velocity = Vector2.zero;
            rigid.simulated = false;
            Destroy(rigid);
            thrown.layer = LayerMask.NameToLayer("Weapons");
            Destroy(gameObject);
        }
    }
}
