using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private GameState gameState;
    [SerializeField] PlayerController playerController;
    private InputHandler inputHandler;
    private InputInfo currentInputInfo;
    private PauseController pauseController;

    void Start()
    {
        gameState = GameState.MainGameMode;
        inputHandler = new InputHandler();
        pauseController = new PauseController();
    }

    void Update()
    {
        currentInputInfo = inputHandler.GetInputInfo();

        UpdateGameState();

        if (gameState == GameState.MainGameMode)
        {
            playerController.HandleUpdate(currentInputInfo);
        }

        if (Input.GetButtonDown("Interact"))
        {
            AttemptToChangeLevel();
        }

        if (Input.GetButtonDown("Pause")) 
        {
            pauseController.Pause();
        }
    }

    private void AttemptToChangeLevel()
    {
        var t =Physics2D.OverlapPoint(playerController.gameObject.transform.position, LayerConstants.LevelChangersLayer);
        if (t != null && t.gameObject.TryGetComponent<LevelChanger>(out var levelChanger))
        {
            ISavable enemy = null;
            ISavable item = null;
            if (playerController.controlled is AEnemy)
            {
                enemy = (playerController.controlled as AEnemy) as ISavable;
                item = (playerController.controlled as AEnemy).GetISavableItem();
            }

            if (enemy == null)
            {
                Debug.Log("null enemy somehow");
            }

            levelChanger.ChangeLevel(null, enemy, item);
        }
    }

    private void UpdateGameState()
    {
        gameState = GameState.MainGameMode;
    }
}
