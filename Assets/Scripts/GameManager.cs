using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    private int score = 0;
    private bool isLoadingNextLevel = false;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText;
    public GameObject gameOverPanel;
    public GameObject gameCanvas;
    public GameObject victoryPanel;
    private AudioSource confirmSound;
    private AudioSource gameOverSound;
    private AudioSource gameMusic;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        gameCanvas = transform.Find("GameCanvas")?.gameObject;
        confirmSound = transform.Find("ConfirmSound")?.GetComponent<AudioSource>();
        gameOverSound = transform.Find("GameOverSound")?.GetComponent<AudioSource>();
        gameMusic = transform.Find("GameMusic")?.GetComponent<AudioSource>();
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;
        AssignUIElements();
        UpdateLevelUI();
        InvokeRepeating("CheckLevelCompletion", 1f, 1f);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignUIElements();
        UpdateLevelUI();

        if (scene.buildIndex == 0 || scene.buildIndex == 1)
        {
            gameMusic?.Stop();
        }
        else if (gameMusic != null && !gameMusic.isPlaying)
        {
            gameMusic.Play();
        }

        if (scene.buildIndex == 0)
        {
            gameOverPanel?.SetActive(false);
            victoryPanel?.SetActive(false);
        }

        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        playerHealth?.ResetHealth();
    }

    private void AssignUIElements()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (sceneIndex == 0 || sceneIndex == 1)
        {
            gameCanvas?.SetActive(false);
            return;
        }

        gameCanvas?.SetActive(true);
        scoreText ??= GameObject.FindWithTag("ScoreText")?.GetComponent<TextMeshProUGUI>();
        levelText ??= GameObject.FindWithTag("LevelText")?.GetComponent<TextMeshProUGUI>();
        gameOverPanel ??= GameObject.FindWithTag("GameOverPanel");
        victoryPanel ??= GameObject.FindWithTag("VictoryPanel");
    }

    private void UpdateLevelUI()
    {
        if (levelText != null)
        {
            int levelNumber = SceneManager.GetActiveScene().buildIndex - 1;
            levelText.text = levelNumber.ToString("00");
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText == null)
            scoreText = GameObject.FindWithTag("ScoreText")?.GetComponent<TextMeshProUGUI>();

        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    public void ShowGameOver()
    {
        gameMusic?.Stop();
        gameOverSound?.Play();

        gameOverPanel ??= GameObject.FindWithTag("GameOverPanel");
        if (gameOverPanel == null) return;

        FindObjectOfType<PlayerHealth>()?.ResetHealth();
        score = 0;
        UpdateScoreUI();

        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;

        foreach (UnityEngine.UI.Button button in gameOverPanel.GetComponentsInChildren<UnityEngine.UI.Button>(true))
        {
            button.interactable = true;
        }
    }

    public void RestartGame()
    {
        confirmSound?.Play();
        Time.timeScale = 1f;
        StartCoroutine(LoadSceneAsync(2));
        StartCoroutine(DelayedAction());

        IEnumerator DelayedAction()
        {
            yield return new WaitForSeconds(0.2f);
            gameOverPanel.SetActive(false);
        }
    }

    public void LoadMainMenu()
    {
        confirmSound?.Play();
        Time.timeScale = 1f;
        StartCoroutine(LoadSceneAsync(0));
    }

    private void CheckLevelCompletion()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex == 0 || sceneIndex == 1) return;

        bool noEnemiesLeft = GameObject.FindGameObjectsWithTag("Enemy").Length == 0;
        bool noFruitsLeft = GameObject.FindGameObjectsWithTag("Fruit").Length == 0;
        bool noTrappedEnemiesLeft = GameObject.FindGameObjectsWithTag("Enemy_Trapped").Length == 0;

        if (noEnemiesLeft && noFruitsLeft && noTrappedEnemiesLeft && !isLoadingNextLevel)
        {
            isLoadingNextLevel = true;
            CancelInvoke("CheckLevelCompletion");
            StartCoroutine(LoadNextLevel());
        }
    }

    private IEnumerator LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            yield return LoadSceneAsync(nextSceneIndex);
        }
        else
        {
            FinishGame();
        }
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
            yield return null;
        }

        isLoadingNextLevel = false;
        InvokeRepeating("CheckLevelCompletion", 1f, 1f);
    }

    private void FinishGame()
    {
        score = 0;
        isLoadingNextLevel = false;
        UpdateScoreUI();

        CancelInvoke("CheckLevelCompletion");

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            Debug.LogError("VictoryPanel is NOT assigned in the Inspector!");
        }
    }
}
