
using UnityEngine;

public static class PauseController 
{
    public static bool gameIsPaused;

    public static void Pause()
    {
        gameIsPaused = !gameIsPaused;
        if(gameIsPaused)
        {
            Time.timeScale = 0f;
        }
        else 
        {
            Time.timeScale = 1;
        }
    }
}