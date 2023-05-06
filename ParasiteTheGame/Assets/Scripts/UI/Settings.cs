using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSettings
{
    public void FullScreenToggle()
    {
        isFullScreen = !isFullScreen;
        Screen.fullScreen = isFullScreen;
    }
}
