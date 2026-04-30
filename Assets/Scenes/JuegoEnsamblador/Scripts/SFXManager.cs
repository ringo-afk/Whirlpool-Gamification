using UnityEngine;

public class SFXManager1 : MonoBehaviour
{
    public AudioClip snap;
    public AudioClip virus;
    public AudioClip start;
    public AudioClip QTE;

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
    public void QTESound()
    {
        AudioSource.PlayClipAtPoint(QTE, Camera.main.transform.position, 0.5f);
    }
   
}
