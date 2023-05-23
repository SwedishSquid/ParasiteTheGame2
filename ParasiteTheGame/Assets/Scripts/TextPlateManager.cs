using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextPlateManager : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        textMeshPro.alpha = 0;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        HandleTrigger(col.gameObject);
    }

    private void HandleTrigger(GameObject obj)
    {
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
}
