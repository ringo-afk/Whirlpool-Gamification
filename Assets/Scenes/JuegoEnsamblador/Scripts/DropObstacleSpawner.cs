using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Spawns clickable obstacles over DropObjects at random intervals.
/// </summary>
public class DropObstacleSpawner : MonoBehaviour
{
    [SerializeField] private DropObject[] dropTargets;
    [SerializeField] private DropObstacleClickable obstaclePrefab;

    [SerializeField] private float minSecondsBetweenSpawns = 1.0f;
    [SerializeField] private float maxSecondsBetweenSpawns = 3.0f;

    
    [SerializeField] private int maxActiveObstacles = 1;

    
    [Tooltip("If true, prevents spawning more than one obstacle on the same DropObject at the same time.")]
    [SerializeField] private bool avoidMultipleOnSameBox = true;

    
    [SerializeField] private int minHitsToClear = 1;
    [SerializeField] private int maxHitsToClear = 3;

    
    [SerializeField] private Transform obstaclesRoot;

    
    [SerializeField] private Vector3 spawnPositionOffset;

    private float nextSpawnTime;
    private int activeObstacles;
    private HashSet<DropObject> blockedTargets = new HashSet<DropObject>();

    private void OnEnable()
    {
        activeObstacles = 0;
        blockedTargets.Clear();
        ScheduleNextSpawn();
    }

    private void Update()
    {
        nextSpawnTime -= Time.deltaTime;
        if (nextSpawnTime > 0f) return;

        if (activeObstacles < maxActiveObstacles)
        {
            TrySpawnObstacle();
        }

        ScheduleNextSpawn();
    }

    private void ScheduleNextSpawn()
    {
        float minT = Mathf.Max(0.01f, minSecondsBetweenSpawns);
        float maxT = Mathf.Max(minT, maxSecondsBetweenSpawns);
        nextSpawnTime = Random.Range(minT, maxT);
    }

    private void TrySpawnObstacle()
    {
        if (dropTargets == null || dropTargets.Length == 0) return;
        if (obstaclePrefab == null) return;

        DropObject target = PickSpawnTarget();
        if (target == null) return;

        Transform parent = obstaclesRoot != null ? obstaclesRoot : target.transform.parent;

        // Spawn at the DropObject position (same "box" center).
        Vector3 spawnPos = target.transform.position + spawnPositionOffset;
        Quaternion spawnRot = target.transform.rotation;

        int hits = (maxHitsToClear > minHitsToClear)
            ? Random.Range(minHitsToClear, maxHitsToClear + 1)
            : minHitsToClear;

        // Instantiate without parent first, then re-parent while preserving world transform.
        // This prevents parent scale from unexpectedly affecting obstacle size.
        DropObstacleClickable obstacleInstance =
            Instantiate(obstaclePrefab, spawnPos, spawnRot);

        if (parent != null)
        {
            obstacleInstance.transform.SetParent(parent, true);
        }

        obstacleInstance.Init(target, hits, this);
        
        GameControl.Instance.sfxManager.VirusSound();
        activeObstacles++;
        if (avoidMultipleOnSameBox)
        {
            blockedTargets.Add(target);
        }
    }

    private DropObject PickSpawnTarget()
    {
        if (!avoidMultipleOnSameBox) return dropTargets[Random.Range(0, dropTargets.Length)];

        // Build a list of available (not currently blocked) targets.
        List<DropObject> available = null;
        for (int i = 0; i < dropTargets.Length; i++)
        {
            DropObject d = dropTargets[i];
            if (d == null) continue;
            if (blockedTargets.Contains(d)) continue;

            if (available == null) available = new List<DropObject>();
            available.Add(d);
        }

        if (available == null || available.Count == 0) return null;
        return available[Random.Range(0, available.Count)];
    }

    public void NotifyObstacleRemoved(DropObstacleClickable obstacle, DropObject target)
    {
        activeObstacles = Mathf.Max(0, activeObstacles - 1);

        if (avoidMultipleOnSameBox && target != null)
        {
            blockedTargets.Remove(target);
        }
    }
}

