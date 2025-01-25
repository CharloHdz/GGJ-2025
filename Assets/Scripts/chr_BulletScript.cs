using UnityEngine;

public class chr_BulletScript : MonoBehaviour
{
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.linearVelocity.x > 15)
            rb.linearVelocity = new Vector2(15, rb.linearVelocity.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Rompible"){
            chr_OP.instance.SpawnRompiblePS(transform.position, transform.rotation);
            Destroy(other.gameObject);
            chr_OP.instance.BulletDestroy(gameObject);
        }else if(other.gameObject.layer == 6 || other.gameObject.layer == 8){
            chr_OP.instance.BulletDestroy(gameObject);
        }
    }
}