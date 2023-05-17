using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    [SerializeField] private string SceneNameToGoTo;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            return;
        }
        Debug.Log($"Press [E] to go to level {SceneNameToGoTo}");
    }

    public void ChangeLevel(ISavable player, ISavable enemy, ISavable item)
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
}
