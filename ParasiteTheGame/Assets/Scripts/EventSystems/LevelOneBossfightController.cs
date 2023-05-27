using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelOneBossfightController : MonoBehaviour, ISavable
{
    [SerializeField] protected string id;

    [ContextMenu("Generate GUID for id")]
    protected void GenerateGUID()
    {
        id = System.Guid.NewGuid().ToString();
    }

    [SerializeField] private List<MonoBehaviour> listeners;

    [NonSerialized]
    public BossfightState bossfightState = BossfightState.NotStarted;

    public static LevelOneBossfightController Instance;

    void Start()
    {
        Instance = this;
    }

    void OnDisable()
    {
        Instance = null;
    }

    public void StartBossfight()
    {
        if (bossfightState == BossfightState.NotStarted)
        {
            foreach (var listener in listeners)
            {
                if (listener == null) continue;
                (listener as IBossfightListener).OnBossfightStart();
            }
            bossfightState = BossfightState.Continued;
        }
    }

    public void EndBossfight()
    {
        if (bossfightState == BossfightState.Continued)
        {
            foreach (var listener in listeners)
            {
                if (listener == null) continue;
                (listener as IBossfightListener).OnBossfightEnd();
            }
            bossfightState= BossfightState.Finished;
        }
    }

    public void NotifyListenersAfterLoad()
    {
        if (bossfightState == BossfightState.Continued)
        {
            foreach (var listener in listeners)
            {
                if (listener == null) continue;
                (listener as IBossfightListener).OnLoadDuringBossfight();
            }
        }
        else if (bossfightState == BossfightState.Finished)
        {
            foreach (var listener in listeners)
            {
                if (listener == null) continue;
                (listener as IBossfightListener).OnLoadAfterBossfight();
            }
        }
    }

    public void AfterAllObjectsLoaded(GameData gameData)
    {
        NotifyListenersAfterLoad();
    }

    public void DestroyIt()
    {
        //nope
    }

    public string GetGUID()
    {
        if (id == null || id == "")
        {
            Debug.LogError($"id for {this} not set");
        }
        return id;
    }

    public void LoadData(GameData gameData)
    {
        var objects = gameData.BossFightControllers;
        if (!objects.ContainsKey(id))
        {
            return;
        }
        var data = objects[id];
        bossfightState = data.bossfightState;
    }

    public void SaveGame(GameData gameData)
    {
        var objects = gameData.BossFightControllers;
        if (!objects.ContainsKey(id))
        {
            objects.Add(id, new BossfightData());
        }
        
        (objects[id] as BossfightData).bossfightState = bossfightState;
    }

    public void SetGUID(string GUID)
    {
        throw new System.NotImplementedException();
    }

    public void SetPosition(Vector2 position)
    {
        throw new System.NotImplementedException();
    }
}
