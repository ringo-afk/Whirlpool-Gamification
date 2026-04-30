using System.Collections.Generic;
using UnityEngine;

public class DraggableSetSpawner : MonoBehaviour
{
    [Header("Spawn Points (exactly the amount you want to show)")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Draggable Prefabs Pool")]
    [Tooltip("These prefabs must already have the Unity tag(s) your DropObject accepts.")]
    [SerializeField] private Draggable[] draggablePrefabs;

    [Header("Parent / Cleanup")]
    [Tooltip("Where the spawned draggables will be parented and cleared from.")]
    [SerializeField] private Transform draggablesRoot;

    [Header("Behavior")]
    [SerializeField] private bool avoidDuplicatesInSet = true;
    [SerializeField] private bool spawnOnStart = false;

    private void Awake()
    {
        if (draggablesRoot == null)
        {
            draggablesRoot = transform;
        }
    }

    private void Start()
    {
        if (spawnOnStart)
        {
            SpawnNextSet();
        }
    }

    public void SpawnNextSet()
    {
        ClearExistingDraggables();
        SpawnSet();
    }

    public void SetDraggablePrefabs(Draggable[] newPrefabs)
    {
        draggablePrefabs = newPrefabs;
    }

    private void ClearExistingDraggables()
    {
        if (draggablesRoot == null) return;

        
        Draggable[] existing = draggablesRoot.GetComponentsInChildren<Draggable>(true);
        for (int i = 0; i < existing.Length; i++)
        {
            if (existing[i] != null)
            {
                Destroy(existing[i].gameObject);
            }
        }
    }

    private void SpawnSet()
    {
        if (spawnPoints == null || spawnPoints.Length == 0) return;
        if (draggablePrefabs == null || draggablePrefabs.Length == 0) return;

        List<Draggable> pool = new List<Draggable>(draggablePrefabs.Length);
        for (int i = 0; i < draggablePrefabs.Length; i++)
        {
            if (draggablePrefabs[i] != null)
            {
                pool.Add(draggablePrefabs[i]);
            }
        }

        if (pool.Count == 0) return;

        List<Draggable> uniquePool = null;
        if (avoidDuplicatesInSet)
        {
            uniquePool = new List<Draggable>(pool);
        }

        for (int slotIndex = 0; slotIndex < spawnPoints.Length; slotIndex++)
        {
            Transform spawnPoint = spawnPoints[slotIndex];
            if (spawnPoint == null) continue;

            Draggable prefabToSpawn = null;

            if (avoidDuplicatesInSet && uniquePool != null && uniquePool.Count > 0)
            {
                int pick = Random.Range(0, uniquePool.Count);
                prefabToSpawn = uniquePool[pick];
                uniquePool.RemoveAt(pick);
            }
            else
            {
                int pick = Random.Range(0, pool.Count);
                prefabToSpawn = pool[pick];
            }

            if (prefabToSpawn == null) continue;

            Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation, draggablesRoot);
        }
    }
}

