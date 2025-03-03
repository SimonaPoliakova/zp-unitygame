using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private int score = 0;
    private bool isLoadingNextLevel = false;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText; // 🔹 Added Level Text reference
    public GameObject gameOverPanel;

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
    }

    private void Start()
    {
        AssignUIElements();
        UpdateLevelUI(); // 🔹 Update level number
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
        UpdateLevelUI(); // 🔹 Update level number when the scene changes
    }

    private void AssignUIElements()
    {
        if (scoreText == null)
        {
            GameObject scoreObj = GameObject.FindWithTag("ScoreText");
            if (scoreObj != null)
                scoreText = scoreObj.GetComponent<TextMeshProUGUI>();
        }

        if (levelText == null)
        {
            GameObject levelObj = GameObject.FindWithTag("LevelText");
            if (levelObj != null)
                levelText = levelObj.GetComponent<TextMeshProUGUI>();
        }

        if (gameOverPanel == null)
        {
            GameObject panelObj = GameObject.FindWithTag("GameOverPanel");
            if (panelObj != null)
                gameOverPanel = panelObj;
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
        {
            GameObject scoreObj = GameObject.FindWithTag("ScoreText");
            if (scoreObj != null)
                scoreText = scoreObj.GetComponent<TextMeshProUGUI>();
        }

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

        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        ResetGame();
    }

    private void ResetGame()
    {
        score = 0;
        UpdateScoreUI();
    }

    private void CheckLevelCompletion()
    {
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
