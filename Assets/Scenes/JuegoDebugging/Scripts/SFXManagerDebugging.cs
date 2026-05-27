using UnityEngine;

public class SFXManagerDebugging : MonoBehaviour
{
    public static SFXManagerDebugging Instance;

    public AudioClip crashSound;
    public AudioClip powerUpSound;
    public AudioClip questionBoostSound;

    public float volume = 0.7f;

    void Awake()
    {
        Instance = this;
    }

    public void PlayCrashSound()
    {
        AudioSource.PlayClipAtPoint(crashSound, Camera.main.transform.position, volume);
    }

    public void PlayPowerUpSound()
    {
        AudioSource.PlayClipAtPoint(powerUpSound, Camera.main.transform.position, volume);
    }

    public void PlayQuestionBoostSound()
    {
        AudioSource.PlayClipAtPoint(questionBoostSound, Camera.main.transform.position, volume);
    }
}