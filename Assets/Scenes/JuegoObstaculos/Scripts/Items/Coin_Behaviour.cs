using UnityEngine;

public class Coin_Behaviour : MonoBehaviour
{
    public float velocity;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameControlRR.Instance.AddCoin();

            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.down * Time.deltaTime * velocity;
        if(transform.position.y < -30)
            Destroy(gameObject);
     
    }
}
