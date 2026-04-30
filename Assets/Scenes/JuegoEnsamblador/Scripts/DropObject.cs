using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class DropObject : MonoBehaviour
{
    public UnityEvent onDropped;
    [SerializeField] private string[] acceptedTags;
    public bool snapDroppedObject = true;
    [Header("Correct answer")]
    [Tooltip("Expected answer ID for this slot. Leave empty to accept any object with valid tag.")]
    [SerializeField] private string requiredAnswerId = "";
    [SerializeField] private DropAnswerTracker answerTracker;
    private bool isCurrentlyCorrect;

    [Header("Cleanup after correct drop")]
    [Tooltip("If true, when a draggable is accepted here, all other draggable objects in the group will be deleted.")]
    [SerializeField] private bool deleteOtherDraggablesOnSuccess = true;

    [Tooltip("If true, only deletes draggable objects that share the same parent (subtree) as the dropped object.")]
    [SerializeField] private bool deleteOtherDraggablesInSameParent = true;

    [Tooltip("If false, inactive draggable objects will not be destroyed.")]
    [SerializeField] private bool destroyInactiveDraggables = false;

    public event Action OnDropFilled;

    private void Awake()
    {
        if (answerTracker == null)
        {
            answerTracker = FindObjectOfType<DropAnswerTracker>();
        }
    }

    private void OnEnable()
    {
        if (answerTracker != null)
        {
            answerTracker.RegisterDropObject(this);
        }
    }

    private void OnDisable()
    {
        if (answerTracker != null)
        {
            answerTracker.UnregisterDropObject(this);
        }
    }

    public void ReceiveDrop(Draggable droppedObject)
    {
        if (droppedObject == null)
        {
            return;
        }

        if (!AcceptsTag(droppedObject))
        {
            return;
        }

        bool isCorrectDrop = IsCorrectAnswer(droppedObject);

        if (snapDroppedObject)
        {
            droppedObject.transform.position = transform.position;
            GameControl1.Instance.sfxManager.SnapSound();
        }

        // Activar penalización extra si es obstáculo
        DraggableObstacle obstacle = droppedObject.GetComponent<DraggableObstacle>();
        if (obstacle != null)
        {
            obstacle.OnPlacedInBox();
        }

        if (deleteOtherDraggablesOnSuccess)
        {
            DeleteOtherDraggables(droppedObject);
        }

        isCurrentlyCorrect = isCorrectDrop;
        if (answerTracker != null)
        {
            answerTracker.NotifyDropChanged();
        }

        onDropped?.Invoke();
        StartCoroutine(NotifyFilledNextFrame());
    }

    private IEnumerator NotifyFilledNextFrame()
{
    yield return null; // espera que Destroy procese
    OnDropFilled?.Invoke();
}

    public bool IsCurrentlyCorrect()
    {
        return isCurrentlyCorrect;
    }

    public void SetRequiredAnswerId(string newRequiredAnswerId)
    {
        requiredAnswerId = newRequiredAnswerId ?? string.Empty;
    }

    public void ResetDropState()
    {
        isCurrentlyCorrect = false;
    }

    private void DeleteOtherDraggables(Draggable droppedObject)
    {
        if (droppedObject == null) return;

        // We try to delete only the "set" of draggables that belongs to this interaction.
        Transform searchRoot = deleteOtherDraggablesInSameParent ? droppedObject.transform.parent : null;

        Draggable[] candidates;
        if (searchRoot != null)
        {
            candidates = searchRoot.GetComponentsInChildren<Draggable>(true);
        }
        else
        {
            candidates = FindObjectsOfType<Draggable>();
        }

        for (int i = 0; i < candidates.Length; i++)
        {
            Draggable candidate = candidates[i];
            if (candidate == null) continue;

            if (candidate.gameObject == droppedObject.gameObject) continue;

            if (!destroyInactiveDraggables && !candidate.gameObject.activeInHierarchy) continue;

            Destroy(candidate.gameObject);
        }
    }

    private bool AcceptsTag(Draggable droppedObject)
    {
        if (droppedObject == null)
        {
            return false;
        }

        if (acceptedTags == null || acceptedTags.Length == 0)
        {
            return true;
        }

        string droppedChoiceTag = droppedObject.choiceTag;
        string droppedUnityTag = droppedObject.gameObject.tag;

        for (int i = 0; i < acceptedTags.Length; i++)
        {
            string acceptedTag = acceptedTags[i];
            if (string.IsNullOrWhiteSpace(acceptedTag))
            {
                continue;
            }

            if (acceptedTag == droppedChoiceTag || acceptedTag == droppedUnityTag)
            {
                return true;
            }
        }

        return false;
    }

    private bool IsCorrectAnswer(Draggable droppedObject)
    {
        if (droppedObject == null)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(requiredAnswerId))
        {
            return true;
        }

        return requiredAnswerId == droppedObject.answerId;
    }
}
