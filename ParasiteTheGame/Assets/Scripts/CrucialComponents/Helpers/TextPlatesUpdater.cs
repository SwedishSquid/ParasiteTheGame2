using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TextPlatesUpdater : MonoBehaviour, IBossfightListener
{
    List<TextPlateManager> list;
    private List<TextPlateManager> textPlates
    {
        get
        {
            if (list == null)
            {
                list = FindObjectsOfType<TextPlateManager>().ToList();
            }
            return list;
        }
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
