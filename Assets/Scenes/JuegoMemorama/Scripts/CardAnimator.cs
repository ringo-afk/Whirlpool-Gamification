using System.Collections;
using UnityEngine;

public class CardAnimator : MonoBehaviour
{
    private RectTransform rectTransform;
    private Transform originalParent;
    private int originalSiblingIndex;
    private Vector3 originalScale;
    private CanvasGroup canvasGroup;

    public bool isAnimating { get; private set; } = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
        originalScale = transform.localScale;
    }

    public void AnimarPopUp()
    {
        if (isAnimating) return;
        StartCoroutine(RutinaPopUp(1.3f, 0.15f));
    }

    public void AnimarRegreso()
    {
        StartCoroutine(RutinaPopUp(1.0f, 0.1f));
    }

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

        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();

        transform.SetParent(matchedPairsContainer);

        Vector3 startScale = transform.localScale;
        Vector3 targetScale = originalScale * 0.8f;
        float startAlpha = canvasGroup.alpha;
        float targetAlpha = 1.0f;

        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsed / duration);
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
        canvasGroup.alpha = targetAlpha;

        isAnimating = false;
    }
}