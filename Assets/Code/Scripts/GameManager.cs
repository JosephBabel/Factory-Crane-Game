using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    // ENCAPSULATION
    public static GameManager instance { get; private set; }
    public bool isRunning { get; private set; }
    public float timeLeft { get; private set; } = 150;
    public int totalProfit { get; private set; }

    // Difficulty modifiers
    public float dropYHeight { get; private set; } // Items dropped by the claw from this height
    public float cannonBallMinSpawnTimer { get; private set; }
    public float cannonBallMaxSpawnTimer { get; private set; }
    public float itemSpawnTimer { get; private set; }
    public float dropBoxSpeed { get; private set; }
    public float conveyorSpeed { get; private set; }

    private GameObject mainMenuUI;
    private GameObject gameOverlayUI;
    private GameObject gameOverUI;

    private CanvasGroup gameOverCanvasGroup;
    private CanvasGroup mainMenuCanvasGroup;

    private TextMeshProUGUI timerText;
    private TextMeshProUGUI scoreText;

    private bool isPaused;
    private bool hasStartedOnce;
    private bool isEnded;

    private Vector3 originalGravity;

    
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
            DontDestroyOnLoad(instance);
        }
    }

    void Start()
    {
        originalGravity = Physics.gravity;
        LoadUI();
        ShowMainMenu();
    }

    void Update()
    {
        if (isRunning && !isPaused && Input.GetKeyDown(KeyCode.Escape))
            PauseGame();
        else if (isPaused && Input.GetKeyDown(KeyCode.Escape))
            UnpauseGame();

        timeLeft -= Time.deltaTime;
        if (timeLeft > 0)
            UpdateTime();
        else if (!isEnded && timeLeft <= 0)
            EndGame();
    }

    IEnumerator SetUpGame()
    {
        if (hasStartedOnce)
        {
            AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
            while (!asyncLoadLevel.isDone)
                yield return null;

            LoadUI();
            AudioManager.instance.ResetAudioSceneDependencies();
        }
        
        hasStartedOnce = true;
        isPaused = false;
        isEnded = false;
        HideMainMenu();
    }

    IEnumerator Fade(float start, float end, float duration, System.Action<float> operation)
    {
        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            float value = Mathf.Lerp(start, end, timeElapsed / duration);
            timeElapsed += Time.unscaledDeltaTime;
            operation(value);
            yield return null;
        }
        operation(end);
    }

    // Helper functions
    void EndGame()
    {
        isEnded = true;
        gameOverCanvasGroup.alpha = 0;
        gameOverlayUI.SetActive(false);
        gameOverUI.SetActive(true);
        StartCoroutine(Fade(0f, 1f, 0.3f, x => { gameOverCanvasGroup.alpha = x; }));
        StartCoroutine(Fade(1f, 0f, 0.3f, x => { Time.timeScale = x; }));
    }

    void PauseGame()
    {
        isPaused = true;
        mainMenuCanvasGroup.alpha = 0f;
        mainMenuUI.SetActive(true);
        StartCoroutine(Fade(0f, 1f, 0.3f, x => { mainMenuCanvasGroup.alpha = x; }));
        StartCoroutine(Fade(1f, 0f, 0.3f, x => { Time.timeScale = x; }));
    }

    void UnpauseGame()
    {
        isPaused = false;
        StartCoroutine(Fade(1f, 0f, 0.3f, x => { mainMenuCanvasGroup.alpha = x; }));
        StartCoroutine(Fade(0f, 1f, 0.3f, x => { Time.timeScale = x; }));
    }

    void ShowMainMenu()
    {
        isRunning = false;
        Time.timeScale = 0.0f;
        mainMenuUI.SetActive(true);
        gameOverlayUI.SetActive(false);
        gameOverUI.SetActive(false);
    }

    void HideMainMenu()
    {
        isRunning = true;
        Time.timeScale = 1.0f;
        mainMenuUI.SetActive(false);
        gameOverlayUI.SetActive(true);
        gameOverUI.SetActive(false);
    }

    void UpdateTime()
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60.0f);
        int seconds = Mathf.FloorToInt(timeLeft - minutes * 60);
        string formattedTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        timerText.text = "TIME LEFT: " + formattedTime;
    }

    void LoadUI()
    {
        mainMenuUI = GameObject.Find("MainMenuUI");
        gameOverlayUI = GameObject.Find("GameOverlayUI");
        gameOverUI = GameObject.Find("GameOverUI");
        mainMenuCanvasGroup = mainMenuUI.GetComponent<CanvasGroup>();
        gameOverCanvasGroup = gameOverUI.GetComponent<CanvasGroup>();
        timerText = gameOverlayUI.transform.Find("TimerText").GetComponent<TextMeshProUGUI>();
        scoreText = gameOverlayUI.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();

        Button[] buttons = mainMenuUI.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            if (button.name == "EasyButton")
                button.onClick.AddListener(SelectEasyMode);
            else if (button.name == "MediumButton")
                button.onClick.AddListener(SelectMediumMode);
            else if (button.name == "HardButton")
                button.onClick.AddListener(SelectHardMode);
            else if (button.name == "QuitButton")
                button.onClick.AddListener(QuitGame);
        }

        Button mainMenuButton = gameOverUI.GetComponentInChildren<Button>();
        mainMenuButton.onClick.AddListener(ShowMainMenu);
    }

    // Button OnClick Events
    public void SelectEasyMode()
    {
        Physics.gravity = originalGravity;
        dropYHeight = 100f;
        cannonBallMinSpawnTimer = 4f;
        cannonBallMaxSpawnTimer = 7f;
        itemSpawnTimer = 7f;
        dropBoxSpeed = 5f;
        conveyorSpeed = 0.5f;
        timeLeft = 5;
        totalProfit = 0;
        StartCoroutine(SetUpGame());
    }

    public void SelectMediumMode()
    {
        Physics.gravity = originalGravity * 2f;
        dropYHeight = 15f;
        cannonBallMinSpawnTimer = 3f;
        cannonBallMaxSpawnTimer = 5f;
        itemSpawnTimer = 6f;
        dropBoxSpeed = 8f;
        conveyorSpeed = 1f;
        timeLeft = 150;
        totalProfit = 0;
        StartCoroutine(SetUpGame());
    }

    public void SelectHardMode()
    {
        Physics.gravity = originalGravity * 2.5f;
        dropYHeight = 15f;
        cannonBallMinSpawnTimer = 2f;
        cannonBallMaxSpawnTimer = 4f;
        itemSpawnTimer = 5.5f;
        dropBoxSpeed = 12f;
        conveyorSpeed = 1.5f;
        timeLeft = 150;
        totalProfit = 0;
        StartCoroutine(SetUpGame());
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void AddToProfit(int profit) 
    { 
        totalProfit += profit;
        scoreText.text = "PROFIT: $" + totalProfit;
    }
}
