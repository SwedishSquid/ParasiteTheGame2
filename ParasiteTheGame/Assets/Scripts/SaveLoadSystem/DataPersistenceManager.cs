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

    public GameData GameData { get; private set; }
    public static DataPersistenceManager Instance { get; private set; }

    private FileGameDataHandler gameDataHandler;

    private ObjectFactory objectFactory;

    private bool inMainMenu = true;

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

        objectFactory = GetComponent<ObjectFactory>();
        
        DontDestroyOnLoad(this.gameObject);

        GameData = gameDataHandler.Load();
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
        if (inMainMenu)
        {
            inMainMenu = false;
            return;
        }

        LoadGame();
    }

    public void NewGame()
    {
        GameData = new GameData();
    }

    public void LoadGame()
    {
        GameData = gameDataHandler.Load();
        
        if (GameData == null)
        {
            Debug.Log("found no data to load. loading default");
            NewGame();
        }

        var dataPersistenceObjects = GetAllDataPersistenceObjects();

        foreach (var gameObject in dataPersistenceObjects)
        {
            gameObject.LoadData(GameData);
            Debug.Log($"loading object {gameObject}");
        }

        foreach (var gameObject in dataPersistenceObjects)
        {
            Debug.Log($"updating object {gameObject}");
            gameObject.AfterAllObjectsLoaded(GameData);
        }

        Debug.Log("loaded the game");
    }

    public void SaveGame()
    {
        var dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISavable>().ToList();
        foreach (var dataPersistence in dataPersistenceObjects)
        {
            dataPersistence.SaveGame(GameData);
        }

        if (GameData == null)
        {
            Debug.LogError("saving null gamedata");
        }

        gameDataHandler.Save(GameData);

        Debug.Log("Saved the game");
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISavable> GetAllDataPersistenceObjects()
    {
        var savablesOnScene = FindObjectsOfType<MonoBehaviour>().OfType<ISavable>().ToList();

        var added = new List<ISavable>();

        var level = GameData.GetLevel(GameData.CurrentLevelName);

        foreach (var GUID in level.AddedGUIDs)
        {
            var typeName = GetTypeNameOfObject(GUID);
            if (typeName != null)
            {
                var inst = objectFactory.GenerateGameObjectByName(typeName, GUID) as ISavable;
                if (inst != null)
                {
                    added.Add(inst);
                }
                else
                {
                    Debug.LogError("something went horribly wrong :O");
                }
            }
        }

        var result = added.ToList();
        foreach (var savable in savablesOnScene)
        {
            if (level.RemovedGUIDs.Contains(savable.GetGUID()))
            {
                savable.DestroyIt();
            }
            else
            {
                result.Add(savable);
            }
        }

        return result;
    }

    private string GetTypeNameOfObject(string GUID)
    {
        if (GameData.Items.ContainsKey(GUID))
        {
            return GameData.Items[GUID].TypeName;
        }
        if (GameData.Enemies.ContainsKey(GUID))
        {
            return GameData.Enemies[GUID].TypeName;
        }

        Debug.LogError($"no object with GUID = {GUID} found in saved files");
        return null;
    }

    /*private List<ISavable> FindAllDataPersistenceObjects()
    {
        if (GameData == null)
        {
            return FindObjectsOfType<MonoBehaviour>().OfType<ISavable>().ToList();
        }

        var savablesInitiallyOnScene = FindObjectsOfType<MonoBehaviour>()
            .Where(o => o is ISavable)
            .ToHashSet();
        var level = GameData.GetLevel(GameData.CurrentLevelName);
        var objectsToRemove = savablesInitiallyOnScene.Where(s => level.RemovedItemsGUIDs.Contains((s as ISavable).GetGUID())
                                || level.RemovedEnemiesGUIDs.Contains((s as ISavable).GetGUID()));
        savablesInitiallyOnScene.RemoveWhere(s => objectsToRemove.Contains(s));
        foreach (var obj in objectsToRemove)
        {
            Destroy(obj);
        }

        var presentGUIDsOnLevel = savablesInitiallyOnScene
            .Select(o => (o as ISavable).GetGUID())
            .ToHashSet();

        var objectsWithData = level.Items
            .Select(kv => (kv.Key, kv.Value.TypeName))
            .Concat(level.Enemies
                .Select(s => (s.Key, s.Value.TypeName)));

        var savablesToLoad = savablesInitiallyOnScene
            .OfType<ISavable>()
            .ToList();

        Debug.Log(objectsWithData.Where(p => !presentGUIDsOnLevel.Contains(p.Key)).Count());
        foreach (var objInf in objectsWithData.Where(p => !presentGUIDsOnLevel.Contains(p.Key)))
        {
            var factory = FindFirstObjectByType<ObjectFactory>();
            Debug.Log(factory);
            Debug.Log(objInf);
            var createdObj = factory.GenerateGameObjectByName(objInf.TypeName);
            
            if (createdObj != null && createdObj.TryGetComponent<ISavable>(out var newSavable))
            {
                newSavable.SetGUID(objInf.Key);
                savablesToLoad.Add(newSavable);
            }
        }

        return savablesToLoad;
    }*/
}