using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossfightTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LevelOneBossfightController.Instance == null)
        {
            return;
        }

        if (LevelOneBossfightController.Instance.bossfightState == BossfightState.NotStarted)
        {
            LevelOneBossfightController.Instance.StartBossfight();
            Debug.Log("starting bossfight");
        }
        else
        {
            Debug.Log("ending bossfight");
            LevelOneBossfightController.Instance.EndBossfight();
        }
    }
}
