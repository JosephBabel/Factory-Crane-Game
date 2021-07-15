using System.Collections;
using UnityEngine;

/// <summary>
/// Base UI class that includes fade animation and ability close out of UI elements.
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class UI : MonoBehaviour
{
    // Inspector settings
    public float fadeInDuration = 0.5f;
    public float fadeOutDuration = 0.5f;

    private CanvasGroup canvasGroup;

    private float originaCanvasAlpha;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        originaCanvasAlpha = canvasGroup.alpha;
        canvasGroup.alpha = 0f;
        FadeIn();

        AddButtonEvents();
    }

    void FadeIn() 
    {
        StartCoroutine(Fade(canvasGroup, 0f, originaCanvasAlpha, fadeInDuration));
    }
    void FadeOut() 
    { 
        StartCoroutine(Fade(canvasGroup, originaCanvasAlpha, 0f, fadeOutDuration));
        canvasGroup.interactable = false;
    }

    IEnumerator Fade(CanvasGroup canvas, float start, float end, float duration)
    {
        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            canvas.alpha = Mathf.Lerp(start, end, timeElapsed / duration);
            timeElapsed += Time.unscaledDeltaTime;

            yield return null;
        }
        canvas.alpha = end;
    }

    protected virtual void AddButtonEvents() { }

    public void CloseUI()
    {
        FadeOut();
        Destroy(gameObject, fadeOutDuration);
    }
}
