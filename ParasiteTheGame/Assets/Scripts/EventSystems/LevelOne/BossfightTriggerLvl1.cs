using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BossfightTriggerLvl1 : MonoBehaviour, IInteractable, IBossfightListener
{
    private bool isActive = true;

    [SerializeField] private Sprite spriteAfterInteraction;
    [SerializeField] private Gardener boss;

    private void Start()
    {
        if (isActive)
        {
            return;
        }
        ChangeSprite();
    }

    public void Interact(InteractorInfo interInfo)
    {
        if (LevelOneBossfightController.Instance == null)
        {
            return;
        }

        if (LevelOneBossfightController.Instance.bossfightState == BossfightState.NotStarted)
        {
            LevelOneBossfightController.Instance.StartBossfight();
            BringTheBoss();
            Debug.Log("starting bossfight");
        }
        else
        {
            Debug.Log("ending bossfight");
            LevelOneBossfightController.Instance.EndBossfight();
        }

        ChangeSprite();
    }

    private void BringTheBoss()
    {
        boss.gameObject.transform.position = gameObject.transform.position + Vector3.right * 10;
    }

    private void ChangeSprite()
    {
        GetComponent<SpriteRenderer>().sprite = spriteAfterInteraction;
    }

    public bool IsActive()
    {
        return isActive;
    }

    public void OnBossfightEnd()
    {
        isActive = false;
    }

    public void OnBossfightStart()
    {
        //isActive = false;
    }

    public void OnLoadAfterBossfight()
    {
        isActive = false;
    }

    public void OnLoadDuringBossfight()
    {
        isActive = false;
    }
}
