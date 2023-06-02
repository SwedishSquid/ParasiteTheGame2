using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChangerTransporter : MonoBehaviour, IBossfightListener
{
    [SerializeField] private MonoBehaviour transported;

    public void OnBossfightEnd()
    {
        transported.transform.position = transform.position;
    }

    public void OnBossfightStart()
    {
        //nope
    }

    public void OnLoadAfterBossfight()
    {
        transported.transform.position = transform.position;
    }

    public void OnLoadDuringBossfight()
    {
        //nope
    }
}
