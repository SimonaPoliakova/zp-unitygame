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

        if (scene.buildIndex == 0 && gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
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

        EnsureSingleEventSystem();
    }

    private void EnsureSingleEventSystem()
    {
        EventSystem[] eventSystems = FindObjectsOfType<EventSystem>();
        if (eventSystems.Length > 1)
        {
            for (int i = 1; i < eventSystems.Length; i++)
            {
                Destroy(eventSystems[i].gameObject);
            }
        }
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
        if (gameOverPanel == null)
        {
            gameOverPanel = GameObject.FindWithTag("GameOverPanel");
            if (gameOverPanel == null)
                return;
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
        Time.timeScale = 1f;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
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
            Invoke("LoadNextLevel", 3f);
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
            SceneManager.LoadScene(0);
        }
    }

    private void RestartLevelCheck()
    {
        Invoke("CheckLevelCompletion", 1f);
        InvokeRepeating("CheckLevelCompletion", 1f, 1f);
    }
}