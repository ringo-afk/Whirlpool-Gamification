using UnityEngine;
using System.Collections;

public class Coins : MonoBehaviour
{
    private bool claimed;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        claimed = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && claimed == false)
        {
            GameControl.Instance.monedas += 1;
            GameControl.Instance.uiController.CoinUpdate();
            GameControl.Instance.sfxManager.CoinSound(transform.position);
            StartCoroutine(Claimed());
            
        }
    }

    private IEnumerator Claimed()
    {
        claimed = true;
        while (spriteRenderer.color.a > 0)
        {
            this.transform.position += new Vector3(0, 0.01f);
            spriteRenderer.color -= new Color(0,0,0,3f*Time.deltaTime);
            yield return null;
        }
        Destroy(gameObject);
    }
}
