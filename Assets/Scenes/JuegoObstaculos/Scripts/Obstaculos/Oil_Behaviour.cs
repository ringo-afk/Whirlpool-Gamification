using Unity.VisualScripting;
using UnityEngine;

public class Oil_Behaviour : MonoBehaviour
{    
    public float velocity;
    public float slipperyDuration = 4f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player_ControlRR player = collision.gameObject.GetComponent<Player_ControlRR>();

            if(player != null)
                player.SetSlippery(slipperyDuration);
    
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
