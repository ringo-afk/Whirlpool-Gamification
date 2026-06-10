using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Draggable : MonoBehaviour
{
    [SerializeField] public string choiceTag = "Rol";
    [SerializeField] public string answerId = "";
    [SerializeField] private TMP_Text answerText;

    public bool IsCorrect { get; private set; }

    private Camera mainCamera;
    private Vector3 dragOffset;
    private Vector3 dragStartPosition;
    private float dragDistanceToCamera;
    private Collider2D objectCollider2D;

    private void Awake()
    {
        mainCamera = Camera.main;
        objectCollider2D = GetComponent<Collider2D>();
    }

    public void Setup(string text, bool correct, string categoryTag = null)
    {
        if (answerText == null)
            answerText = GetComponentInChildren<TMP_Text>();

        if (answerText != null)
            answerText.text = text;

        if (!string.IsNullOrWhiteSpace(categoryTag))
            choiceTag = categoryTag;

        IsCorrect = correct;
        answerId = correct ? "correct" : "wrong";
    }

    private void OnMouseDown()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null) return;
        }

        Vector3 objectScreenPoint = mainCamera.WorldToScreenPoint(transform.position);
        dragDistanceToCamera = objectScreenPoint.z;

        Vector2 pointer = GetPointerScreenPosition();
        Vector3 mouseWorldPoint = mainCamera.ScreenToWorldPoint(
            new Vector3(pointer.x, pointer.y, dragDistanceToCamera)
        );
        dragOffset = transform.position - mouseWorldPoint;
        dragStartPosition = transform.position;
    }

    private void OnMouseDrag()
    {
        if (mainCamera == null) return;

        Vector2 pointer = GetPointerScreenPosition();
        Vector3 mouseWorldPoint = mainCamera.ScreenToWorldPoint(
            new Vector3(pointer.x, pointer.y, dragDistanceToCamera)
        );
        transform.position = mouseWorldPoint + dragOffset;
    }

    private void OnMouseUp()
    {
        DropObject dropTarget = FindBestDropTarget();
        if (dropTarget != null)
        {
            if (!dropTarget.ReceiveDrop(this))
            {
                transform.position = dragStartPosition;
            }

            return;
        }

        transform.position = dragStartPosition;
    }

    private DropObject FindBestDropTarget()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        Vector2 pointer = GetPointerScreenPosition();
        Vector2 worldPoint = mainCamera != null
            ? mainCamera.ScreenToWorldPoint(
                new Vector3(pointer.x, pointer.y, dragDistanceToCamera))
            : (Vector2)transform.position;

        DropObject bestTarget = null;
        float bestDistance = float.MaxValue;

        if (objectCollider2D != null)
        {
            Bounds bounds = objectCollider2D.bounds;
            Collider2D[] hits2D = Physics2D.OverlapBoxAll(
                bounds.center,
                bounds.size,
                transform.eulerAngles.z);

            for (int i = 0; i < hits2D.Length; i++)
            {
                TryPickDropTarget(hits2D[i], worldPoint, ref bestTarget, ref bestDistance);
            }
        }

        Collider2D[] pointHits = Physics2D.OverlapPointAll(worldPoint);
        for (int i = 0; i < pointHits.Length; i++)
        {
            TryPickDropTarget(pointHits[i], worldPoint, ref bestTarget, ref bestDistance);
        }

        return bestTarget;
    }

    private void TryPickDropTarget(
        Collider2D hit,
        Vector2 worldPoint,
        ref DropObject bestTarget,
        ref float bestDistance)
    {
        if (hit == null || hit.gameObject == gameObject)
        {
            return;
        }

        DropObject dropTarget = hit.GetComponent<DropObject>();
        if (dropTarget == null || !dropTarget.CanAcceptDrop(this))
        {
            return;
        }

        float distance = Vector2.Distance(worldPoint, dropTarget.transform.position);
        if (distance < bestDistance)
        {
            bestTarget = dropTarget;
            bestDistance = distance;
        }
    }

    private static Vector2 GetPointerScreenPosition()
    {
        if (Mouse.current != null)
        {
            return Mouse.current.position.ReadValue();
        }

        return Vector2.zero;
    }
}
