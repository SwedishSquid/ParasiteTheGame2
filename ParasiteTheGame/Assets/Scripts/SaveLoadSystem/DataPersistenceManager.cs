using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField]
    private string fileName;

    /*[Header("Enable to save qurrent items on scene. Run scene. Disable.")]
    public bool SetCurrentValuesAsDefault = false;*/


    private GameData gameData;
    public static DataPersistenceManager Instance { get; private set; }

    private List<ISavable> dataPersistenceObjects = new List<ISavable>();
    private FileGameDataHandler gameDataHandler;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("another DataPersistenceManager on the scene detected, creation of new one cancelled");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        gameDataHandler = new FileGameDataHandler(Application.persistentDataPath, fileName);
        
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        /*if (SetCurrentValuesAsDefault)
        {
            Debug.Log("Current save system mode is writing items new default position." +
                " Exit application now to save. Disable it after.");
            return;
        }*/
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = gameDataHandler.Load();
        

        
        if (gameData == null)
        {
            Debug.Log("found no data to load. loading default");
            NewGame();
        }

        foreach (var gameObject in dataPersistenceObjects)
        {
            gameObject.LoadData(gameData);
        }

        foreach (var gameObject in dataPersistenceObjects)
        {
            gameObject.AfterAllObjectsLoaded(gameData);
        }

        Debug.Log("loaded the game");
    }

    public void SaveGame()
    {
        foreach (var dataPersistence in dataPersistenceObjects)
        {
            dataPersistence.SaveGame(gameData);
        }

        /*if (SetCurrentValuesAsDefault)
        {
            gameDataHandler.SaveDefaultLevel(gameData, SceneManager.GetActiveScene().name);
            Debug.Log("saving new default values finished");
            return;
        }*/
        gameDataHandler.Save(gameData);

        Debug.Log("Saved the game");
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISavable> FindAllDataPersistenceObjects()
    {
        return FindObjectsOfType<MonoBehaviour>()
            .OfType<ISavable>()
            .ToList();
    }
}