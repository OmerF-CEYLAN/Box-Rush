// ═══════════════════════════════════════════════════════
//  GameUI.cs  —  REUSABLE. Wire up in Inspector per game.
//  Subscribes to game events via EventBus.
//  No direct reference to GameManager needed anywhere.
// ═══════════════════════════════════════════════════════

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Experimental.GraphView;

public class GameUI : MonoBehaviour
{
    // ───────────────────────────────────────────────────
    //  Inspector References
    // ───────────────────────────────────────────────────
    [Header("Panels")]
    [SerializeField] private GameObject idlePanel;
    [SerializeField] private GameObject hudPanel;
    [SerializeField] private GameObject gameOverPanel;

    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Game Over Screen")]
    [SerializeField] private TextMeshProUGUI gameOverScoreText;
    [SerializeField] private TextMeshProUGUI gameOverHighScoreText;
    [SerializeField] private GameObject newRecordObject;

    [Header("Idle Screen")]
    [SerializeField] private TextMeshProUGUI idleHighScoreText;

    [Header("Buttons")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;


    private int cachedHighScore;

    private EventBinding<GameStartedEvent> gameStartedBinding;
    private EventBinding<GameOverEvent> gameOverBinding;
    private EventBinding<ScoreChangedEvent> scoreChangedBinding;
    private EventBinding<NewHighScoreEvent> newHighScoreBinding;

    private void OnEnable()
    {
        gameStartedBinding = new EventBinding<GameStartedEvent>(HandleGameStarted);
        gameOverBinding = new EventBinding<GameOverEvent>(HandleGameOver);
        scoreChangedBinding = new EventBinding<ScoreChangedEvent>(HandleScoreChanged);
        newHighScoreBinding = new EventBinding<NewHighScoreEvent>(HandleNewHighScore);

        EventBus<GameStartedEvent>.Subscribe(gameStartedBinding);
        EventBus<GameOverEvent>.Subscribe(gameOverBinding);
        EventBus<ScoreChangedEvent>.Subscribe(scoreChangedBinding);
        EventBus<NewHighScoreEvent>.Subscribe(newHighScoreBinding);
    }

    private void OnDisable()
    {
        EventBus<GameStartedEvent>.Unsubscribe(gameStartedBinding);
        EventBus<GameOverEvent>.Unsubscribe(gameOverBinding);
        EventBus<ScoreChangedEvent>.Unsubscribe(scoreChangedBinding);
        EventBus<NewHighScoreEvent>.Unsubscribe(newHighScoreBinding);
    }

    private void Start()
    {
        restartButton?.onClick.AddListener(OnRestartClicked);

        leftButton?.onClick.AddListener(() => OnMoveClicked(Direction.Left));
        rightButton?.onClick.AddListener(() => OnMoveClicked(Direction.Right));

        // Seed the cached high score from GameManager if available,
        // otherwise it will be updated by the first NewHighScoreEvent.
        if (GameManager.Instance != null)
            cachedHighScore = GameManager.Instance.HighScore;

        ShowIdle();
    }

    void OnMoveClicked(Direction direction)
    {
        EventBus<MoveClickedEvent>.Publish(new MoveClickedEvent { direction = direction });
    }

    private void HandleGameStarted()
    {
        idlePanel?.SetActive(false);
        gameOverPanel?.SetActive(false);
        hudPanel?.SetActive(true);
        newRecordObject?.SetActive(false);
        SetText(scoreText, "0");
    }

    private void HandleGameOver(GameOverEvent e)
    {
        // Cache the latest high score from the event (no GameManager reference needed)
        cachedHighScore = e.highScore;

        hudPanel?.SetActive(false);
        gameOverPanel?.SetActive(true);

        SetText(gameOverScoreText, Mathf.FloorToInt(e.finalScore).ToString());
        SetText(gameOverHighScoreText, e.highScore.ToString());
    }

    private void HandleScoreChanged(ScoreChangedEvent e)
    {
        SetText(scoreText, Mathf.FloorToInt(e.currentScore).ToString());
    }

    private void HandleNewHighScore(NewHighScoreEvent e)
    {
        cachedHighScore = e.highScore;
        newRecordObject?.SetActive(true);
        SetText(gameOverHighScoreText, e.highScore.ToString());
    }

    // ───────────────────────────────────────────────────
    //  Restart Button
    // ───────────────────────────────────────────────────
    private void OnRestartClicked()
    {
        // GameStartedEvent will fire and HandleGameStarted() will
        // switch panels automatically — no manual panel work needed here.
        GameManager.Instance?.RestartGame();
    }

    // ───────────────────────────────────────────────────
    //  Helpers
    // ───────────────────────────────────────────────────
    private void ShowIdle()
    {
        idlePanel?.SetActive(true);
        hudPanel?.SetActive(false);
        gameOverPanel?.SetActive(false);
        SetText(idleHighScoreText, $"{cachedHighScore}");
    }

    private static void SetText(TextMeshProUGUI label, string value)
    {
        if (label != null) label.text = value;
    }
}