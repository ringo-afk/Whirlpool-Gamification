using System.Collections;
using UnityEngine;

public class SpawnerDebugging : MonoBehaviour
{
    public GameObject obstaclePrefab;

    public float minHeight = -3f;
    public float maxHeight = 3f;

    public float timeToSpawnMin = 1f;
    public float timeToSpawnMax = 3f;

    void Start()
    {
        StartCoroutine(SpawnerTime());
    }

    IEnumerator SpawnerTime()
    {
        yield return new WaitForSeconds(Random.Range(timeToSpawnMin, timeToSpawnMax));

        Instantiate(
            obstaclePrefab,
            new Vector3(transform.position.x, transform.position.y + Random.Range(minHeight, maxHeight), 0),
            Quaternion.identity
        );

        StartCoroutine(SpawnerTime());
    }
}
