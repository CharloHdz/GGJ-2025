using UnityEngine;
using UnityEngine.UI; // Para manejar UI
using UnityEngine.SceneManagement; // Para reiniciar escena si el tiempo llega a 0
using TMPro;
public class PlayerLifeTimer : MonoBehaviour
{
    [Header("Configuración del temporizador")]
    public float lifeTime = 30f; // Tiempo de vida inicial del jugador
    public TextMeshProUGUI timerText; // Referencia al texto de la UI para mostrar el tiempo

    [Header("Interacción con burbujas")]
    public float bubbleBonusTime = 2f; // Tiempo que suman las burbujas

    private bool isAlive = true; // Verifica si el jugador está vivo

    void Update()
    {
        if (isAlive)
        {
            // Reducir el tiempo en función del tiempo transcurrido
            lifeTime -= Time.deltaTime;

            // Mostrar el tiempo en formato con decimales
            timerText.text = "Tiempo: " + lifeTime.ToString("F2");

            // Si el tiempo llega a 0, termina el juego
            if (lifeTime <= 0f)
            {
                isAlive = false;
                GameOver();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detectar colisión con burbujas
        if (collision.CompareTag("Bubble"))
        {
            lifeTime += bubbleBonusTime; // Añadir tiempo al temporizador
            Destroy(collision.gameObject); // Eliminar la burbuja
        }
    }

    void GameOver()
    {
        Debug.Log("¡Juego Terminado!");
        // Reiniciar la escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
