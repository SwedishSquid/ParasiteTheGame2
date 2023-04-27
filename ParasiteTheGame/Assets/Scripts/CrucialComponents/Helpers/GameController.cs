using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    private GameState gameState;
    [SerializeField] PlayerController playerController;
    private InputHandler inputHandler;
    private InputInfo currentInputInfo;
    void Start()
    {
        gameState = GameState.NotControlling;
        inputHandler = new InputHandler();
    }

    // Update is called once per frame
    void Update()
    {
        currentInputInfo = inputHandler.GetInputInfo();

        if (currentInputInfo.JumpoutPressed)
        {
            playerController.ActOnJumpout();
        }

        UpdateGameState();

        if (gameState == GameState.NotControlling)
        {
            playerController.HandleUpdate(currentInputInfo);
        }
        else
        {
            if (currentInputInfo.PickOrDropPressed)
            {
                playerController.controlled.ActOnPickOrDrop();
            }

            playerController.controlled.ControlledUpdate(currentInputInfo);
            playerController.controlled.UpdatePlayerPos(playerController.transform);
        }
    }

    private void UpdateGameState()
    {
        if (playerController.controlled is not null)
        {
            gameState = GameState.Controlling;
        }
        else
        {
            gameState = GameState.NotControlling;
        }
    }
}
