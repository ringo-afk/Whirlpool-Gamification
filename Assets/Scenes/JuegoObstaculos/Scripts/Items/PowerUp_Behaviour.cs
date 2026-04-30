using UnityEngine;

public class PowerUp_Behaviour : MonoBehaviour
{
    public float velocity;
    public float shootingDuration = 5f;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            int randomPower = Random.Range(1, 2);

            if (randomPower == 0)
            {
                GameControlRR.Instance.ActivateShield();
            }
            else if(randomPower == 1)
            {
                Player_ControlRR player = collision.gameObject.GetComponent<Player_ControlRR>();

                if (player != null)
                    player.ActivateShooting(shootingDuration);
               
            }

            Destroy(gameObject);
        }
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.down * Time.deltaTime * velocity;

        if (transform.position.y < -30)
            Destroy(gameObject);
        
    }
}
