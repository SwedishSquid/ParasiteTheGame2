using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TextPlatesUpdater : MonoBehaviour, IBossfightListener
{
    private List<TextPlateManager> textPlates;

    // Start is called before the first frame update
    void Start()
    {
        textPlates = FindObjectsOfType<TextPlateManager>().ToList();
    }

    public void OnBossfightEnd()
    {
        foreach (var t in textPlates)
        {
            t.OnBossfightEnd();
        }
    }

    public void OnBossfightStart()
    {
        foreach (var t in textPlates)
        {
            t.OnBossfightStart();
        }
    }

    public void OnLoadAfterBossfight()
    {
        foreach (var t in textPlates)
        {
            t.OnLoadAfterBossfight();
        }
    }

    public void OnLoadDuringBossfight()
    {
        foreach (var t in textPlates)
        {
            t.OnLoadDuringBossfight();
        }
    }
}
