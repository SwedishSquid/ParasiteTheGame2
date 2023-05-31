using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript: MonoBehaviour
{
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button settingsBackButton;
    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        LoadSettings();
        UpdateContinueButton();
    }

    public void PlayPressed()
    {
        DisableAllButtons();
        DataPersistenceManager.Instance.SaveGame();
        SceneManager.LoadSceneAsync(DataPersistenceManager.Instance.GameData.CurrentLevelName);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);//"SceneExample"); // change to scene "GAME_NAME"
    }

    public void SettingsPressed()
    {
        EventSystem.current.SetSelectedGameObject(settingsBackButton.gameObject);
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
        SceneManager.LoadSceneAsync("Education");
    }

    private void DisableAllButtons()
    {
        newGameButton.interactable = false;
        continueButton.interactable = false;
    }

    private void LoadSettings()
    {
        fullScreenToggle.isOn = PlayerPrefs.GetInt("screenSize", 1) == 1;
        volumeSlider.value = PlayerPrefs.GetFloat("settingsVolume", 0.5f);
    }

    public void UpdateContinueButton()
    {
        continueButton.gameObject.SetActive(FileGameDataHandler.CheckLoadFileExists());
        EventSystem.current.SetSelectedGameObject(FileGameDataHandler.CheckLoadFileExists()
            ? continueButton.gameObject
            : newGameButton.gameObject);
    }
}