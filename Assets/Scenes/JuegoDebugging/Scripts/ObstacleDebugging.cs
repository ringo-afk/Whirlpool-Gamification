using UnityEngine;

public class ObstacleDebugging : MonoBehaviour
{
    public float velocity = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerDebugging player = collision.GetComponent<PlayerDebugging>();

            if (player != null)
            {
                player.CrashAnimation();
            }

            // Sonido de choque
            if (SFXManagerDebugging.Instance != null)
            {
                SFXManagerDebugging.Instance.PlayCrashSound();
            }

            GameControlDebugging.Instance.LoseLife();
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Mueve el obstaculo hacia la izquierda
        transform.position += Vector3.left * Time.deltaTime * velocity;

        // Destruye el obstaculo si sale de pantalla
        if (transform.position.x < -15f)
        {
            Destroy(gameObject);
        }
    }
}