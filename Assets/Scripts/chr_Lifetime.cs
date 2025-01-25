using UnityEngine;

public class chr_Lifetime : MonoBehaviour
{ 
    [SerializeField] private float time;
    private float Lifetime;
    void OnEnable()
    {
        Lifetime = time;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Lifetime -= Time.deltaTime;
        if(Lifetime <= 0){
            gameObject.SetActive(false);
        }
    }
}
