using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameControl.Instance.checkpointsInt >= GameControl.Instance.checkpointsNeeded)
            {
                if (GameControl.Instance.Laps+1 >= GameControl.Instance.TotalLaps)
                {
                    PlayerPrefs.SetInt("Monedas", GameControl.Instance.monedas);
                    //PlayerPrefs.SetFloat("Tiempo", GameControl.Instance.tiempoInicio - GameControl.Instance.timer.tiempo);
                    SceneManager.LoadScene("Ganaste");
                }
                else
                {
                    GameControl.Instance.Laps += 1;
                    GameControl.Instance.lapsText.UpdateLaps();
                }
            }

            GameControl.Instance.checkpointsInt = 0;

            foreach (Checkpoint cp in Checkpoint.AllCheckpoints)
            {
                cp.ResetCheckpoint();
            }
        }
    }
}