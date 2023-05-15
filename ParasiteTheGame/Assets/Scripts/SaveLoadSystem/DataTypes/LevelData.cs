using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public Vector2 PlayerPosition;

    public SerializableDictionary<string, EnemyData> Enemies = new SerializableDictionary<string, EnemyData>();
    public SerializableHashset<string> RemovedEnemiesGUIDs = new SerializableHashset<string>();

    public SerializableDictionary<string, ItemData> Items = new SerializableDictionary<string, ItemData>();
    public SerializableHashset<string> RemovedItemsGUIDs = new SerializableHashset<string>();
}
