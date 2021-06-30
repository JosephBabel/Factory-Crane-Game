using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
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
    public float timeLeft { get; private set; }
    public int totalProfit { get; private set; }

    // Difficulty modifiers
    public float dropYHeight { get; private set; } // Items dropped by the claw from this height
    public float cannonBallMinSpawnTimer { get; private set; }
    public float cannonBallMaxSpawnTimer { get; private set; }
    public float itemSpawnTimer { get; private set; }

    private GameObject mainMenuUI;
    private GameObject gameOverlayUI;
    private TextMeshProUGUI timerText;
    private TextMeshProUGUI scoreText;

    private bool isPaused;
    private bool hasStartedOnce;
    
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
        {
            UpdateTime();
        }
        else
        {
            // END GAME
        }
    }

    IEnumerator SetUpGame()
    {
        if (hasStartedOnce)
        {
            AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
            while (!asyncLoadLevel.isDone)
                yield return null;

            LoadUI();
        }

        hasStartedOnce = true;
        HideMainMenu();
    }

    // Helper functions
    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0.0f;
        mainMenuUI.gameObject.SetActive(true);
    }

    void UnpauseGame()
    {
        isPaused = false;
        Time.timeScale = 1.0f;
        mainMenuUI.gameObject.SetActive(false);
    }

    void ShowMainMenu()
    {
        isRunning = false;
        Time.timeScale = 0.0f;
        mainMenuUI.gameObject.SetActive(true);
        gameOverlayUI.gameObject.SetActive(false);
    }

    void HideMainMenu()
    {
        isRunning = true;
        Time.timeScale = 1.0f;
        mainMenuUI.gameObject.SetActive(false);
        gameOverlayUI.gameObject.SetActive(true);
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
        mainMenuUI = GameObject.Find("Main Menu UI");
        gameOverlayUI = GameObject.Find("Game Overlay UI");
        mainMenuUI.SetActive(true);
        gameOverlayUI.SetActive(true);
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
    }

    // Button OnClick Events
    public void SelectEasyMode()
    {
        timeLeft = 180.0f;
        dropYHeight = 0.0f;
        cannonBallMinSpawnTimer = 4.0f;
        cannonBallMaxSpawnTimer = 6.0f;
        itemSpawnTimer = 7.0f;
        StartCoroutine(SetUpGame());
    }

    public void SelectMediumMode()
    {
        timeLeft = 180.0f;
        dropYHeight = 15.0f;
        cannonBallMinSpawnTimer = 3.0f;
        cannonBallMaxSpawnTimer = 5.0f;
        itemSpawnTimer = 6.0f;
        StartCoroutine(SetUpGame());
    }

    public void SelectHardMode()
    {
        timeLeft = 180.0f;
        dropYHeight = 15.0f;
        cannonBallMinSpawnTimer = 2.0f;
        cannonBallMaxSpawnTimer = 4.0f;
        itemSpawnTimer = 5.5f;
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
