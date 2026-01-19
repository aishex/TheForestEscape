using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public AudioSource musicSource;
    [Range(0f, 1f)] public float targetVolume = 0.2f;
    public float fadeDuration = 3.0f;

    void Start()
    {
        if (musicSource != null)
        {
            musicSource.volume = 0f;
            musicSource.loop = true;
            if (!musicSource.isPlaying) musicSource.Play();
            StartCoroutine(FadeInMusic());
        }
    }

    IEnumerator FadeInMusic()
    {
        float currentTime = 0f;
        
        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, targetVolume, currentTime / fadeDuration);
            yield return null;
        }
        
        musicSource.volume = targetVolume;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("House"); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}