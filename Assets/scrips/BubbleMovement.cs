using UnityEngine;

public class BubbleMovement : MonoBehaviour
{
    //public float speed = 2f; // Velocidad de movimiento vertical
    public float floatRange = 0.5f; // Rango del efecto flotante
    public float floatSpeed = 2f; // Velocidad del efecto flotante

    private float startLocalY; // Posición inicial en Y

    void Start()
    {
        startLocalY = transform.localPosition.y;
    }

    void Update()
    {
        // Movimiento vertical hacia arriba
        //transform.Translate(Vector2.up * speed * Time.deltaTime);
        //startY += speed * Time.deltaTime;

        // Efecto flotante (movimiento oscilante en Y)
        float offset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
        transform.localPosition = new Vector2(transform.localPosition.x, startLocalY + offset);
    }

    private void OnBecameInvisible()
    {
        // Destruir la burbuja si sale de la pantalla
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
