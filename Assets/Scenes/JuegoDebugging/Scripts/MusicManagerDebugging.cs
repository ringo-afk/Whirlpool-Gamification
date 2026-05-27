using UnityEngine;

public class MusicManagerDebugging : MonoBehaviour
{
    public AudioClip backgroundMusic;
    public float volume = 0.5f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.clip = backgroundMusic;
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        audioSource.Play();
    }
}
