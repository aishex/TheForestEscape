using UnityEngine;

public class GameOverSound : MonoBehaviour
{
    public AudioClip gameOverClip;
    [Range(0f, 1f)]
    public float volume = 0.5f;

    void OnEnable()
    {
        if (gameOverClip != null)
        {
            if (Camera.main != null)
            {
                AudioSource.PlayClipAtPoint(gameOverClip, Camera.main.transform.position, volume);
            }
            
            GameObject bgMusic = GameObject.Find("BackgroundMusic");
            if (bgMusic != null) bgMusic.SetActive(false);
        }
    }
}