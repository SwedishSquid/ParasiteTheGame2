

using System.IO;
using UnityEngine;

public class FileGameDataHandler
{
    private string path;

    public FileGameDataHandler(string pathToDir, string fileName)
    {
        path = Path.Combine(pathToDir, fileName);
    }

    public GameData Load()
    {
        string dataToLoad;
        using (var stream = new FileStream(path, FileMode.Open))
        {
            using (var reader = new StreamReader(stream))
            {
                dataToLoad = reader.ReadToEnd();
            }
        }

        var loadedData = JsonUtility.FromJson<GameData>(dataToLoad);

        return loadedData;
    }

    public void Save(GameData gameData)
    {
        var dataToStore = JsonUtility.ToJson(gameData, true);
        using (var stream = new FileStream(path, FileMode.Create))
        {
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(dataToStore);
            }
        }
    }
}