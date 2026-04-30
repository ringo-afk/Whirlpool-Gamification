using UnityEngine;

// Agregar este componente al prefab obstáculo junto con Draggable.
public class DraggableObstacle : MonoBehaviour
{
    [Tooltip("Penalización extra que se suma al timeLostOnWrongInput del QTE mientras este bloque esté en una caja")]
    [SerializeField] private int extraTimePenalty = 5;

    private QTEButton qte;
    private bool isPlacedInBox = false;
    private int originalPenalty;

    private void Awake()
    {
        qte = FindObjectOfType<QTEButton>();
    }

    // Llamar desde DropObject cuando este bloque es dropeado en una caja
    public void OnPlacedInBox()
    {
        if (isPlacedInBox || qte == null) return;

        isPlacedInBox = true;
        originalPenalty = qte.timeLostOnWrongInput;
        qte.timeLostOnWrongInput = originalPenalty + extraTimePenalty;
    }

    // Llamar desde DropObject cuando este bloque es removido de la caja
    // o cuando se destruye (al avanzar de ronda)
    public void OnRemovedFromBox()
    {
        if (!isPlacedInBox || qte == null) return;

        isPlacedInBox = false;
        qte.timeLostOnWrongInput = originalPenalty;
    }

    private void OnDestroy()
    {
        // Asegurarse de restaurar la penalización si el bloque es destruido
        OnRemovedFromBox();
    }
}