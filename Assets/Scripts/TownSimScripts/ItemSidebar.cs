using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSidebar : MonoBehaviour
{
    public TownGameManager gameManager;
    public RecordsManager recordsManager;

    [Header("Default Settings")]

    public string defaultDescription;

    [Header("Description")]

    public TMP_Text descriptionTextbox;

    [Header("Currently Held")]

    public TMP_Text currentHeldTextbox;

    [Header("Item Sprite")]

    public Image spriteDisplay;

    void Start()
    {
        gameManager = TownGameManager.i;
    }
    void Update()
    {
        if (recordsManager.selectedBanner != null)
        {
            if (gameManager.Inventory.ContainsKey(recordsManager.selectedBanner.itemID))
            {

                //descriptionContainer.SetActive(true);

                UpdateDescription(recordsManager.selectedBanner.itemID.Description);
                UpdateCurrentlyHeld(gameManager.Inventory[recordsManager.selectedBanner.itemID]);
                UpdateDisplaySprite(recordsManager.selectedBanner.itemID.sprite);
            }
            else
            {
                //descriptionContainer.SetActive(true);

                UpdateDescription(recordsManager.selectedBanner.itemID.Description);
                UpdateCurrentlyHeld(0);
                UpdateDisplaySprite(recordsManager.selectedBanner.itemID.sprite);
            }
        }
        else
        {
            UpdateDescription(defaultDescription);
            UpdateCurrentlyHeld();
            UpdateDisplaySprite();
        }
    }
    void UpdateDescription(string newDesc)
    {
        if(descriptionTextbox != null)
        {
            descriptionTextbox.text = newDesc;
        }
    }

    void UpdateCurrentlyHeld(int newCount=-1)
    {
        if (currentHeldTextbox != null)
        {
            if (newCount != -1)
            {
                currentHeldTextbox.text = "Currently Held: " + newCount.ToString();
            }
            else
            {
                currentHeldTextbox.text = "Currently Held: ?";
            }
        }
    }

    void UpdateDisplaySprite(Sprite sprite = null)
    {
        if (spriteDisplay != null)
        {

            if (sprite == null)
            {
                spriteDisplay.gameObject.SetActive(false);
            }
            else
            {
                spriteDisplay.gameObject.SetActive(true);
            }

            spriteDisplay.sprite = sprite;
        }
           
    }
}
