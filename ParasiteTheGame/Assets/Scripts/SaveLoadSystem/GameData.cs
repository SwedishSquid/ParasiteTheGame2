using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string CurrentLevelName = "LevelOne";    //should be menu or something else - changed when level changes

    public PlayerData PlayerInfo = new PlayerData();

    public SerializableDictionary<string, LevelData> Levels = new SerializableDictionary<string, LevelData>();

    public EnemyData GetEnemyOnSceneByGUID(string sceneName, string GUID)
    {
        var level = GetLevel(sceneName);

        if (!level.Enemies.ContainsKey(GUID))
        {
            Debug.LogError($"Enemy with GUID {GUID} not found on scene {sceneName}");
            return null;
        }

        return level.Enemies[GUID];
    }

    public bool TryGetEnemyOnLevelByGUID(string levelName, string GUID, out EnemyData enemyData)
    {
        var level = GetLevel(levelName);

        if (!level.Enemies.ContainsKey(GUID))
        {
            level.Enemies.Add(GUID, new EnemyData());
            enemyData = level.Enemies[GUID];
            return false;
        }

        enemyData = level.Enemies[GUID];
        return true;
    }

    public ItemData GetItemOnSceneByGUID(string sceneName, string GUID)
    {
        var level = GetLevel(sceneName);

        

        if (!level.Items.ContainsKey(GUID))
        {
            Debug.LogError($"Enemy with GUID {GUID} not found on scene {sceneName}");
            return null;
        }

        return level.Items[GUID];
    }

    public bool TryGetItemOnLevelByGUID(string levelName, string GUID, out ItemData itemData)
    {
        var level = GetLevel(levelName);

        if (!level.Items.ContainsKey(GUID))
        {
            level.Items.Add(GUID, new ItemData());
            itemData = level.Items[GUID];
            return false;
        }

        itemData = level.Items[GUID];
        return true;
    }

    public LevelData GetLevel(string sceneName)
    {
        if (!Levels.ContainsKey(sceneName))
        {
            Levels.Add(sceneName, new LevelData());
        }
        return Levels[sceneName];
    }
}