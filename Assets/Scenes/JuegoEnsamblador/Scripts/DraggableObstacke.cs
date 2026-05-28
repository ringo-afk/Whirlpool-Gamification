using UnityEngine;

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

    public void OnPlacedInBox()
    {
        if (isPlacedInBox || qte == null) return;

        isPlacedInBox = true;
        originalPenalty = qte.timeLostOnWrongInput;
        qte.timeLostOnWrongInput = originalPenalty + extraTimePenalty;
    }

    public void OnRemovedFromBox()
    {
        if (!isPlacedInBox || qte == null) return;

        isPlacedInBox = false;
        qte.timeLostOnWrongInput = originalPenalty;
    }

    private void OnDestroy()
    {
        OnRemovedFromBox();
    }
}