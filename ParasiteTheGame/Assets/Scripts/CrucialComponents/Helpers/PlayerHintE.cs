using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHintE : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool frozen = false;

    private void Start()
    {
        spriteRenderer= GetComponent<SpriteRenderer>();
        spriteRenderer.enabled= false;
    }

    public void ShowHint()
    {
        if (frozen) return;
        StartCoroutine(MakeAppear());
    }

    private IEnumerator MakeAppear()
    {
        frozen= true;
        spriteRenderer.enabled= true;
        yield return new WaitForSeconds(1);
        MakeDisappear();
    }

    private void MakeDisappear()
    {
        spriteRenderer.enabled= false;
        frozen= false;
    }
}
