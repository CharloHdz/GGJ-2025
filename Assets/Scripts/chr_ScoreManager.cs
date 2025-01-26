using UnityEngine;
using TMPro;

public class chr_ScoreManager : MonoBehaviour
{
    [SerializeField] private Rigidbody2D PlayerRB;
    [SerializeField] public GameObject player;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float Distance;
    [SerializeField] private TextMeshProUGUI ScoreText;
    public float Score;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerRB = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        // Load High Score from PlayerPrefs
        chr_GameManager.Instance.HighScore = PlayerPrefs.GetFloat("HighScore", 0);
        //Buscar Score Text
        ScoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
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
        if (Score > chr_GameManager.Instance.HighScore)
        {
            chr_GameManager.Instance.HighScore = Score;
            PlayerPrefs.SetFloat("HighScore", chr_GameManager.Instance.HighScore);
            chr_GameManager.Instance.UpdateHighScoreUI();
        }

        ScoreText.text = "Depth: " + Score.ToString() + "m";
    }

    public void Respawn()
    {
        player.transform.position = respawnPoint.position;
        PlayerRB.linearVelocity = new Vector2(0, 0);
    }

}
