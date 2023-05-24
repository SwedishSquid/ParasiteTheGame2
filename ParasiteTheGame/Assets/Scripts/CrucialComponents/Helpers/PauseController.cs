
using UnityEngine;

public class PauseController 
{
    public static bool gameIsPaused;

    public void Pause ()
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