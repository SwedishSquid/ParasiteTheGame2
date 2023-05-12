using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponExample : MonoBehaviour, IWeapon
{
    private Vector3 forward;
    private DamageSource damageSource;
    private Vector3 currentDirection;
    //bullet also may have its own interface but for now it seems to be an overkill
    [SerializeField] private BulletExample bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        forward = new Vector3(0, 0, 1);
        currentDirection = new Vector3();
    }

    public void OnDropDown(IControlable user)
    {
        Debug.Log("dropped");
        damageSource = DamageSource.Enemy;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void OnPickUp(IControlable user)
    {
        //an issue here
        damageSource = DamageSource.Player;
        //this way other controllables wont be able to pick it up
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    public void RotateAndMove(Vector2 desiredRotation, Vector2 centre, float radius)
    {
        var angle = Mathf.Atan2(desiredRotation.y, desiredRotation.x) * (180 / Mathf.PI);
        transform.position = centre + desiredRotation * radius;
        transform.rotation = Quaternion.AngleAxis(angle, forward);
        currentDirection = desiredRotation;
    }

    public void Use()
    {
        Debug.Log("Puf Puf Puf!!!");
        var bullet = Instantiate(bulletPrefab, transform.position + currentDirection * 0.6f, transform.rotation);
        bullet.SetParameters(new DamageInfo(DamageType.Distant, damageSource, 1, currentDirection),
            currentDirection, 10, 0.2f, 2000);
    }
}
