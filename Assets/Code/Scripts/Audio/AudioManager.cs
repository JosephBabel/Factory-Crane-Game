using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    public Sound[] sounds;

    private IEnumerator startCraneFade;
    private IEnumerator stopCraneFade;

    private AudioSource music;
    private AudioSource ambience;
    private AudioSource crane;

    private float originalMusicVolume;
    private float originalAmbienceVolume;
    private float originalCraneVolume;

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

    private void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        music = audioSources[0];
        ambience = audioSources[1];
        ResetAudioSceneDependencies();
        originalMusicVolume = music.volume;
        originalAmbienceVolume = ambience.volume;
        originalCraneVolume = crane.volume;

        StopCraneSound();
    }

    public void ResetAudioSceneDependencies()
    {
        crane = GameObject.Find("Crane/Arm/Controller").GetComponent<AudioSource>();
    }

    public void StartCraneSound()
    {
        if (stopCraneFade != null)
        {
            StopCoroutine(stopCraneFade);
            stopCraneFade = null;
        }

        crane.Play();
        startCraneFade = FadeSound(crane, crane.volume, originalCraneVolume, 0.01f);
        StartCoroutine(startCraneFade);
    }

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

    IEnumerator FadeSound(AudioSource audioSource, float startVolume, float endVolume, float duration)
    {
        float timeElapsed = 0;
        while (audioSource != null && timeElapsed < duration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, endVolume, timeElapsed / duration);
            timeElapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        if (audioSource != null)
        {
            audioSource.volume = endVolume;

            if (audioSource.volume == 0)
                audioSource.Stop();
        }
    }

    public void PlayClipAt(string name, Vector3 position)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);
        if (sound == null)
        {
            Debug.LogError($"Error: Sound of name \"{name}\" does not exist.");
            return;
        }

        GameObject tempAudio = new GameObject("TempAudio");
        tempAudio.transform.position = position;
        AudioSource tempSource = tempAudio.AddComponent<AudioSource>();
        tempSource.clip = sound.clip;
        tempSource.volume = sound.volume;
        tempSource.pitch = sound.pitch;
        tempSource.minDistance = sound.minDistance;
        tempSource.maxDistance = sound.maxDistance;
        tempSource.spatialBlend = sound.spatialBlend;
        tempSource.Play();
        Destroy(tempAudio, tempSource.clip.length);
    }
}
