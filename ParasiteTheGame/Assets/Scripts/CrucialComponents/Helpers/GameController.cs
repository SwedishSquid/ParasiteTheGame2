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

        if (gameState == GameState.MainGameMode && !PauseController.gameIsPaused)
        {
            playerController.HandleUpdate(currentInputInfo);
        }

        if (!PauseController.gameIsPaused)
        {
            AttemptToInteract();
        }

        if (Input.GetButtonDown("Pause"))
        {
            pauseController.Pause();
        }
    }

    /*    private void AttemptToChangeLevel()
        {
            var t =Physics2D.OverlapPoint(playerController.gameObject.transform.position, LayerConstants.InteractiveObjectsLayer);
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
        }*/

    private void AttemptToInteract()
    {
        var t = Physics2D.OverlapPoint(playerController.gameObject.transform.position, LayerConstants.InteractiveObjectsLayer);
        if (t == null)
        {
            return;
        }

        if (!t.gameObject.TryGetComponent<IInteractable>(out var interactable))
        {
            return;
        }

        if (!interactable.IsActive())
        {
            return;
        }

        Debug.Log("before show");
        playerController.ShowHintE();

        if (!Input.GetButtonDown("Interact"))
        {
            return;
        }

        interactable.Interact(MakeInteractorInfo());
    }

    private InteractorInfo MakeInteractorInfo()
    {
        ISavable enemy = null;
        ISavable item = null;
        if (playerController.controlled is AEnemy)
        {
            enemy = (playerController.controlled as AEnemy) as ISavable;
            item = (playerController.controlled as AEnemy).GetISavableItem();
        }
        return new InteractorInfo(playerController, enemy, item);
    }

    private void UpdateGameState()
    {
        gameState = GameState.MainGameMode;
    }
}
