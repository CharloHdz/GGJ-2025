using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class chr_ScoreManager : MonoBehaviour
{
    [SerializeField] private Rigidbody2D PlayerRB;
    [SerializeField] public GameObject player;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float Distance;
    public float Score;
    public float HighScore;

    public static chr_ScoreManager Instance { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerRB = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        // Load High Score from PlayerPrefs
        HighScore = PlayerPrefs.GetFloat("HighScore", 0);
    }

    // Update is called once per frame
    void Update()
    {
        ScoreDistancia();
        if (PlayerRB.linearVelocity.y < -10)
        {
            PlayerRB.linearVelocity = new Vector2(PlayerRB.linearVelocity.x, -10);
        }
        UpdateHighScore();

        chr_GameManager.Instance.Score = Score;
        chr_GameManager.Instance.HighScore = HighScore;
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

        chr_GameManager.Instance.ScoreText.text = Score.ToString() + "m";
    }

    public void UpdateHighScore()
    {
        chr_GameManager.Instance.HighScoreText.text = "HIGHSCORE: " + HighScore.ToString() + "m";
    }

    public static void Respawn()
    {
        SceneManager.LoadScene(1);
    }

}
