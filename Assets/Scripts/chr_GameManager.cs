using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class chr_GameManager : MonoBehaviour
{
    // Singleton
    public static chr_GameManager Instance {get; private set;}

    [Header("UI")]
    // GamePanels
    [SerializeField] private GameObject[] GamePanels;
    [SerializeField] private GameState GameState;
    [SerializeField] private TextMeshProUGUI HighScoreText;
    [SerializeField] private chr_ScoreManager ScoreManager;
    public Idiomas Idioma;

    // Singleton
    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
        
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
            DontDestroyOnLoad(gameObject);
        } 
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DisableAllPanels();
    }

    // Update is called once per frame
    void Update()
    {

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

    public void SettingsOut(){
        if(SceneManager.GetActiveScene().buildIndex == 0){
            ChangeGameState(GameState.Menu);
            DisableAllPanels();
        }else if(SceneManager.GetActiveScene().buildIndex == 1){
            ChangeGameState(GameState.PlayGame);
            DisableAllPanels();
            GamePanels[1].SetActive(true);
        }
    }

    public void GameOver()
    {
        ChangeGameState(GameState.GameOver);
    }

    public void QuitGame(){
        Application.Quit();
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
                GamePanels[1].SetActive(true);
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

