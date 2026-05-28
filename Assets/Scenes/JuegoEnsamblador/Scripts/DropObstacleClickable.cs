using System.Collections.Generic;
using UnityEngine;

public class DropObstacleClickable : MonoBehaviour
{
    [SerializeField] private int hitsToClear = 3;

    private static readonly Dictionary<Collider2D, int> DisabledColliderCounts = new Dictionary<Collider2D, int>();

    private int currentHits;
    private Collider2D[] disabledColliders;
    private bool isBlocking;

    private DropObject blockedDropObject;
    private DropObstacleSpawner ownerSpawner;

    private void Awake()
    {
        currentHits = hitsToClear;
    }

    public void Init(DropObject dropTarget, int overrideHits, DropObstacleSpawner spawnerOwner)
    {
        blockedDropObject = dropTarget;
        ownerSpawner = spawnerOwner;

        currentHits = overrideHits > 0 ? overrideHits : hitsToClear;
        BlockDropTarget();
    }

    private void BlockDropTarget()
    {
        if (blockedDropObject == null) return;

        if (isBlocking) return;
        isBlocking = true;

        disabledColliders = blockedDropObject.GetComponentsInChildren<Collider2D>(true);
        for (int i = 0; i < disabledColliders.Length; i++)
        {
            DisableCollider(disabledColliders[i]);
        }
    }

    private void DisableCollider(Collider2D col)
    {
        if (col == null) return;

        if (!DisabledColliderCounts.TryGetValue(col, out int count))
        {
            DisabledColliderCounts[col] = 1;
            col.enabled = false;
            return;
        }

        DisabledColliderCounts[col] = count + 1;
    }

    private void EnableCollider(Collider2D col)
    {
        if (col == null) return;
        if (!DisabledColliderCounts.TryGetValue(col, out int count)) return;

        count--;
        if (count <= 0)
        {
            DisabledColliderCounts.Remove(col);
            col.enabled = true;
        }
        else
        {
            DisabledColliderCounts[col] = count;
        }
    }

    private void OnMouseDown()
    {
        if (!isBlocking) return;

        currentHits--;
        if (currentHits <= 0)
        {
            ClearObstacle();
        }
    }

    private void ClearObstacle()
    {
        // Re-enable any blocked drop colliders before destroying the obstacle.
        UnblockDropTarget();

        if (ownerSpawner != null)
        {
            ownerSpawner.NotifyObstacleRemoved(this, blockedDropObject);
        }

        Destroy(gameObject);
    }

    private void UnblockDropTarget()
    {
        if (!isBlocking) return;
        isBlocking = false;

        if (disabledColliders == null) return;

        for (int i = 0; i < disabledColliders.Length; i++)
        {
            EnableCollider(disabledColliders[i]);
        }

        disabledColliders = null;
    }

    private void OnDestroy()
    {
        // Safety net: if something destroys the obstacle without calling ClearObstacle().
        UnblockDropTarget();
    }
}

