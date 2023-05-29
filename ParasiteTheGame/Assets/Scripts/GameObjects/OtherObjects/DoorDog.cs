using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDog : MonoBehaviour, IBossfightListener
{
    public void OnBossfightEnd()
    {
        Destroy(gameObject);
    }

    public void OnBossfightStart()
    {
        //nothing
    }

    public void OnLoadAfterBossfight()
    {
        Destroy(gameObject);
    }

    public void OnLoadDuringBossfight()
    {
        //nothing again
    }
}
