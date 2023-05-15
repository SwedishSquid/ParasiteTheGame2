using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript: MonoBehaviour
{
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;

    public void PlayPressed()
    {
        DisableAllButtons();
        DataPersistenceManager.Instance.SaveGame();
        SceneManager.LoadSceneAsync("LevelOne");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);//"SceneExample"); // change to scene "GAME_NAME"
    }

    public void ExitPressed()
    {
        Debug.Log("Exit pressed!"); 
        Application.Quit();
    }

    public void OnNewGamePressed()
    {
        DisableAllButtons();
        DataPersistenceManager.Instance.NewGame();
        DataPersistenceManager.Instance.SaveGame();
        SceneManager.LoadSceneAsync("LevelOne");
    }

    private void DisableAllButtons()
    {
        newGameButton.interactable = false;
        continueButton.interactable = false;
    }
}