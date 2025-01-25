using UnityEngine;

public class chr_OP : MonoBehaviour
{
    //Object Pooling
    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private GameObject[] bulletPool;

    [Header("RompiblePS")]
    [SerializeField] private GameObject rompiblePS;
    [SerializeField] private int poolSizeRompiblePS = 10;
    [SerializeField] private GameObject[] rompiblePSPool;

    public static chr_OP instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bulletPool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            bulletPool[i] = Instantiate(bulletPrefab, transform);
            bulletPool[i].SetActive(false);
        }

        rompiblePSPool = new GameObject[poolSizeRompiblePS];
        for (int i = 0; i < poolSizeRompiblePS; i++)
        {
            rompiblePSPool[i] = Instantiate(rompiblePS, transform);
            rompiblePSPool[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnBullet(Vector3 position, Quaternion rotation)
    {
        for (int i = 0; i < poolSize; i++)
        {
            if (!bulletPool[i].activeInHierarchy)
            {
                bulletPool[i].transform.position = position;
                bulletPool[i].transform.rotation = rotation;
                bulletPool[i].SetActive(true);
                break;
            }
        }
    }

    public void SpawnRompiblePS(Vector3 position, Quaternion rotation)
    {
        for (int i = 0; i < poolSizeRompiblePS; i++)
        {
            if (!rompiblePSPool[i].activeInHierarchy)
            {
                rompiblePSPool[i].transform.position = position;
                rompiblePSPool[i].transform.rotation = rotation;
                rompiblePSPool[i].SetActive(true);
                break;
            }
        }
    }

    public void BulletDestroy(GameObject bullet)
    {
        bullet.SetActive(false);
    }

    public void RompiblePSDestroy(GameObject rompiblePS)
    {
        rompiblePS.SetActive(false);
    }
}
