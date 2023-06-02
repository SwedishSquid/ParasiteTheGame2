using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextPlateManager : MonoBehaviour, IBossfightListener
{
    [SerializeField] private bool showBeforeBossfight = true;
    //[SerializeField] private bool showDuringBossfight = true;
    [SerializeField] private bool showAfterBossfight = true;

    private bool bossfightTookPlace = false;

    private TextMeshProUGUI textMesh;

    private TextMeshProUGUI textMeshPro { 
        get 
        {
            if (textMesh == null)
            {
                textMesh = GetComponent<TextMeshProUGUI>();
            }
            return textMesh;
        } 
    }
    void Start()
    {
        textMeshPro.alpha = 0;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        HandleTrigger(col.gameObject);
    }

    private void HandleTrigger(GameObject obj)
    {
        if (bossfightTookPlace && !showAfterBossfight 
            || !bossfightTookPlace && !showBeforeBossfight)
        {
            return;
        }

        if (obj.layer == LayerConstants.PlayerLayer)
        {
            StartCoroutine(MakeEffects());
            Destroy(gameObject.GetComponent<Collider2D>());
        }

        if (obj.layer == LayerConstants.ControllablesLayerNum)
        {
            var enemy = obj.GetComponent<AEnemy>();
            if (enemy is not null && enemy.IsCaptured)
            {
                StartCoroutine(MakeEffects());
                Destroy(gameObject.GetComponent<Collider2D>());
            }
        }
    }

    private IEnumerator MakeEffects()
    {
        for (float i = 0.05f; i <= 1; i += 0.05f)
        {
            textMeshPro.alpha = i;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void OnBossfightStart()
    {
        AttemptToRemoveDuringBossfight();
    }

    public void OnLoadDuringBossfight()
    {
        AttemptToRemoveDuringBossfight();
    }

    public void OnBossfightEnd()
    {
        AttemptToRemoveAfterBossfight();
    }

    public void OnLoadAfterBossfight()
    {
        AttemptToRemoveAfterBossfight();
    }

    private void AttemptToRemoveAfterBossfight()
    {
        bossfightTookPlace = true;
        if (bossfightTookPlace && !showAfterBossfight)
        {
            textMeshPro.alpha = 0;
        }
    }

    private void AttemptToRemoveDuringBossfight()
    {
        if (!showAfterBossfight)
        {
            textMeshPro.alpha = 0;
        }
    }
}
