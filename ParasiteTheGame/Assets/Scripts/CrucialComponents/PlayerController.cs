using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D thisRigidbody2d;
    private Vector2 input;
    private float velocity = 3.09f;
    private LayerMask controllablesLayer;
    public IControlable controlled;
    // Start is called before the first frame update
    void Start()
    {
        thisRigidbody2d = GetComponent<Rigidbody2D>();
        controllablesLayer = LayerMask.GetMask("Controllables");
    }

    // Update is called once per frame
    public void HandleUpdate(InputInfo inpInf)
    {
        input = inpInf.Axis;
        thisRigidbody2d.velocity = input * velocity;
    }

    public void ActOnJumpout()
    {
        if (controlled == null)
        {
            Capture();
        }
        else
        {
            LetItGo();
        }
    }

    private void Capture()
    {
        var t = Physics2D.OverlapPoint(transform.position, controllablesLayer);
        if (t)
        {
            controlled = t.gameObject.GetComponent<IControlable>();
            thisRigidbody2d.simulated = false;
            controlled.OnCapture(this);
        }
        else
        {
            Debug.Log("Nope");
        }
    }

    private void LetItGo()
    {
        thisRigidbody2d.simulated = true;
        controlled.OnRelease(this);
        controlled = null;
    }
}
