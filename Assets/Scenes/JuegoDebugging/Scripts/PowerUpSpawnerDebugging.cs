using System.Collections;
using UnityEngine;

public class PowerUpSpawnerDebugging : MonoBehaviour
{
    public GameObject powerUpPrefab;

    public float minHeight = -2f;
    public float maxHeight = 2f;

    public float timeToSpawnMin = 5f;
    public float timeToSpawnMax = 9f;

    void Start()
    {
        StartCoroutine(SpawnerTime());
    }

    IEnumerator SpawnerTime()
    {
        yield return new WaitForSeconds(Random.Range(timeToSpawnMin, timeToSpawnMax));

        Instantiate(
            powerUpPrefab,
            new Vector3(transform.position.x, transform.position.y + Random.Range(minHeight, maxHeight), 0),
            Quaternion.identity
        );

        StartCoroutine(SpawnerTime());
    }
}
