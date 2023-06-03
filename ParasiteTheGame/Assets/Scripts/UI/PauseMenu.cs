using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu: MonoBehaviour
{
    [SerializeField] private Canvas pauseCanvas;
    [SerializeField] private Button saveGame;
    [SerializeField] private Button mainMenu;

    public void PressedPause()
    {
        if (!PauseController.gameIsPaused)
        {
            pauseCanvas.gameObject.SetActive(false);
            return;
        }
        pauseCanvas.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(saveGame.gameObject);
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
        saveGame.interactable = false;
        mainMenu.interactable = false;
    }
    
}