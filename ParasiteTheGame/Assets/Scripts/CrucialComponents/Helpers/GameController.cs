using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private GameState gameState;
    [SerializeField] PlayerController playerController;
    private InputHandler inputHandler;
    private InputInfo currentInputInfo;

    void Start()
    {
        gameState = GameState.MainGameMode;
        inputHandler = new InputHandler();
    }

    void Update()
    {
        currentInputInfo = inputHandler.GetInputInfo();

        UpdateGameState();

        if (gameState == GameState.MainGameMode)
        {
            playerController.HandleUpdate(currentInputInfo);
        }
    }

    private void UpdateGameState()
    {
        gameState = GameState.MainGameMode;
    }
}
