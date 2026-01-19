using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    private bool canPickup;
    private InventorySystem inventory;
    private GameObject playerLabel;
    public AudioClip pickupSound; 

    void Start()
    {
        inventory = FindObjectOfType<InventorySystem>();
    }

    void Update()
    {
        if (canPickup && Input.GetKeyDown(KeyCode.E))
        {
            if (inventory != null)
            {
                inventory.AddItem("Battery", inventory.batterySprite, 1);
                
                if (pickupSound != null)
                {
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                }

                if (playerLabel != null) playerLabel.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other) { if (other.CompareTag("Player")) { canPickup = true; Transform labelTr = other.transform.Find("InteractCanvas"); if (labelTr != null) { playerLabel = labelTr.gameObject; playerLabel.SetActive(true); } } }
    void OnTriggerExit2D(Collider2D other) { if (other.CompareTag("Player")) { canPickup = false; if (playerLabel != null) { playerLabel.SetActive(false); playerLabel = null; } } }
}