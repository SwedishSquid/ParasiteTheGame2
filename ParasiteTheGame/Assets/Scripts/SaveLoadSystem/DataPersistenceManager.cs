using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    private string fileName = "gameData";
    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileGameDataHandler gameDataHandler;

    private void Start()
    {
        gameDataHandler = new FileGameDataHandler(Application.persistentDataPath, fileName);
        dataPersistenceObjects = FindAllDataPersistenceObjects();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = gameDataHandler.Load();
        foreach (var dataPersistence in dataPersistenceObjects)
        {
            dataPersistence.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach (var dataPersistence in dataPersistenceObjects)
        {
            dataPersistence.SaveGame(gameData);
        }
        gameDataHandler.Save(gameData);
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        return new List<IDataPersistence>(FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>());
    }
}