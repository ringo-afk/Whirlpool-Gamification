using UnityEngine;
using UnityEngine.InputSystem;

public class Draggable : MonoBehaviour
{
    // Logical identifier used by DropObject to determine correctness.
    // This is intentionally NOT Unity's built-in tag system (TagManager),
    // so we can use values like "Contexto" and "Tarea" even if they are not defined as Unity tags.
    [SerializeField] public string choiceTag = "Rol";
    [SerializeField] public string answerId = "";

    private Camera mainCamera;
    private Vector3 dragOffset;
    private float dragDistanceToCamera;
    private Collider2D objectCollider2D;

    private void Awake()
    {
        mainCamera = Camera.main;
        objectCollider2D = GetComponent<Collider2D>();
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
        DropObject dropTarget = FindOverlappingDropTarget();
        if (dropTarget != null)
        {
            dropTarget.ReceiveDrop(this);
            return;
        }

        if (mainCamera == null)
        {
            return;
        }

        Vector2 pointer = GetPointerScreenPosition();
        Vector2 worldPoint = mainCamera.ScreenToWorldPoint(new Vector3(pointer.x, pointer.y, dragDistanceToCamera));
        Collider2D hit2D = Physics2D.OverlapPoint(worldPoint);
        if (hit2D != null)
        {
            DropObject target2D = hit2D.GetComponent<DropObject>();
            if (target2D != null && target2D.gameObject != gameObject)
            {
                target2D.ReceiveDrop(this);
            }
        }
    }

    private DropObject FindOverlappingDropTarget()
    {
        if (objectCollider2D != null)
        {
            Bounds bounds = objectCollider2D.bounds;
            Collider2D[] hits2D = Physics2D.OverlapBoxAll(bounds.center, bounds.size, transform.eulerAngles.z);
            for (int i = 0; i < hits2D.Length; i++)
            {
                if (hits2D[i].gameObject == gameObject)
                {
                    continue;
                }

                DropObject dropTarget = hits2D[i].GetComponent<DropObject>();
                if (dropTarget != null)
                {
                    return dropTarget;
                }
            }
        }

        return null;
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
