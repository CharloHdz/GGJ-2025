using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class chr_GameManager : MonoBehaviour
{
    [Header("Singleton")]
    public static chr_GameManager Instance;
    [Header("Player")]
    [SerializeField] private Rigidbody2D PlayerRB;
    [SerializeField] public GameObject player;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float Distance;
    public float Score;
    public float HighScore;

    [Header("UI")]
    // GamePanels
    [SerializeField] private GameObject[] GamePanels;
    [SerializeField] private GameState GameState;
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private TextMeshProUGUI HighScoreText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerRB = GameObject.Find("Player").GetComponent<Rigidbody2D>();

        // Load High Score from PlayerPrefs
        HighScore = PlayerPrefs.GetFloat("HighScore", 0);
        UpdateHighScoreUI();

        ChangeGameState(GameState.Menu);
    }

    // Update is called once per frame
    void Update()
    {
        ScoreDistancia();

        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }

        if (PlayerRB.linearVelocity.y < -10)
        {
            PlayerRB.linearVelocity = new Vector2(PlayerRB.linearVelocity.x, -10);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameState == GameState.PlayGame || GameState == GameState.ResumeGame)
            {
                Pause();
            }
            else if (GameState == GameState.Pause)
            {
                Resume();
            }
        }
    }

    public void ScoreDistancia()
    {
        float currentY = player.transform.position.y;
        Distance = respawnPoint.position.y - currentY;
        Score = Mathf.Round(Distance);

        // Check and update High Score
        if (Score > HighScore)
        {
            HighScore = Score;
            PlayerPrefs.SetFloat("HighScore", HighScore);
            UpdateHighScoreUI();
        }

        ScoreText.text = "Depth: " + Score.ToString() + "m";
    }

    public void UpdateHighScoreUI()
    {
        HighScoreText.text = "High Score: " + HighScore.ToString() + "m";
    }

    public void Respawn()
    {
        player.transform.position = respawnPoint.position;
        PlayerRB.linearVelocity = new Vector2(0, 0);
    }

    // UI Buttons
    public void ReturnToMenu()
    {
        ChangeGameState(GameState.Menu);
    }

    public void Play()
    {
        ChangeGameState(GameState.PlayGame);
    }

    public void Pause()
    {
        ChangeGameState(GameState.Pause);
    }

    public void Resume()
    {
        ChangeGameState(GameState.ResumeGame);
    }

    public void Settings()
    {
        ChangeGameState(GameState.Settings);
    }

    public void GameOver()
    {
        ChangeGameState(GameState.GameOver);
    }

    public void DisableAllPanels()
    {
        foreach (GameObject panel in GamePanels)
        {
            panel.SetActive(false);
        }
    }

    // Game States
    public void ChangeGameState(GameState state)
    {
        GameState = state;
        switch (state)
        {
            case GameState.Menu:
                Time.timeScale = 1;
                SceneManager.LoadScene(0);
                break;
            case GameState.PlayGame:
                SceneManager.LoadScene(1);
                DisableAllPanels();
                GamePanels[0].SetActive(true);
                break;
            case GameState.Pause:
                Time.timeScale = 0;
                DisableAllPanels();
                GamePanels[1].SetActive(true);
                break;
            case GameState.ResumeGame:
                Time.timeScale = 1;
                DisableAllPanels();
                GamePanels[0].SetActive(true);
                break;
            case GameState.Settings:
                DisableAllPanels();
                GamePanels[2].SetActive(true);
                break;
            case GameState.GameOver:
                DisableAllPanels();
                GamePanels[3].SetActive(true);
                break;
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save(); // Ensure High Score is saved when the application closes
    }
}

public enum GameState
{
    Menu,
    PlayGame,
    ResumeGame,
    Pause,
    Settings,
    GameOver
}

