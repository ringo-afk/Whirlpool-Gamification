using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioClip snap;
    public AudioClip virus;
    public AudioClip start;

    public void SnapSound()
    {
        AudioSource.PlayClipAtPoint(snap, Camera.main.transform.position, 0.5f);
    }

    public void VirusSound()
    {
        AudioSource.PlayClipAtPoint(virus, Camera.main.transform.position, 0.5f);
    }
    public void StartSound()
    {
        AudioSource.PlayClipAtPoint(start, Camera.main.transform.position, 0.5f);
    }
   
}
