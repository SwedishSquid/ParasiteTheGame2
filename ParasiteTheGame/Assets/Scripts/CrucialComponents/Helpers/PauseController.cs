
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public static bool gameIsPaused = false;

    void PauseGame ()
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