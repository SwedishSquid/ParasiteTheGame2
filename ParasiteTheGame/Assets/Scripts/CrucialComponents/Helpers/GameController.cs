using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour, IBossfightListener
{
    private GameState gameState;
    [SerializeField] PlayerController playerController;
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private DeathMenu deathMenu;
    private InputHandler inputHandler;
    private InputInfo currentInputInfo;

    private Listener listener;
    [SerializeField] private AudioSource simpleMusic;
    [SerializeField] private AudioSource battleMusic;
    [SerializeField] private AudioSource envirMusic;

    void Start()
    {
        gameState = GameState.MainGameMode;
        inputHandler = new InputHandler();
        listener = new Listener(simpleMusic, envirMusic);
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            PauseController.Pause();
            pauseMenu.PressedPause();
        }

        if (playerController == null)
        {
            return;
        }

        currentInputInfo = inputHandler.GetInputInfo();

        if (gameState == GameState.PlayerDeathMode)
            return;
        
        UpdateGameState();

        if (gameState == GameState.MainGameMode && playerController != null)
        {
            playerController.HandleUpdate(currentInputInfo);
        }

        if (playerController.Dead)
        {
            gameState = GameState.PlayerDeathMode;
            deathMenu.StartDeathMenu();
        }

        if (!PauseController.gameIsPaused && playerController != null)
        {
            AttemptToInteract();
        }
    }

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

    public void OnBossfightStart()
    {
        listener.ChangeAudioSource(battleMusic);
    }

    public void OnLoadDuringBossfight()
    {
        listener.ChangeAudioSource(battleMusic);
    }

    public void OnBossfightEnd()
    {
        listener.ChangeAudioSource(simpleMusic);
    }

    public void OnLoadAfterBossfight()
    {
        //listener.ChangeAudioSource(simpleMusic);
    }
}
