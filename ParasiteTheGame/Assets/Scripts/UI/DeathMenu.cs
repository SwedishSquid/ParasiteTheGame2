using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathMenu: MonoBehaviour
{
    [SerializeField] private Canvas deathMenuCanvas;
    [SerializeField] private Button restart;
    [SerializeField] private Button mainMenu;

    public void StartDeathMenu()
    {
        deathMenuCanvas.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(restart.gameObject);
    }

    public void PressedRestart()
    {
        deathMenuCanvas.gameObject.SetActive(false);
        SceneManager.LoadSceneAsync(DataPersistenceManager.Instance.GameData.CurrentLevelName);
    }

    public void PressedMainMenu()
    {
        DisableAllButtons();
        deathMenuCanvas.gameObject.SetActive(false);
        SceneManager.LoadSceneAsync("MainMenu");
    }

    private void DisableAllButtons()
    {
        restart.interactable = false;
        mainMenu.interactable = false;
    }

}