using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenBossfightTrigger : MonoBehaviour, IInteractable, IBossfightListener
{
    private bool isActive = true;

    [SerializeField] private Sprite spriteAfterInteraction;
    [SerializeField] private Kitchener boss;

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
        if (BossfightController.Instance == null)
        {
            return;
        }

        if (BossfightController.Instance.BossfightState == BossfightState.NotStarted)
        {
            BossfightController.Instance.StartBossfight();
            BringTheBoss();
            Debug.Log("starting bossfight");
        }
        else
        {
            Debug.Log("ending bossfight");
            BossfightController.Instance.EndBossfight();
        }

        ChangeSprite();
    }

    private void BringTheBoss()
    {
        boss.gameObject.transform.position = gameObject.transform.position + Vector3.left * 7;
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
        isActive = false;
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
