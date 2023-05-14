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

    private GameData gameData;
    public static DataPersistenceManager instance { get; private set; }

    private List<ISavable> dataPersistenceObjects;
    private FileGameDataHandler gameDataHandler;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("another DataPersistenceManager on the scene detected");
        }
        instance = this;

        gameDataHandler = new FileGameDataHandler(Application.persistentDataPath, fileName);
    }

    private void Start()
    {
        
        
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        SaveGame();
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

        Debug.Log("loaded the game");
    }

    public void SaveGame()
    {
        foreach (var dataPersistence in dataPersistenceObjects)
        {
            dataPersistence.SaveGame(gameData);
        }
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