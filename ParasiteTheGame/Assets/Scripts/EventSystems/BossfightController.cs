using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossfightController: MonoBehaviour, ISavable
{
    [SerializeField] protected string id;

    [ContextMenu("Generate GUID for id")]
    protected void GenerateGUID()
    {
        id = System.Guid.NewGuid().ToString();
    }

    [SerializeField] private AEnemy boss;

    [SerializeField] private List<MonoBehaviour> listeners;

    [NonSerialized]
    public BossfightState BossfightState = BossfightState.NotStarted;

    public static BossfightController Instance;

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
        if (BossfightState == BossfightState.NotStarted)
        {
            foreach (var listener in listeners)
            {
                if (listener == null) continue;
                (listener as IBossfightListener).OnBossfightStart();
            }
            BossfightState = BossfightState.Continued;
        }
    }

    public void EndBossfight()
    {
        if (BossfightState == BossfightState.Continued)
        {
            foreach (var listener in listeners)
            {
                if (listener == null) continue;
                (listener as IBossfightListener).OnBossfightEnd();
            }
            BossfightState = BossfightState.Finished;
        }
    }

    public void NotifyListenersAfterLoad()
    {
        if (BossfightState == BossfightState.Continued)
        {
            foreach (var listener in listeners)
            {
                if (listener == null) continue;
                (listener as IBossfightListener).OnLoadDuringBossfight();
            }
        }
        else if (BossfightState == BossfightState.Finished)
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

    public string GetBossGUID()
    {
        if (boss == null)
        {
            Debug.LogError($"boss is not set in BossfightController");
        }
        return boss.GetGUID();
    }

    public void LoadData(GameData gameData)
    {
        var objects = gameData.BossFightControllers;
        if (!objects.ContainsKey(id))
        {
            return;
        }
        var data = objects[id];
        BossfightState = data.bossfightState;
    }

    public void SaveGame(GameData gameData)
    {
        var objects = gameData.BossFightControllers;
        if (!objects.ContainsKey(id))
        {
            objects.Add(id, new BossfightData());
        }

        (objects[id] as BossfightData).bossfightState = BossfightState;
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