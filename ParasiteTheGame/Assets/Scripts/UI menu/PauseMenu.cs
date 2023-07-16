using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu: AMenu
{
    [SerializeField] private Canvas pauseCanvas;
    [SerializeField] private Button continueGame;
    [SerializeField] private Button saveGame;
    [SerializeField] private Button settings;
    [SerializeField] private Button mainMenu;

    public void PressedPause()
    {
        if (!PauseController.gameIsPaused)
        {
            pauseCanvas.gameObject.SetActive(false);
            return;
        }
        pauseCanvas.gameObject.SetActive(true);
        OnActive();
    }

    public void PressedContinue()
    {
        PauseController.Pause();
        PressedPause();
    }

    public void PressedSaveGame()
    {
        if (DataPersistenceManager.Instance != null)
        {
            DataPersistenceManager.Instance.SaveGame();
        }
    }

    public void PressedMainMenu()
    {
        DisableAllButtons();
        pauseCanvas.gameObject.SetActive(false);
        PauseController.Pause();
        PressedSaveGame();
        SceneManager.LoadSceneAsync("MainMenu");
    }

    private void DisableAllButtons()
    {
        continueGame.interactable = false;
        saveGame.interactable = false;
        settings.interactable = false;
        mainMenu.interactable = false;
    }

    public override void OnActive()
    {
        EventSystem.current.SetSelectedGameObject(continueGame.gameObject);
    }
    
}