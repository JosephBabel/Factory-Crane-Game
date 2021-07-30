using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Adds animation to TextMeshPro text to change its font size, transform, and opacity over time.
/// </summary>
public class TextAnimationController : MonoBehaviour
{
    // Inspector settings
    [SerializeField] private bool followMainCamera = false;
    [SerializeField] private bool playAnimationOnAwake = false;
    [SerializeField] private bool returnToDefaultStateOnAnimationEnd = false;
    [SerializeField] private bool destroyOnAnimationEnd = false;
    [Header("Text Animation Settings")]
    [SerializeField] private float duration = 2f;
    [SerializeField] private Vector3 newPositionOffset;
    [SerializeField] private float newFontSize = 0f;
    [SerializeField] private Color newColor;
    
    private float originalFontSize;
    private Color originalColor;
    private Vector3 originalPosition;

    private TextMeshPro textMeshPro => GetComponent<TextMeshPro>();
    private TextMeshProUGUI textMeshProUGUI => GetComponent<TextMeshProUGUI>();

    void Awake()
    {
        if (!textMeshPro && !textMeshProUGUI)
            Debug.LogError("Error: TextAnimationController script requires either TextMeshPro or TextMeshProUGUI component.");
        else if (textMeshPro && textMeshProUGUI)
            Debug.LogError("Warning: Both TextMeshPro and TextMeshProUGUI components detected. Animation defaulting to TextMeshPro component.");
    }

    void Start()
    {
        if (textMeshPro)
        {
            originalPosition = transform.position;
            originalFontSize = textMeshPro.fontSize;
            originalColor = textMeshPro.color;
        }
        else if (textMeshProUGUI)
        {
            originalFontSize = textMeshProUGUI.fontSize;
            originalColor = textMeshProUGUI.color;
        }

        if (playAnimationOnAwake)
            StartAnimation(); 
    }

    void Update()
    {
        // Follow camera
        if (followMainCamera)
            transform.rotation = Camera.main.transform.rotation;
    }

    private void PlayAnimationOnce()
    {
        StartCoroutine(PlayAnimation(originalPosition, originalPosition + newPositionOffset, originalFontSize, newFontSize, originalColor, newColor, () => 
        {
            if (returnToDefaultStateOnAnimationEnd)
                ReverseAnimation();
            else if (destroyOnAnimationEnd)
                Destroy(gameObject);
        }));
    }

    private void ReverseAnimation()
    {
        StartCoroutine(PlayAnimation(originalPosition + newPositionOffset, originalPosition, newFontSize, originalFontSize, newColor, originalColor, () => 
        {
            if (destroyOnAnimationEnd)
                Destroy(gameObject);
        })) ;
    }

    IEnumerator PlayAnimation(Vector3 startPosition, Vector3 endPosition, float startFontSize, float endFontSize, Color startColor, Color endColor, System.Action onEndCallback)
    {
        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            if (textMeshPro)
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, timeElapsed / duration);
                textMeshPro.fontSize = Mathf.Lerp(startFontSize, endFontSize, timeElapsed / duration);
                textMeshPro.color = Color.Lerp(startColor, endColor, timeElapsed / duration);
            }
            else if (textMeshProUGUI)
            {
                textMeshProUGUI.fontSize = Mathf.Lerp(startFontSize, endFontSize, timeElapsed / duration);
                textMeshProUGUI.color = Color.Lerp(startColor, endColor, timeElapsed / duration);
            }

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        if (textMeshPro)
        {
            transform.position = endPosition;
            textMeshPro.fontSize = endFontSize;
            textMeshPro.color = endColor;
        }
        else if (textMeshProUGUI)
        {
            textMeshProUGUI.fontSize = endFontSize;
            textMeshProUGUI.color = endColor;
        }

        onEndCallback();
    }

    /// <summary>
    /// Start animation of TextMeshPro or TextMeshProUGUI component.
    /// </summary>
    public void StartAnimation() { PlayAnimationOnce(); }
}
