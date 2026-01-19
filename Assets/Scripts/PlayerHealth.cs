using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public static int savedHealth = 3;
    public int health;
    public int numOfHearts = 3; 
    public GameObject heartContainer;
    public Image[] hearts;       
    public Sprite fullHeart;     
    public Sprite emptyHeart;    
    public GameObject gameOverUI; 
    public Image flashScreen;     
    public CameraShake cameraShake;
    public bool isDead = false; 
    
    private Animator animator;
    private PlayerMovement movementScript;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        movementScript = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();

        if (cameraShake == null && Camera.main != null) 
            cameraShake = Camera.main.GetComponent<CameraShake>();

        health = savedHealth;

        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (flashScreen != null) flashScreen.color = new Color(0,0,0,0);
        if (heartContainer != null) heartContainer.SetActive(true);

        UpdateHearts();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        health -= damage;
        
        if (health > numOfHearts) health = numOfHearts;
        
        savedHealth = health;

        if (cameraShake != null) StartCoroutine(cameraShake.Shake(0.2f, 0.3f));
        
        UpdateHearts();

        if (health <= 0)
        {
            Die();
        }
    }

    void UpdateHearts()
    {
        if (hearts == null) return;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] == null) continue;

            if (i < health)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;

            if (i < numOfHearts) hearts[i].enabled = true;
            else hearts[i].enabled = false;
        }
    }

    void Die()
    {
        isDead = true;
        savedHealth = 3; 

        if (animator != null) animator.SetTrigger("Death"); 
        
        if (movementScript != null) 
        {
            if (movementScript.audioSource != null) movementScript.audioSource.Stop();
            movementScript.enabled = false;
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; 
            rb.bodyType = RigidbodyType2D.Static; 
        }

        if (heartContainer != null) heartContainer.SetActive(false);

        if (gameOverUI != null) gameOverUI.SetActive(true);
        if (flashScreen != null) StartCoroutine(FadeToBlack());
    }

    IEnumerator FadeToBlack()
    {
        float alpha = 0;
        yield return new WaitForSeconds(0.2f); 
        while (alpha < 1)
        {
            alpha += Time.deltaTime * 0.8f; 
            if (flashScreen != null) flashScreen.color = new Color(0f, 0f, 0f, alpha); 
            yield return null;
        }
    }
}