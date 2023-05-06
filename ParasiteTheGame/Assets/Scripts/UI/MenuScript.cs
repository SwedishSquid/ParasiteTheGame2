using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript
{
    public void PlayPressed()
    {
        SceneManager.LoadScene("SceneExample"); // change to scene "GAME_NAME"
    }

    public void ExitPressed()
    {
        Debug.Log("Exit pressed!"); 
        Application.Quit();
    }
}