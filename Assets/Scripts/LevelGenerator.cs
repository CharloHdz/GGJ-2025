using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] levelPrefabs; // Array de prefabs para los pedazos del nivel
    public Transform player; // Referencia al jugador
    public float spawnDistance = 20f; // Distancia vertical para generar nuevos pedazos
    public float pieceHeight = 10f; // Altura de cada pedazo de nivel
    public int poolSize = 10; // Número de piezas en el pool
    public float initialYPosition = 100f; // Posición fija inicial para generar las piezas
    public float reuseMargin = 30f; // Margen adicional para reutilizar piezas antes de que el jugador se acerque

    private Queue<GameObject> levelPool; // Cola para manejar el pool de objetos
    private float lastSpawnY; // Última posición Y donde se generó un pedazo de nivel

    void Start()
    {
        InitializePool();

        // Genera los pedazos iniciales del nivel
        lastSpawnY = initialYPosition;
        for (int i = 0; i < poolSize; i++)
        {
            ReusePiece(lastSpawnY - spawnDistance);
            lastSpawnY -= pieceHeight;
        }
    }

    void Update()
    {
        // Verifica si el jugador se acerca al límite para reutilizar piezas
        if (player.position.y < lastSpawnY - spawnDistance - reuseMargin)
        {
            ReusePiece(lastSpawnY - spawnDistance);
            lastSpawnY -= pieceHeight;
        }
    }

    public void InitializePool()
    {
        // Inicializa el pool de objetos
        levelPool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            int prefabIndex = Random.Range(0, levelPrefabs.Length);
            // Instanciar la pieza en el lugar que indica el prefab 
            GameObject piece = Instantiate(levelPrefabs[prefabIndex]);
            piece.SetActive(false); // Desactiva la pieza inicialmente

            // Mantener la posición en x del prefab y ajustar la posición en y desde una posición fija
            Vector3 position = piece.transform.position;
            piece.transform.position = new Vector3(position.x, initialYPosition - i * pieceHeight, position.z);

            levelPool.Enqueue(piece);
        }
    }

    void ReusePiece(float spawnY)
    {
        if (levelPool.Count > 0)
        {
            GameObject piece = levelPool.Dequeue();
            piece.SetActive(true);

            // Mantener la posición en x del prefab y ajustar la posición en y
            Vector3 position = piece.transform.position;
            piece.transform.position = new Vector3(position.x, spawnY, position.z);

            levelPool.Enqueue(piece);
        }
    }
}