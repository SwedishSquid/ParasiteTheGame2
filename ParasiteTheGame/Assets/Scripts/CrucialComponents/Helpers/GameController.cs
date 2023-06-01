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
    [SerializeField] private AudioClip simpleMusic;
    [SerializeField] private AudioClip battleMusic;

    void Start()
    {
        gameState = GameState.MainGameMode;
        inputHandler = new InputHandler();
        listener = new Listener(GetComponent<AudioSource>());
        listener.PutOnClip(simpleMusic);
    }

    void Update()
    {
        currentInputInfo = inputHandler.GetInputInfo();

        if (gameState == GameState.PlayerDeathMode)
            return;
        
        UpdateGameState();

        if (gameState == GameState.MainGameMode)
        {
            playerController.HandleUpdate(currentInputInfo);
        }

        if (playerController.Dead)
        {
            gameState = GameState.PlayerDeathMode;
            deathMenu.StartDeathMenu();
        }

        if (!PauseController.gameIsPaused)
        {
            AttemptToInteract();
        }

        if (Input.GetButtonDown("Pause"))
        {
            PauseController.Pause();
            pauseMenu.PressedPause();
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

    public void OnBossfightStart()
    {
        listener.PutOnClip(battleMusic);
    }

    public void OnLoadDuringBossfight() { }

    public void OnBossfightEnd()
    {
        listener.PutOnClip(simpleMusic);
    }

    public void OnLoadAfterBossfight() { }
}
