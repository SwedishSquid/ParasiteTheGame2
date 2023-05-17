using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public Vector2 PlayerPosition;

    public SerializableHashset<string> RemovedGUIDs = new SerializableHashset<string>();

    public SerializableHashset<string> AddedGUIDs = new SerializableHashset<string>();
}
