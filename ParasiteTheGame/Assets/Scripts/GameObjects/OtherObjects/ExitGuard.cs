using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGuard : MonoBehaviour, IBossfightListener
{
    private readonly float scalingFactor = 4.2f;

    public void OnBossfightStart()
    {
        transform.localScale = new Vector2(scalingFactor, scalingFactor);
        GetComponent<CircleCollider2D>().enabled = true;
        GetComponent<SpriteRenderer>().sortingLayerName = "Player";
    }

    public void OnBossfightEnd()
    {
        StartCoroutine(Shrink());
    }

    IEnumerator Shrink()
    {
        Debug.Log("trying to shrink");
        for (int i = 0; i < 12; i++)
        {
            transform.localScale *= 0.8f;
            yield return new WaitForSeconds(0.1f);
        }
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().sortingLayerName = "Background";
    }

    public void OnLoadDuringBossfight()
    {
        transform.localScale = new Vector2(scalingFactor, scalingFactor);
        GetComponent<CircleCollider2D>().enabled = true;
        GetComponent<SpriteRenderer>().sortingLayerName = "Player";
    }

    public void OnLoadAfterBossfight()
    {
        //pass
    }
}
