using UnityEngine;

public class RespawnSystem : MonoBehaviour
{
    public Transform respawnPoint; // Asigna aquí el punto de respawn en el Inspector
    public float respawnDelay = 2f; // Tiempo de espera antes del respawn
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position; // Guarda la posición inicial del personaje
    }

    public void Respawn()
    {
        // Desactiva al personaje mientras respawnea
        gameObject.SetActive(false);
        Invoke("RespawnAtPoint", respawnDelay);
    }

    private void RespawnAtPoint()
    {
        transform.position = respawnPoint != null ? respawnPoint.position : startPosition; // Mueve al personaje al punto de respawn
        gameObject.SetActive(true); // Reactiva al personaje
    }
    private void Update()
{
    if (transform.position.y < -10) // Si cae por debajo de cierto punto
    {
        Respawn();
    }
}

}
