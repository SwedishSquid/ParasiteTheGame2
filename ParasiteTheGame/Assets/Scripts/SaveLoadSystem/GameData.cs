using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameData
{
    public SerializableDictionary<string, EnemyData> Enemies = new SerializableDictionary<string, EnemyData>();

    public SerializableDictionary<string, ItemData> Items = new SerializableDictionary<string, ItemData>();

    public string CurrentLevelName = "level nope";

    public PlayerData PlayerInfo = new PlayerData();

    public SerializableDictionary<string, LevelData> Levels = new SerializableDictionary<string, LevelData>();

    public SerializableDictionary<string, BossfightData> BossFightControllers = new();

    public void SetPlayerPosition(Vector2 newPosition, string sceneName)
    {
        var level = GetLevel(sceneName);
        level.PlayerPosition = newPosition;
        level.IsPlayerPosInitialised = true;
    }

    public LevelData GetLevel(string sceneName)
    {
        if (!Levels.ContainsKey(sceneName))
        {
            Levels.Add(sceneName, new LevelData());
        }
        return Levels[sceneName];
    }

    public ItemData GetItemToSave(string GUID)
    {
        if (!Items.ContainsKey(GUID))
        {
            Items.Add(GUID, new ItemData());
        }
        return Items[GUID];
    }

    public EnemyData GetEnemyToSave(string GUID)
    {
        if (!Enemies.ContainsKey(GUID))
        {
            Enemies.Add(GUID, new EnemyData());
        }
        return Enemies[GUID];
    }

    public void MoveObjectReferenceFromLevelToLevel(string senderLevelName, string recieverLevelName, string GUID)
    {
        Debug.Log($"moving object from {senderLevelName} to {recieverLevelName} the {GUID}");

        var sender = GetLevel(senderLevelName);
        
        var reciever = GetLevel(recieverLevelName);

        if (sender.AddedGUIDs.Contains(GUID))
        {
            sender.AddedGUIDs.Remove(GUID);
        }
        else
        {
            sender.RemovedGUIDs.Add(GUID);
        }

        if (reciever.RemovedGUIDs.Contains(GUID))
        {
            reciever.RemovedGUIDs.Remove(GUID);
        }
        else
        {
            reciever.AddedGUIDs.Add(GUID);
        }
    }

    public bool PlayerDead => PlayerInfo.Health <= 0;
}