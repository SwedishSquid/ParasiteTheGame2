using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour, IInteractable
{
    [SerializeField] private string SceneNameToGoTo;

    [SerializeField] bool moveObjects = true;

    private void ChangeLevel(ISavable player, ISavable enemy, ISavable item)
    {
        var currentSceneName = SceneManager.GetActiveScene().name;
        
        if (enemy != null)
        {
            DataPersistenceManager.Instance.GameData.MoveObjectReferenceFromLevelToLevel(currentSceneName, SceneNameToGoTo, enemy.GetGUID());
        }
        if (item != null)
        {
            DataPersistenceManager.Instance.GameData.MoveObjectReferenceFromLevelToLevel(currentSceneName, SceneNameToGoTo, item.GetGUID());
        }
        DataPersistenceManager.Instance.GameData.CurrentLevelName = SceneNameToGoTo;

        DataPersistenceManager.Instance.SaveGame();

        SceneManager.LoadSceneAsync(SceneNameToGoTo);
    }

    public void Interact(InteractorInfo interInfo)
    {
        if (moveObjects)
        {
            ChangeLevel(interInfo.Player, interInfo.Enemy, interInfo.Item);
        }
        else
        {
            DataPersistenceManager.Instance.GameData.CurrentLevelName = SceneNameToGoTo;
            DataPersistenceManager.Instance.SaveGame();
            SceneManager.LoadSceneAsync(SceneNameToGoTo);
        }
    }

    public bool IsActive()
    {
        return true;
    }
}
