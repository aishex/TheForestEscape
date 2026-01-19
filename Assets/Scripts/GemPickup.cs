using UnityEngine;
using UnityEngine.SceneManagement;

public class GemPickup : MonoBehaviour
{
    public Sprite gemIcon; 
    public AudioClip gemPickupSound;
    
    private bool canPickup;
    private InventorySystem inventory;
    private GameObject playerLabel;

    void Start()
    {
        inventory = FindObjectOfType<InventorySystem>();
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene != GemManager.chosenSceneName) Destroy(gameObject); 
    }

    void Update()
    {
        if (canPickup && Input.GetKeyDown(KeyCode.E))
        {
            if (inventory != null)
            {
                inventory.AddItem("Gem", gemIcon, 1);
                
                if (gemPickupSound != null)
                {
                    AudioSource.PlayClipAtPoint(gemPickupSound, transform.position);
                }

                if (playerLabel != null) playerLabel.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other) { if (other.CompareTag("Player")) { canPickup = true; Transform labelTr = other.transform.Find("InteractCanvas"); if (labelTr != null) { playerLabel = labelTr.gameObject; playerLabel.SetActive(true); } } }
    void OnTriggerExit2D(Collider2D other) { if (other.CompareTag("Player")) { canPickup = false; if (playerLabel != null) { playerLabel.SetActive(false); playerLabel = null; } } }
}