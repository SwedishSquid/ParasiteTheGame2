using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour, IInteractable
{
    [SerializeField] private string SceneNameToGoTo;

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
        ChangeLevel(interInfo.Player, interInfo.Enemy, interInfo.Item);
    }

    public bool IsActive()
    {
        return true;
    }
}
