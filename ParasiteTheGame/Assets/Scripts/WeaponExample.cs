using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponExample : MonoBehaviour, IWeapon
{
    private Vector3 forward;

    // Start is called before the first frame update
    void Start()
    {
        forward = new Vector3(0, 0, 1);
    }

    public void OnDropDown(IControlable user)
    {
        Debug.Log("dropped");
    }

    public void OnPickUp(IControlable user)
    {
        
    }

    public void RotateAndMove(Vector2 desiredRotation, Vector2 centre, float radius)
    {
        var angle = Mathf.Atan2(desiredRotation.y, desiredRotation.x) * (180 / Mathf.PI);
        transform.position = centre + (desiredRotation * radius);
        transform.rotation = Quaternion.AngleAxis(angle, forward);
        //Debug.Log(angle);
    }

    public void Use()
    {
        Debug.Log("Puf Puf Puf!!!");
    }
}
