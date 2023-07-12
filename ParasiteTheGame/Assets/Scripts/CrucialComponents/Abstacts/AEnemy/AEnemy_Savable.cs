using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
//using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
//using UnityEditor.Sprites;
using UnityEngine;


public abstract partial class AEnemy : ISavable
{
    [SerializeField] protected string id;

    [ContextMenu("Generate GUID for id")]
    protected void GenerateGUID()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public virtual void SaveGame(GameData gameData)
    {
        var enemyData = gameData.GetEnemyToSave(id);

        enemyData.EnemyPosition = transform.position;
        enemyData.CanBeCaptured = CanBeCaptured;
        enemyData.PickedItemGUID = itemGUID;
        enemyData.Health = Health;

        enemyData.TypeName = this.GetType().Name;//typeName;
    }

    public virtual void LoadData(GameData gameData)
    {
        if (gameData.Enemies.ContainsKey(id))
        {
            var enemyData = gameData.Enemies[id];
            transform.position = enemyData.EnemyPosition;
            itemGUID = enemyData.PickedItemGUID;
            Health = enemyData.Health;

            Debug.Log("load something to enemy");

            enemyData.thisEnemy = this;
        }
        AfterDataLoaded();
        TryPassOut();
        TryDie();
    }

    protected virtual void AfterDataLoaded()
    {

    }

    public string GetGUID()
    {
        if (id == "")
        {
            Debug.LogError($"GUID for {this} is not set");
        }
        return id;
    }

    public void AfterAllObjectsLoaded(GameData gameData)
    {
        if (itemGUID == "")
        {
            return;
        }

        if (!gameData.Items.ContainsKey(itemGUID))
        {
            Debug.LogError($"cannot pick up item with GUID {itemGUID} - no such item found");
            return;
        }

        item = gameData.Items[itemGUID].thisItem;

        item?.OnPickUp(this);
    }

    public void SetGUID(string GUID)
    {
        id = GUID;
    }

    public void DestroyIt()
    {
        Destroy(gameObject);
    }



    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }
}

