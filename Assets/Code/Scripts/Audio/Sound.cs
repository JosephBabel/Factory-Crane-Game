using UnityEngine;

/// <summary>
/// Contains information on how to construct an Audio Source component.
/// </summary>
[System.Serializable]
public class Sound
{
    [Header("Audio Source Settings")]
    public AudioClip clip;

    public string name;
    public bool bypassEffects;
    public bool bypassListenerEffects;
    public bool loop;
    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(.1f, 3f)]
    public float pitch = 1f;
    [Range(0f, 1f)]
    public float spatialBlend = 1f;
    public float minDistance = 10f;
    public float maxDistance = 500f;
}
