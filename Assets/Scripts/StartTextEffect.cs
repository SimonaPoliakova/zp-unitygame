using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartTextEffect : MonoBehaviour
{
    public TextMeshProUGUI startText;
    public GameObject pressAnyKeyText; // Reference to the "Press Any Key" text
    public float typeSpeed = 0.05f;

    private string fullText = "NOW IT IS THE BEGINNING OF A FANTASTIC STORY!!\nLET'S MAKE A JOURNEY TO THE CAVE OF MONSTERS!\nGOOD LUCK!\n\n";
    private bool isTyping = false;

    private void Start()
    {
        startText.text = "";
        pressAnyKeyText.SetActive(false); // Hide the start text initially
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        isTyping = true;

        for (int i = 0; i < fullText.Length; i++)
        {
            startText.text += fullText[i];
            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
        pressAnyKeyText.SetActive(true); // Show the text after typing ends
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
