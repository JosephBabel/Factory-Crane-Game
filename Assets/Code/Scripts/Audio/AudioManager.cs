using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;

/// <summary>
/// Manages sfx and music.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    public Sound[] sounds;

    private IEnumerator startCraneFade;
    private IEnumerator stopCraneFade;

    // Fixed audio sources
    private AudioSource music;
    private AudioSource ambience;
    private AudioSource crane;

    // Save original volume settings
    private float originalMusicVolume;
    private float originalAmbienceVolume;
    private float originalCraneVolume;

    public bool isMusicMuted;
    public bool isSFXMuted;

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
        AudioSource[] audioSources = GetComponents<AudioSource>();
        music = audioSources[0];
        ambience = audioSources[1];
        crane = audioSources[2];
        originalMusicVolume = music.volume;
        originalAmbienceVolume = ambience.volume;
        originalCraneVolume = crane.volume;

        crane.volume = 0f;
        crane.Play();
    }

    void Update()
    {
        if (!GameManager.instance.isRunning)
            StopCraneSound();
    }

    IEnumerator FadeSound(AudioSource audioSource, float startVolume, float endVolume, float duration)
    {
        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, endVolume, timeElapsed / duration);
            timeElapsed += Time.unscaledDeltaTime;

            yield return null;
        }
        audioSource.volume = endVolume;
    }

    /// <summary>
    /// Start playing crane sound with fade in.
    /// </summary>
    public void StartCraneSound()
    {
        if (stopCraneFade != null)
        {
            StopCoroutine(stopCraneFade);
            stopCraneFade = null;
        }

        startCraneFade = FadeSound(crane, crane.volume, originalCraneVolume, 0.01f);
        StartCoroutine(startCraneFade);
    }

    /// <summary>
    /// Stop playing crane sound with fade out.
    /// </summary>
    public void StopCraneSound()
    {
        if (startCraneFade != null)
        {
            StopCoroutine(startCraneFade);
            startCraneFade = null;
        }

        stopCraneFade = FadeSound(crane, crane.volume, 0f, 0.6f);
        StartCoroutine(stopCraneFade);
    }

    private Sound FindSound(string name)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);
        if (sound == null)
        {
            Debug.LogError($"Error: Sound of name \"{name}\" does not exist.");
            return null;
        }

        return sound;
    }

    private void LoadAudioFromSound(AudioSource audioSource, Sound sound)
    {
        audioSource.clip = sound.clip;
        audioSource.bypassEffects = sound.bypassEffects;
        audioSource.bypassListenerEffects = sound.bypassListenerEffects;
        audioSource.volume = sound.volume;
        audioSource.pitch = sound.pitch;
        audioSource.minDistance = sound.minDistance;
        audioSource.maxDistance = sound.maxDistance;
        audioSource.spatialBlend = sound.spatialBlend;
    }

    IEnumerator DestroyAfterDelay(GameObject gameObject, float delay)
    {
        float timer = 0f;
        while (gameObject)
        {
            timer += Time.unscaledDeltaTime;
            if (timer >= delay)
                Destroy(gameObject);

            yield return null;
        }
    }

    /// <summary>
    /// Instantiate temporary Audio Source that plays a sound effect at AudioManager location.
    /// </summary>
    /// <param name="name">Name of audio source to instantiate.</param>
    public void PlayClip(string name)
    {
        if (!isSFXMuted)
        {
            Sound sound = FindSound(name);
            GameObject tempAudio = new GameObject("TempAudio");
            tempAudio.transform.position = gameObject.transform.position;
            tempAudio.transform.parent = gameObject.transform;
            AudioSource tempSource = tempAudio.AddComponent<AudioSource>();
            LoadAudioFromSound(tempSource, sound);
            tempSource.Play();
            StartCoroutine(DestroyAfterDelay(tempAudio, tempSource.clip.length));
        }
    }

    /// <summary>
    /// Instantiate temporary Audio Source that plays a sound effect at a specified location.
    /// </summary>
    /// <param name="name">Name of audio source to instantiate.</param>
    /// <param name="position">Position to play audio source.</param>
    public void PlayClip(string name, Vector3 position)
    {
        if (!isSFXMuted)
        {
            Sound sound = FindSound(name);
            GameObject tempAudio = new GameObject("TempAudio");
            tempAudio.transform.position = position;
            tempAudio.transform.parent = gameObject.transform;
            AudioSource tempSource = tempAudio.AddComponent<AudioSource>();
            LoadAudioFromSound(tempSource, sound);
            tempSource.Play();
            StartCoroutine(DestroyAfterDelay(tempAudio, tempSource.clip.length));
        }
    }

    /// <summary>
    /// Toggles music on or off.
    /// </summary>
    /// <param name="isMuted">Whether to mute music.</param>
    public void ToggleMusic(bool isMuted)
    {
        if (isMuted)
        {
            isMusicMuted = true;
            music.Pause();
        }
        else
        {
            isMusicMuted = false;
            music.Play();
        }
    }

    /// <summary>
    /// Toggles SFX on or off.
    /// </summary>
    /// <param name="isMuted">Whether to mute SFX.</param>
    public void ToggleSFX(bool isMuted)
    {
        if (isMuted)
        {
            isSFXMuted = true;
            ambience.Pause();
            crane.Pause();
        } 
        else
        {
            isSFXMuted = false;
            ambience.Play();
            crane.Play();
        }
    }
}
