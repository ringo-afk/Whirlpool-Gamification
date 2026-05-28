using UnityEngine;

public class PowerUpDebugging : MonoBehaviour
{
    public float velocity = 5f;
    public float boostAmount = 15f;
    public int points = 50;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Sonido de power-up
            if (SFXManagerDebugging.Instance != null)
            {
                SFXManagerDebugging.Instance.PlayPowerUpSound();
            }

            GameControlDebugging.Instance.AddPlayerBoost(boostAmount);
            GameControlDebugging.Instance.AddRewardPoints(points);

            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Mueve el power-up hacia la izquierda
        transform.position += Vector3.left * Time.deltaTime * velocity;

        // Destruye el power-up si sale de pantalla
        if (transform.position.x < -15f)
        {
            Destroy(gameObject);
        }
    }
}
