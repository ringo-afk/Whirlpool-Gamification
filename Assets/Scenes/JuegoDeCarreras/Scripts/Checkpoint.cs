using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static List<Checkpoint> AllCheckpoints = new List<Checkpoint>();
    private bool claimed;

    void OnEnable()
    {
        AllCheckpoints.Add(this);
        claimed = false;
    }

    void OnDisable()
    {
        AllCheckpoints.Remove(this);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !claimed)
        {
            GameControl.Instance.checkpointsInt += 1;
            claimed = true;
        }
    }

    public void ResetCheckpoint()
    {
        claimed = false;
    }
}