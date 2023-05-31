
using System;
using System.IO;
using UnityEngine;

public class FileGameDataHandler
{
    private static string path;

    public FileGameDataHandler(string pathToDir, string fileName)
    {
        path = Path.Combine(pathToDir, fileName);
    }

    public GameData Load()
    {
        string dataToLoad;

        GameData loadedData = null;
        if (File.Exists(path))
        {
            try
            {
                using (var stream = new FileStream(path, FileMode.Open))
                using (var reader = new StreamReader(stream))
                {
                    dataToLoad = reader.ReadToEnd();
                }
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch(Exception e)
            {
                Debug.LogError(e.Message + "occured during reading from file");
            }
        }

        return loadedData;
    }

    public void Save(GameData gameData)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            var dataToStore = JsonUtility.ToJson(gameData, true);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message + "occured while saving to file");
        }
    }

    /*public void SaveDefaultLevel(GameData gameData, string sceneName)
    {
        var pathToFile = Path.Combine("Assets", "DefaultValues", sceneName + ".json");
        
        var t = Directory.CreateDirectory(Path.GetDirectoryName(pathToFile));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(gameData.Levels);
            }
        }
    }*/

    public static bool CheckLoadFileExists()
    {
        return File.Exists(path);
    }
}