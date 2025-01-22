using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] levelPrefabs; // Array de prefabs para los pedazos del nivel
    public Transform player; // Referencia al jugador
    public float spawnDistance = 20f; // Distancia vertical para generar nuevos pedazos
    public float pieceHeight = 10f; // Altura de cada pedazo de nivel
    public int poolSize = 10; // Número de piezas en el pool

    private Queue<GameObject> levelPool; // Cola para manejar el pool de objetos
    private float lastSpawnY; // Última posición Y donde se generó un pedazo de nivel

    void Start()
    {
        InitializePool();

        // Genera los pedazos iniciales del nivel
        lastSpawnY = player.position.y;
        for (int i = 0; i < poolSize / 2; i++)
        {
            ReusePiece(i * pieceHeight);
        }
    }

    void Update()
    {
        // Verifica si el jugador se acerca al límite para reutilizar piezas
        if (player.position.y < lastSpawnY - spawnDistance)
        {
            ReusePiece(lastSpawnY - spawnDistance);
            lastSpawnY -= pieceHeight;
        }
    }

    void InitializePool()
    {
        // Inicializa el pool de objetos
        levelPool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            int prefabIndex = Random.Range(0, levelPrefabs.Length);
            GameObject piece = Instantiate(levelPrefabs[prefabIndex], Vector3.zero, Quaternion.identity);
            piece.SetActive(false); // Desactiva la pieza inicialmente
            levelPool.Enqueue(piece);
        }
    }

    void ReusePiece(float spawnY)
    {
        // Obtén un objeto del pool
        GameObject piece = levelPool.Dequeue();

        // Reactiva y posiciona la pieza
        piece.transform.position = new Vector3(0, spawnY, 0);
        piece.SetActive(true);

        // Devuélvelo al final de la cola
        levelPool.Enqueue(piece);
    }
}