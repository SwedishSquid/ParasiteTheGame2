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

    public void MoveEnemyFromLevelToLevel(string senderLevelName, string recieverLevelName, string enemyGUID)
    {
        var sender = GetLevel(senderLevelName);
        if (!sender.Enemies.ContainsKey(enemyGUID))
        {
            Debug.LogError($"no enemy with GUID={enemyGUID} on level {senderLevelName}");
            return;
        }
        var reciever = GetLevel(recieverLevelName);
        if (reciever.Enemies.ContainsKey(enemyGUID))
        {
            Debug.LogError($"enemy with same GUID={enemyGUID} detected on recieving level {recieverLevelName}");
            return;
        }

        sender.RemovedEnemiesGUIDs.Add(enemyGUID);
        var enemyData = sender.Enemies[enemyGUID];
        sender.Enemies.Remove(enemyGUID);

        reciever.Enemies.Add(enemyGUID, enemyData);
    }

    public void MoveItemFromLevelToLevel(string senderLevelName, string recieverLevelName, string itemGUID)
    {
        var sender = GetLevel(senderLevelName);
        if (!sender.Items.ContainsKey(itemGUID))
        {
            Debug.LogError($"no item with GUID={itemGUID} found on sending level {senderLevelName}");
            return;
        }
        var reciever = GetLevel(recieverLevelName);
        if (reciever.Items.ContainsKey(itemGUID))
        {
            Debug.LogError($"item with same GUID={itemGUID} found on recieving level {recieverLevelName}");
        }

        sender.RemovedItemsGUIDs.Add(itemGUID);
        var itemData = sender.Items[itemGUID];
        sender.Items.Remove(itemGUID);

        reciever.Items.Add(itemGUID, itemData);
    }
}