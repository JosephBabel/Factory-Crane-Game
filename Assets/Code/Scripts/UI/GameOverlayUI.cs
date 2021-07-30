using UnityEngine;
using TMPro;

public class GameOverlayUI : UI
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;

    public TextAnimationController timerAnimation => timerText.GetComponent<TextAnimationController>();
    public TextAnimationController scoreAnimation => scoreText.GetComponent<TextAnimationController>();
}
