using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject cone;
    public GameObject trash;
    public GameObject lamp;
    public GameObject coin;
    public GameObject powerUp;
    public float maxLength;
    public float minLength;
    public float timeToSpawn;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnerTime());
    }

    IEnumerator SpawnerTime()
    {
        float spawn = GetTimeByLevel();

        // Spawnear obstaculos
        yield return new WaitForSeconds(spawn);

        GameObject objectToSpawn = GetObject();
        
        Instantiate(objectToSpawn, new Vector3(
            transform.position.x + Random.Range(minLength, maxLength), transform.position.y, 0), Quaternion.identity);
            StartCoroutine(SpawnerTime());
    }

    GameObject GetObject()
    {
        int random1 = Random.Range(0,11);
        if(random1 <=7)
        {
            int level = GameControlRR.Instance.currentLevel;

            if (level == 1)
            {
                return cone;
            }
            else if (level == 2)
            {
                int random = Random.Range(0,2);
                if(random == 0)
                    return cone;
                else
                    return trash;
                
            }
            else
            {
                int random = Random.Range(0,3);
                if(random == 0)
                    return cone;
                else if(random == 1)
                    return trash;
                else
                {
                    return lamp;
                }

            }
                
        }
        else
        {
            int random = Random.Range(0, 3);
            if(random <= 1)
                return coin;
            else
                return powerUp;
        }


    }

    float GetTimeByLevel()
    {
        int level = GameControlRR.Instance.currentLevel;

        if (level == 1)
        {
            return timeToSpawn;
        }
        else if(level == 2)
        {
            return timeToSpawn / 1.5f;
        }
        else
        {
            return timeToSpawn / 2f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
