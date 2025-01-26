using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class chr_ScoreManager : MonoBehaviour
{
    [SerializeField] private Rigidbody2D PlayerRB;
    [SerializeField] public GameObject player;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float Distance;
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private TextMeshProUGUI HighScoreText;
    public float Score;
    public float HighScore;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerRB = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        // Load High Score from PlayerPrefs
        HighScore = PlayerPrefs.GetFloat("HighScore", 0);
        //Buscar Score Text
        ScoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        HighScoreText = GameObject.Find("HighScoreText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        ScoreDistancia();
        if (PlayerRB.linearVelocity.y < -10)
        {
            PlayerRB.linearVelocity = new Vector2(PlayerRB.linearVelocity.x, -10);
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
        }

        ScoreText.text = "Depth: " + Score.ToString() + "m";
    }

    public void UpdateHighScore()
    {
        HighScoreText.text = "High Score: " + HighScore.ToString() + "m";
    }

    public static void Respawn()
    {
        SceneManager.LoadScene(1);
    }

}
