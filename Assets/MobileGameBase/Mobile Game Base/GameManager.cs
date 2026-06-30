// ═══════════════════════════════════════════════════════
//  GameManager.cs  —  REUSABLE BASE. NEVER MODIFY.
//  Subclass this per game. See TapGameManager.cs.
//  Publishes via EventBus. Subscribe in any system
//  using EventBinding<T> — no direct references needed.
// ═══════════════════════════════════════════════════════

using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ───────────────────────────────────────────────────
    //  Singleton
    // ───────────────────────────────────────────────────
    public static GameManager Instance { get; private set; }

    // ───────────────────────────────────────────────────
    //  State
    // ───────────────────────────────────────────────────
    public GameState CurrentState { get; private set; } = GameState.Idle;
    public bool IsPlaying => CurrentState == GameState.Playing;

    // ───────────────────────────────────────────────────
    //  Score
    // ───────────────────────────────────────────────────
    public float CurrentScore { get; private set; }
    public int HighScore { get; private set; }
    protected virtual string HighScoreKey => "HighScore";

    // ───────────────────────────────────────────────────
    //  Input binding — tap starts the game from Idle
    // ───────────────────────────────────────────────────
    private EventBinding<TapEvent> tapBinding;

    // ───────────────────────────────────────────────────
    //  Unity Lifecycle
    // ───────────────────────────────────────────────────
    protected virtual void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadHighScore();

        tapBinding = new EventBinding<TapEvent>(OnTapReceived);
        EventBus<TapEvent>.Subscribe(tapBinding);
    }

    protected virtual void OnDestroy()
    {
        EventBus<TapEvent>.Unsubscribe(tapBinding);
    }

    protected virtual void Update() { }

    // ───────────────────────────────────────────────────
    //  Tap-to-start  (runs in Idle only)
    // ───────────────────────────────────────────────────
    private void OnTapReceived()
    {
        if (CurrentState == GameState.Idle)
            StartGame();
    }

    // ───────────────────────────────────────────────────
    //  Public API
    // ───────────────────────────────────────────────────
    public void StartGame()
    {
        if (CurrentState == GameState.Playing) return;

        CurrentScore = 0f;
        CurrentState = GameState.Playing;

        EventBus<GameStartedEvent>.Publish(new GameStartedEvent());
        EventBus<ScoreChangedEvent>.Publish(new ScoreChangedEvent { currentScore = 0f });

        OnGameStarted_Hook();
    }

    /// <summary>
    /// Call this the moment the player fails.
    /// Any script can call it: collision, timer, obstacle, etc.
    /// </summary>
    public void TriggerGameOver()
    {
        if (CurrentState != GameState.Playing) return;

        CurrentState = GameState.GameOver;

        bool newRecord = TrySaveHighScore();

        // highScore is now embedded in the event so subscribers
        // don't need a direct reference to GameManager.
        EventBus<GameOverEvent>.Publish(new GameOverEvent
        {
            finalScore = CurrentScore,
            highScore = HighScore
        });

        if (newRecord)
            EventBus<NewHighScoreEvent>.Publish(new NewHighScoreEvent { highScore = HighScore });

        OnGameOver_Hook();
    }

    /// <summary>Adds to the current score. Does nothing if not playing.</summary>
    public void AddScore(float amount)
    {
        if (!IsPlaying) return;
        CurrentScore += amount;

        EventBus<ScoreChangedEvent>.Publish(new ScoreChangedEvent { currentScore = CurrentScore });
        OnScoreChanged_Hook(CurrentScore);
    }

    /// <summary>Sets the score to an exact value. Useful for distance-based scoring.</summary>
    public void SetScore(float value)
    {
        if (!IsPlaying) return;
        CurrentScore = value;

        EventBus<ScoreChangedEvent>.Publish(new ScoreChangedEvent { currentScore = CurrentScore });
        OnScoreChanged_Hook(CurrentScore);
    }

    /// <summary>
    /// Returns to Idle state and immediately starts a new game.
    /// Publishes GameStartedEvent — UI and audio react automatically.
    /// </summary>
    public void RestartGame()
    {
        CurrentState = GameState.Idle;
        OnRestart_Hook();
        StartGame();
    }

    /// <summary>Wipes the saved high score.</summary>
    public void ResetHighScore()
    {
        HighScore = 0;
        PlayerPrefs.DeleteKey(HighScoreKey);
        PlayerPrefs.Save();
    }

    // ───────────────────────────────────────────────────
    //  Override Hooks — use these in your subclass.
    //  Fire AFTER the EventBus event is published.
    // ───────────────────────────────────────────────────
    protected virtual void OnGameStarted_Hook() { }
    protected virtual void OnGameOver_Hook() { }
    protected virtual void OnScoreChanged_Hook(float score) { }
    protected virtual void OnRestart_Hook() { }

    // ───────────────────────────────────────────────────
    //  High Score
    // ───────────────────────────────────────────────────
    private bool TrySaveHighScore()
    {
        int rounded = Mathf.FloorToInt(CurrentScore);
        if (rounded <= HighScore) return false;
        HighScore = rounded;
        PlayerPrefs.SetInt(HighScoreKey, HighScore);
        PlayerPrefs.Save();
        return true;
    }

    private void LoadHighScore()
    {
        HighScore = PlayerPrefs.GetInt(HighScoreKey, 0);
    }
}