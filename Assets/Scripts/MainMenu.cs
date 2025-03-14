using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    private AudioSource confirmSound;
    private AudioSource exitSound;

    private void Awake()
    {
        confirmSound = transform.Find("ConfirmSound")?.GetComponent<AudioSource>();
        exitSound = transform.Find("ExitSound")?.GetComponent<AudioSource>();
    }

    public void StartGame()
    {
        if (confirmSound != null)
        {
            StartCoroutine(PlaySoundAndLoadScene(confirmSound, "StartingScreen"));
        }
        else
        {
            SceneManager.LoadScene("StartingScreen");
        }
    }

    public void ExitGame()
    {
        if (exitSound != null)
        {
            StartCoroutine(PlaySoundAndExit(exitSound));
        }
        else
        {
            Application.Quit();
        }
    }

    private IEnumerator PlaySoundAndLoadScene(AudioSource audioSource, string sceneName)
    {
        audioSource.Play();
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator PlaySoundAndExit(AudioSource audioSource)
    {
        audioSource.Play();
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        Application.Quit();
    }
}
