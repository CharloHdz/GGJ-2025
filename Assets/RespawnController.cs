using UnityEngine;

public class RespawnController : MonoBehaviour
{
    [Header("Respawn Settings")]
    public Transform respawnPoint; // Punto de respawn asignado desde el Inspector
    public float fallThreshold = -10f; // Altura mínima antes de que el personaje reaparezca
    public float respawnSpeed = 5f; // Velocidad que tendrá el personaje al reaparecer

    private Rigidbody2D rb;

    private void Start()
    {
        // Obtiene el componente Rigidbody2D del personaje
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("No se encontró un Rigidbody2D en el personaje.");
        }
    }

    private void Update()
    {
        // Verifica si el personaje cae por debajo del umbral
        if (transform.position.y < fallThreshold)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        // Reinicia la posición del personaje al punto de respawn
        transform.position = respawnPoint.position;

        // Resetea la velocidad del Rigidbody2D
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // Resetea la velocidad completamente

            // Determina la dirección hacia donde está mirando el personaje
            float direction = transform.localScale.x > 0 ? 1 : -1;

            // Aplica la nueva velocidad horizontal
            rb.linearVelocity = new Vector2(respawnSpeed * direction, 0); // Velocidad fija en X
        }

        Debug.Log("Personaje reapareció con velocidad fija: " + respawnSpeed);
    }
}
