using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using System.Collections;
using System.Collections.Generic;

public class InventorySystem : MonoBehaviour
{
    [System.Serializable]
    public class Slot
    {
        public Image itemIcon;
        public TextMeshProUGUI amountText;
        public bool isFull;
        public string itemName;
        public int count;
    }

    public Transform slotsParent;
    public RectTransform selector;
    public List<Slot> slots = new List<Slot>();

    public Sprite batterySprite;
    public Sprite gemSprite;
    
    public FlashlightSystem flashlightScript;
    public GameObject gameOverPanel; 

    public static int savedBatteriesCount = 0;
    public static bool hasGemGlobal = false; 

    private int currentSlotIndex = 0;

    IEnumerator Start()
    {
        yield return null;
        InitializeSlots();
        
        if (savedBatteriesCount > 0)
        {
            AddItem("Battery", batterySprite, savedBatteriesCount);
        }

        if (hasGemGlobal)
        {
            AddItem("Gem", gemSprite, 1);
        }
        
        UpdateSelectorPosition();
    }

    void InitializeSlots()
    {
        slots.Clear();
        foreach (Transform child in slotsParent)
        {
            if (child.name.Contains("Selector")) continue;
            if (selector != null && child == selector.transform) continue;

            Transform iconObj = child.Find("ItemIcon");
            Transform countObj = child.Find("Count");
            if (iconObj == null || countObj == null) continue;

            Slot newSlot = new Slot();
            newSlot.itemIcon = iconObj.GetComponent<Image>();
            newSlot.itemIcon.preserveAspect = true;
            if (newSlot.itemIcon == null) newSlot.itemIcon = iconObj.gameObject.AddComponent<Image>();
            newSlot.amountText = countObj.GetComponent<TextMeshProUGUI>();
            if (newSlot.amountText == null) newSlot.amountText = countObj.gameObject.AddComponent<TextMeshProUGUI>();
            
            newSlot.isFull = false; newSlot.count = 0;
            newSlot.itemIcon.sprite = null; newSlot.itemIcon.color = new Color(1, 1, 1, 0);
            newSlot.amountText.text = ""; newSlot.amountText.enabled = false;
            slots.Add(newSlot);
        }
    }

    void Update()
    {
        if (gameOverPanel != null && gameOverPanel.activeSelf) return;
        if (slots.Count == 0) return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) ChangeSlot(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) ChangeSlot(4);

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f) { currentSlotIndex--; if (currentSlotIndex < 0) currentSlotIndex = slots.Count - 1; ChangeSlot(currentSlotIndex); }
        else if (scroll < 0f) { currentSlotIndex++; if (currentSlotIndex >= slots.Count) currentSlotIndex = 0; ChangeSlot(currentSlotIndex); }

        if (Input.GetMouseButtonDown(1))
        {
            UseCurrentItem();
        }
    }

    public void AddItem(string name, Sprite icon, int amount)
    {
        if (name == "Gem") hasGemGlobal = true;
        
        bool added = false;
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].isFull && slots[i].itemName == name)
            {
                slots[i].count += amount; UpdateSlotUI(i); added = true; break;
            }
        }
        if (!added)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (!slots[i].isFull)
                {
                    slots[i].isFull = true; slots[i].itemName = name; slots[i].count = amount; slots[i].itemIcon.sprite = icon;
                    UpdateSlotUI(i); added = true; break;
                }
            }
        }
        UpdateSavedBatteryCount();
    }

    void UseCurrentItem()
    {
        if (currentSlotIndex >= slots.Count) return;
        Slot current = slots[currentSlotIndex];

        if (!current.isFull) return;

        if (current.itemName == "Battery")
        {
            if (flashlightScript != null)
            {
                flashlightScript.AddBattery(60f);
                current.count--;
                if (current.count <= 0) ClearSlot(currentSlotIndex);
                else UpdateSlotUI(currentSlotIndex);
                UpdateSavedBatteryCount();
            }
        }
        else if (current.itemName == "Gem")
        {
            if (FountainWin.isPlayerNearFountain && FountainWin.currentFountain != null)
            {
                current.count--;
                if (current.count <= 0) ClearSlot(currentSlotIndex);
                
                hasGemGlobal = false;
                FountainWin.currentFountain.ActivateWinSequence();
            }
        }
    }

    void UpdateSavedBatteryCount() { int t=0; foreach(var s in slots) if(s.isFull && s.itemName=="Battery") t+=s.count; savedBatteriesCount=t; }
    void UpdateSlotUI(int index) { Slot s=slots[index]; s.itemIcon.enabled=true; s.itemIcon.color=Color.white; if(s.count>0){s.amountText.enabled=true; s.amountText.text=s.count.ToString();} else{s.amountText.enabled=false; s.amountText.text="";} }
    void ClearSlot(int index) { Slot s=slots[index]; s.isFull=false; s.itemName=""; s.count=0; s.itemIcon.sprite=null; s.itemIcon.color=new Color(1,1,1,0); s.amountText.text=""; s.amountText.enabled=false; }
    void ChangeSlot(int index) { if(index>=slots.Count)return; currentSlotIndex=index; UpdateSelectorPosition(); }
    void UpdateSelectorPosition() { if(selector!=null && slots.Count>0) { selector.pivot=new Vector2(0.5f,0.5f); if(slots[currentSlotIndex].itemIcon!=null) selector.position=slots[currentSlotIndex].itemIcon.transform.parent.position; } }
}