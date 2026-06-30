using UnityEngine;

public class ConveyorGameManager : GameManager
{
    [Header("Tap Game Settings")]
    [SerializeField] private float timeoutDuration = 2f;

    private float timeSinceLastTap;
    private float gameStartTime;

    EventBinding<GameOverEvent> gameOverBinding;

    private void OnEnable()
    {
        gameOverBinding = new EventBinding<GameOverEvent>(TriggerGameOver);
    }

    protected override void Awake()
    {
        base.Awake();
        InputManager.OnTap += HandleTap;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        InputManager.OnTap -= HandleTap;
    }

    protected override void Update()
    {
        base.Update();

        if (!IsPlaying) return;
    }

    private void HandleTap()
    {

    }

    protected override void OnGameStarted_Hook()
    {

    }

    protected override void OnRestart_Hook()
    {

    }

    protected override void OnGameOver_Hook()
    {

    }
}
