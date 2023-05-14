using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript: MonoBehaviour
{
    public void PlayPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);//"SceneExample"); // change to scene "GAME_NAME"
    }

    public void ExitPressed()
    {
        Debug.Log("Exit pressed!"); 
        Application.Quit();
    }
}