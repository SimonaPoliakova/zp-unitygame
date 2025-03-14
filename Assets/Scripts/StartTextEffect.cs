using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartTextEffect : MonoBehaviour
{
    public TextMeshProUGUI startText;
    public GameObject pressAnyKeyText;
    public float typeSpeed = 0.05f;

    private string fullText = "NOW IT IS THE BEGINNING OF A FANTASTIC STORY!!\nLET'S MAKE A JOURNEY TO THE CAVE OF MONSTERS!\nGOOD LUCK!\n\n";
    private bool isTyping = false;
    private AudioSource typingSound;
    private bool hasPlayedSound = false;

    private void Awake()
    {
        typingSound = transform.Find("TypingSound")?.GetComponent<AudioSource>();
    }

    private void Start()
    {
        startText.text = "";
        pressAnyKeyText.SetActive(false);
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        isTyping = true;

        if (typingSound != null && !hasPlayedSound)
        {
            typingSound.Play();
            hasPlayedSound = true;
        }

        for (int i = 0; i < fullText.Length; i++)
        {
            startText.text += fullText[i];
            yield return new WaitForSeconds(typeSpeed);
        }

        if (typingSound != null)
        {
            typingSound.Stop();
        }

        isTyping = false;
        pressAnyKeyText.SetActive(true);
    }

    private void Update()
    {
        if (!isTyping && Input.anyKeyDown)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }
}
