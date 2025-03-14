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
        if (gameMusic != null)
        {
            gameMusic.Stop();
        }
    }
    else
    {
        if (gameMusic != null && !gameMusic.isPlaying)
        {
            gameMusic.Play(); 
        }
    }

    if (scene.buildIndex == 0)
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
    }

    PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
    if (playerHealth != null) playerHealth.ResetHealth();
}

    private void AssignUIElements()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (sceneIndex == 0 || sceneIndex == 1)
        {
            if (gameCanvas != null)
                gameCanvas.SetActive(false);
            return;
        }

        if (gameCanvas != null)
            gameCanvas.SetActive(true);

        if (scoreText == null)
            scoreText = GameObject.FindWithTag("ScoreText")?.GetComponent<TextMeshProUGUI>();

        if (levelText == null)
            levelText = GameObject.FindWithTag("LevelText")?.GetComponent<TextMeshProUGUI>();

        if (gameOverPanel == null)
            gameOverPanel = GameObject.FindWithTag("GameOverPanel");

        if (victoryPanel == null)
            victoryPanel = GameObject.FindWithTag("VictoryPanel");
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
        gameMusic.Stop();
        if (gameOverSound != null)
        {

            gameOverSound.Play();
        }
        if (gameOverPanel == null)
        {
            gameOverPanel = GameObject.FindWithTag("GameOverPanel");
            if (gameOverPanel == null)
                return;
        }

        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.ResetHealth();
        }

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
        if (confirmSound != null)
        {

            confirmSound.Play();
        }
        Time.timeScale = 1f;
        SceneManager.sceneLoaded += OnSceneLoadedAfterRestart;
        SceneManager.LoadScene(2);
    }

    public void LoadMainMenu()
    {
        if (confirmSound != null)
        {

            confirmSound.Play();
        }
        Time.timeScale = 1f;
        SceneManager.sceneLoaded += OnSceneLoadedAfterRestart;
        SceneManager.LoadScene(0);
    }

    private void OnSceneLoadedAfterRestart(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoadedAfterRestart;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }

        score = 0;
        UpdateScoreUI();
        InvokeRepeating("CheckLevelCompletion", 1f, 1f);
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
            Invoke("LoadNextLevel", 1f);
        }
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
            isLoadingNextLevel = false;
            Invoke("RestartLevelCheck", 1f);
        }
        else
        {
            FinishGame();
        }
    }

    private void RestartLevelCheck()
    {
        isLoadingNextLevel = false;
        Invoke("CheckLevelCompletion", 1f);
        InvokeRepeating("CheckLevelCompletion", 1f, 1f);
    }

    private void FinishGame()
    {
        score = 0;
        isLoadingNextLevel = false;
        UpdateScoreUI();

        CancelInvoke("CheckLevelCompletion");
        CancelInvoke("RestartLevelCheck");

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
