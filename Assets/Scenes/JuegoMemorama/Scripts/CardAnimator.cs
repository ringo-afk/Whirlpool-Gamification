using System.Collections;
using UnityEngine;

public class CardAnimator : MonoBehaviour
{
    private RectTransform rectTransform;
    private Transform originalParent;
    private int originalSiblingIndex;
    private Vector3 originalScale;
    private CanvasGroup canvasGroup; // Para suavizar la transición

    public bool isAnimating { get; private set; } = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>(); // Asegúrate de agregarlo a la carta
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
        originalScale = transform.localScale;
    }

    // A: Animación de Pop-Up (Escalar un poco en su lugar para leer)
    public void AnimarPopUp()
    {
        if (isAnimating) return; 
        StartCoroutine(RutinaPopUp(1.3f, 0.15f)); // Crece un 30% en 0.15s
    }

    // B: Animación de Regresar (Cuando fallan un par)
    public void AnimarRegreso()
        {
            StartCoroutine(RutinaPopUp(1.0f, 0.1f)); // Regresa a 1.0x en 0.1f
        }

    // C: Animación de Viajar (Cuando hacen MATCH, viajan a la columna izquierda)
    public void AnimarViajeAlSofa(Transform matchedPairsContainer)
    {
        if (isAnimating) return; 
        StartCoroutine(RutinaViajarAlSofa(matchedPairsContainer));
    }

    private IEnumerator RutinaPopUp(float scaleFactor, float duration)
    {
        isAnimating = true;
        Vector3 targetScale = originalScale * scaleFactor;
        Vector3 startScale = transform.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;
        isAnimating = false;
    }

    private IEnumerator RutinaViajarAlSofa(Transform matchedPairsContainer)
    {
        isAnimating = true;

        // 1. Guardamos datos originales (por si acaso reiniciamos el juego)
        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();

        // 2. Rompemos las reglas del Grid del tablero
        // Al ponerlo en el container, el Vertical Layout Group lo va a absorber
        transform.SetParent(matchedPairsContainer);
        
        // 3. Interpolación (Lerp) visual para que se vea que "vuela" a su nuevo lugar
        // Como el Vertical Layout Group controla la posición, nosotros interpolamos escala y transparencia
        
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = originalScale * 0.8f; // Las hacemos un poquito más chicas en la columna
        float startAlpha = canvasGroup.alpha;
        float targetAlpha = 1.0f; // Asegurar que sea visible

        float duration = 0.5f; // Viaje suave y satisfactorio
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Escala suave
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsed / duration);
            // Transparencia suave
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
        canvasGroup.alpha = targetAlpha;
        
        isAnimating = false;
    }
}