using UnityEngine;
using TMPro;

public class chr_GameManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Rigidbody2D PlayerRB;
    [SerializeField] public GameObject player;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float Distance;
    public float Score;
    [SerializeField] private TextMeshProUGUI ScoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerRB = GameObject.Find("Player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ScoreDistancia();
        if(Input.GetKeyDown(KeyCode.R)){
            Respawn();
        }

        if(PlayerRB.linearVelocity.y < -10){
            PlayerRB.linearVelocity = new Vector2(PlayerRB.linearVelocity.x, -10);
        }
    }

    public void ScoreDistancia(){
        float currentY = player.transform.position.y;
        Distance = respawnPoint.position.y - currentY;
        Score = Mathf.Round(Distance);
        ScoreText.text = "Score: " + Score.ToString() + "m";
    }

    public void Respawn(){
        player.transform.position = respawnPoint.position;
        PlayerRB.linearVelocity = new Vector2(0, 0);
    }
}
