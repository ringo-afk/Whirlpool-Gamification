using UnityEngine;

public class CPUCarDebugging : MonoBehaviour
{
    //--------------------------------------
    public float baseX = 0f;
    public float minVisibleX = -9f;
    public float maxVisibleX = 9f;

    public float visualScale = 0.15f;
    public float smoothSpeed = 5f;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        baseX = transform.position.x;
    }

    void Update()
    {
        if (GameControlDebugging.Instance == null)
        {
            return;
        }

        //--------------------------------------------------------
        float playerProgress = GameControlDebugging.Instance.playerProgress;
        float enemyProgress = GameControlDebugging.Instance.enemyProgress;

        float difference = enemyProgress - playerProgress;

        float targetX = baseX + (difference * visualScale);

        //--------------------------------------------------------
        Vector3 newPosition = transform.position;
        newPosition.x = Mathf.Lerp(transform.position.x, targetX, smoothSpeed * Time.deltaTime);
        transform.position = newPosition;

        if (spriteRenderer != null)
        {
            //-----------------------------------------------------------------------
            if (transform.position.x < minVisibleX || transform.position.x > maxVisibleX)
            {
                spriteRenderer.enabled = false;
            }
            else
            {
                spriteRenderer.enabled = true;
            }
        }
    }
}
