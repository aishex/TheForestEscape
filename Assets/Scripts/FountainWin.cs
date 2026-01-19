using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class FountainWin : MonoBehaviour
{
    public CanvasGroup winScreenGroup;
    public Light2D purpleLight;
    public float fadeSpeed = 0.5f;

    public AudioClip portalSound;
    [Range(0f, 1f)] public float portalMaxVolume = 0.5f; 
    public float soundFadeSpeed = 1.0f; 

    public AudioClip winMusicClip;
    [Range(0f, 1f)] public float winMusicVolume = 0.5f;

    public static bool isPlayerNearFountain = false;
    public static FountainWin currentFountain;

    private bool gameEnded = false;
    private AudioSource audioSource;

    void Start()
    {
        if (purpleLight != null) purpleLight.intensity = 0;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 0f; 
    }

    public void ActivateWinSequence()
    {
        if (gameEnded) return;
        gameEnded = true;

        StartCoroutine(WinAnimation());
    }

    IEnumerator WinAnimation()
    {
        GameObject bgMusic = GameObject.Find("BackgroundMusic");
        if (bgMusic != null) bgMusic.SetActive(false);

        if (portalSound != null)
        {
            audioSource.clip = portalSound;
            audioSource.volume = 0f; 
            audioSource.loop = false; 
            audioSource.Play();

            float v = 0f;
            while (v < portalMaxVolume)
            {
                v += Time.deltaTime * soundFadeSpeed;
                audioSource.volume = v;
                yield return null;
            }
        }

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 1.5f;
            if (purpleLight != null) purpleLight.intensity = Mathf.Lerp(0f, 5f, t);
            yield return null;
        }

        float flickerDuration = 2.0f;
        float stopTime = Time.time + flickerDuration;
        
        while (Time.time < stopTime)
        {
            if (purpleLight != null)
            {
                purpleLight.intensity = Random.Range(3.0f, 6.0f);
                purpleLight.pointLightOuterRadius = Random.Range(4.5f, 5.5f);
            }
            yield return new WaitForSeconds(0.08f);
        }

        if (purpleLight != null) purpleLight.intensity = 8.0f;
        
        if (winMusicClip != null)
        {
            audioSource.Stop(); 
            audioSource.clip = winMusicClip;
            audioSource.volume = winMusicVolume;
            audioSource.Play();
        }

        yield return new WaitForSeconds(0.2f);

        if (winScreenGroup != null)
        {
            winScreenGroup.gameObject.SetActive(true);
            winScreenGroup.alpha = 0f;
            winScreenGroup.blocksRaycasts = true;
        }
        
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            if (winScreenGroup != null) winScreenGroup.alpha = alpha;
            yield return null;
        }
        
        if (winScreenGroup != null) winScreenGroup.alpha = 1f;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearFountain = true;
            currentFountain = this;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearFountain = false;
            currentFountain = null;
        }
    }
}