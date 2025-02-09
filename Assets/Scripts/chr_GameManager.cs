using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using BubbleAbyssLB;
using System.Runtime.CompilerServices;

public class chr_GameManager : MonoBehaviour
{
    // Singleton
    public static chr_GameManager Instance { get; private set; }

    [Header("UI")]
    // GamePanels
    [SerializeField] private GameObject[] GamePanels;
    public GameState GameState;
    public float Score;
    public TextMeshProUGUI ScoreText;
    public float HighScore;
    public TextMeshProUGUI HighScoreText;
    [SerializeField] private chr_ScoreManager ScoreManager;
    public Idiomas Idioma;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DisableAllPanels();
        ChangeGameState(GameState.Menu);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print("Escape");
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
    // UI Buttons
    public void ReturnToMenu()
    {
        ChangeGameState(GameState.Menu);
        SceneManager.LoadScene(0);
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

    public void SettingsOut()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            DisableAllPanels();
            ChangeGameState(GameState.Menu);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            ChangeGameState(GameState.Pause);
            DisableAllPanels();
            GamePanels[2].SetActive(true);
        }
    }

    public void GameOver()
    {
        ChangeGameState(GameState.GameOver);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void DisableAllPanels()
    {
        foreach (GameObject panel in GamePanels)
        {
            panel.SetActive(false);
        }
    }

    public void CambiarIdioma()
    {
        if (Idioma == Idiomas.Español)
        {
            Idioma = Idiomas.Ingles;
        }
        else
        {
            Idioma = Idiomas.Español;
        }
    }

    // Game States
    public void ChangeGameState(GameState state)
    {
        GameState = state;

        switch (state)
        {
            case GameState.Menu:
                DisableAllPanels();
                Time.timeScale = 1;
                GamePanels[0].SetActive(true);
                break;
            case GameState.PlayGame:
                SceneManager.LoadScene(1);
                DisableAllPanels();
                GamePanels[1].SetActive(true);
                break;
            case GameState.Pause:
                Time.timeScale = 0;
                DisableAllPanels();
                GamePanels[2].SetActive(true);
                break;
            case GameState.ResumeGame:
                Time.timeScale = 1;
                DisableAllPanels();
                GamePanels[1].SetActive(true);
                break;
            case GameState.Settings:
                DisableAllPanels();
                GamePanels[3].SetActive(true);
                print("Mondongo");
                break;
            case GameState.GameOver:
                DisableAllPanels();
                GamePanels[4].SetActive(true);
                break;
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
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

public enum Idiomas
{
    Español,
    Ingles
}



