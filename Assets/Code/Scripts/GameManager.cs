using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    // ENCAPSULATION
    public static GameManager instance { get; private set; }
    public bool isRunning { get; private set; }
    public float timeLeft { get; private set; }

    // Difficulty modifiers
    public float dropYHeight { get; private set; }
    public float cannonBallMinSpawnTimer { get; private set; }
    public float cannonBallMaxSpawnTimer { get; private set; }
    public float itemSpawnTimer { get; private set; }
    public float dropBoxSpeed { get; private set; }
    public float conveyorSpeed { get; private set; }

    // Counters
    public int totalProfit { get; private set; }
    public int bombsExplodedCounter { get; private set; }
    public int itemsDroppedCounter { get; private set; }
    public int itemsLaunchedCounter { get; private set; }

    // UI Prefabs
    public GameObject mainMenuUIPrefab;
    public GameObject gameOverlayUIPrefab;
    public GameObject gameOverUIPrefab;

    // UI
    private MainMenuUI mainMenuUI;
    private GameOverlayUI gameOverlayUI;
    private GameOverUI gameOverUI;

    // Game states
    private bool isPaused;
    private bool hasStartedOnce;
    private bool isEnded;

    private Vector3 originalGravity;
    private float oneSecondTimer = 1f;

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
        ResetGameValues();
        OpenMainMenu();
    }

    void Update()
    {
        if (isRunning && !isPaused && Input.GetKeyDown(KeyCode.Escape))
            PauseGame();
        else if (isPaused && Input.GetKeyDown(KeyCode.Escape))
            UnpauseGame();

        // Game timer
        if (isRunning)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft > 0)
                UpdateTime();
            else if (!isEnded && timeLeft <= 0)
                EndGame();

            // Running out of time animation
            if (timeLeft <= 11f)
            {
                oneSecondTimer += Time.deltaTime;
                if (oneSecondTimer >= 1)
                {
                    gameOverlayUI.timerAnimation.StartAnimation();
                    oneSecondTimer = 0f;
                }
            }
        }
    }

    IEnumerator SetUpGame()
    {
        // Load game
        if (hasStartedOnce)
        {
            AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
            while (!asyncLoadLevel.isDone)
                yield return null;
        }

        // Finished loading
        hasStartedOnce = true; // Prevents game from reloading for first time
        isPaused = false;
        isEnded = false;
        ResetGameValues();
        CloseMainMenu();
        gameOverlayUI = Instantiate(gameOverlayUIPrefab).GetComponent<GameOverlayUI>();
    }

    // ABSTRACTION
    void EndGame()
    {
        isRunning = false;
        isEnded = true;
        Time.timeScale = 0f;
        gameOverlayUI.CloseUI();
        gameOverUI = Instantiate(gameOverUIPrefab).GetComponent<GameOverUI>();
        gameOverUI.totalProfitText.text = "$" + totalProfit.ToString();
        gameOverUI.bombsExplodedText.text = bombsExplodedCounter.ToString();
        gameOverUI.itemsDroppedText.text = itemsDroppedCounter.ToString();
        gameOverUI.itemsLaunchedText.text = itemsLaunchedCounter.ToString();
    }

    void PauseGame()
    {
        isRunning = false;
        isPaused = true;
        Time.timeScale = 0f;
        mainMenuUI = Instantiate(mainMenuUIPrefab).GetComponent<MainMenuUI>();
    }

    void UnpauseGame()
    {
        isRunning = true;
        isPaused = false;
        Time.timeScale = 1f;
        mainMenuUI.CloseUI();
    }

    void CloseMainMenu()
    {
        isRunning = true;
        Time.timeScale = 1f;
    }

    void UpdateTime()
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60.0f);
        int seconds = Mathf.FloorToInt(timeLeft - minutes * 60);
        string formattedTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        gameOverlayUI.timerText.text = "TIME LEFT: " + formattedTime;
    }

    void ResetGameValues()
    {
        timeLeft = 120f;
        totalProfit = 0;
        bombsExplodedCounter = 0;
        itemsDroppedCounter = 0;
        itemsLaunchedCounter = 0;
    }

    /// <summary>
    /// Instantiates MainMenuUI GameObject.
    /// </summary>
    public void OpenMainMenu()
    {
        isRunning = false;
        Time.timeScale = 0f;
        mainMenuUI = Instantiate(mainMenuUIPrefab).GetComponent<MainMenuUI>();
    }

    /// <summary>
    /// Update game values for easy difficulty.
    /// </summary>
    public void SelectEasyMode()
    {
        Physics.gravity = originalGravity;
        dropYHeight = 15f;
        cannonBallMinSpawnTimer = 4f;
        cannonBallMaxSpawnTimer = 7f;
        itemSpawnTimer = 7f;
        dropBoxSpeed = 5f;
        conveyorSpeed = 0.5f;
        StartCoroutine(SetUpGame());
    }

    /// <summary>
    /// Update game values for medium difficulty.
    /// </summary>
    public void SelectMediumMode()
    {
        Physics.gravity = originalGravity * 2f;
        dropYHeight = 15f;
        cannonBallMinSpawnTimer = 3f;
        cannonBallMaxSpawnTimer = 5f;
        itemSpawnTimer = 6f;
        dropBoxSpeed = 8f;
        conveyorSpeed = 1f;
        StartCoroutine(SetUpGame());
    }

    /// <summary>
    /// Update game values for hard difficulty.
    /// </summary>
    public void SelectHardMode()
    {
        Physics.gravity = originalGravity * 2.5f;
        dropYHeight = 15f;
        cannonBallMinSpawnTimer = 2f;
        cannonBallMaxSpawnTimer = 4f;
        itemSpawnTimer = 5.5f;
        dropBoxSpeed = 12f;
        conveyorSpeed = 1.5f;
        StartCoroutine(SetUpGame());
    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// Adds profit to totalProfit and updates text in GameOverlayUI
    /// </summary>
    /// <param name="profit">profit value to add to totalProfit.</param>
    public void AddToProfit(int profit) 
    { 
        if (gameOverlayUI != null)
        {
            totalProfit += profit;
            gameOverlayUI.scoreText.text = "PROFIT: $" + totalProfit.ToString("N0");
            gameOverlayUI.scoreAnimation.StartAnimation();
        }
    }

    /// <summary>
    /// Increments number of bombs exploded
    /// </summary>
    public void IncrementBombsExploded() { bombsExplodedCounter++; }

    /// <summary>
    /// Increments number of items dropped
    /// </summary>
    public void IncrementItemsDropped() { itemsDroppedCounter++; }

    /// <summary>
    /// Increments number of items launched
    /// </summary>
    public void IncrementItemsLaunched() { itemsLaunchedCounter++; }
}
