using UnityEngine;

public class Bullet_Behaviour : MonoBehaviour
{
    public float speed = 10f;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;
        
        if(transform.position.y > 20)
            Destroy(gameObject);
    }
}
