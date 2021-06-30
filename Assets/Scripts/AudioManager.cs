using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    public AudioClip explosionClip;

    private AudioSource audioSource;

    [SerializeField] private float craneVolume = 0.4f;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayExplosionSound(Vector3 location)
    {
        AudioSource.PlayClipAtPoint(explosionClip, location, 0.7f);
    }

    public void PlayCraneSound()
    {
        if (audioSource.volume == 0)
            StartCoroutine(FadeSound(audioSource, 0f, craneVolume, 0.1f));
    }

    public void StopCraneSound()
    {
        if (audioSource.volume == craneVolume)
            StartCoroutine(FadeSound(audioSource, craneVolume, 0f, 1f));
    }

    IEnumerator FadeSound(AudioSource audioSource, float startVolume, float endVolume, float duration)
    {
        if (startVolume == 0)
            audioSource.Play();

        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, endVolume, timeElapsed / duration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        audioSource.volume = endVolume;

        if (audioSource.volume == 0)
            audioSource.Stop();
    }
}
