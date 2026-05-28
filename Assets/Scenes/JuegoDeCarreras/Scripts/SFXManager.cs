using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioClip coin;

    public void CoinSound(Vector3 _position)
    {
        AudioSource.PlayClipAtPoint(coin, _position, 0.5f);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
