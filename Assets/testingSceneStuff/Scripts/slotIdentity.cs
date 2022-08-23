using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class slotIdentity : NetworkBehaviour
{
    public int slotNumber;

    public inventoryUI inventoryItemMovement;
    public inventory inventoryList;
    public playerMovement playerScript;
    public Database gameDatabase;

    public playerLocator playerLocateScript;

    public Image nullImage;

    public GameObject destroyItem;
    public GameObject canvasObject;

    // Start is called before the first frame update
    void Start()
    {
        canvasObject = GameObject.Find("Canvas(Clone)");
        playerLocateScript = canvasObject.GetComponent<playerLocator>();
        GameObject playerObject = playerLocateScript.player;
        inventoryList = playerObject.GetComponent<inventory>();
        playerScript = playerObject.GetComponent<playerMovement>();
        GameObject itemDatabase = GameObject.Find("Database(Clone)");
        gameDatabase = itemDatabase.GetComponent<Database>();

    }

    // Update is called once per frame
    void Update()
    {
        if(inventoryItemMovement == null)
        {
            if(playerScript.inventoryOpen == true)
            {
                GameObject bigInv = GameObject.Find("bigInventory");
                inventoryItemMovement = bigInv.GetComponent<inventoryUI>();
            }
        }
    }

    public void whenClicked()
    {
        inventoryItemMovement.moveItemImage.enabled = true;
        if (inventoryItemMovement.movingItemID == 0)
        {
            inventoryItemMovement.moveItemImage.sprite = gameDatabase.itemImages[inventoryList.Inventory[slotNumber].id];
            inventoryItemMovement.movingItemID = inventoryList.Inventory[slotNumber].id;
            //If we are not currently moving an item set the movingItemID to the id of the item in this slot.
            inventoryList.Inventory[slotNumber].id = 0;
            //Set the item in the slot we clicked to 0, empty.
            UnityEngine.UI.Image imageToChange = inventoryList.images[slotNumber].GetComponent<UnityEngine.UI.Image>();
            //Get the slotImage, we have to get the component so we have to do it on its own line.
            imageToChange.sprite = nullImage.GetComponent<Sprite>();
            //Set that slotImage we gotComponent of to null.

            //This if statement runs if we are picking up an object from a slot, and we are not currently moving an item.
        }
        else if(inventoryItemMovement.movingItemID != 0 && inventoryList.Inventory[slotNumber].id ==0 )
        {
            UnityEngine.UI.Image imageToChange = inventoryList.images[slotNumber].GetComponent<UnityEngine.UI.Image>();
            //Get the slotImage, we have to get the component so we have to do it on its own line.
            imageToChange.sprite = gameDatabase.itemImages[inventoryItemMovement.movingItemID];
            //Set that slotImage we gotComponent of to the item image for the item that we are moving.
            inventoryList.Inventory[slotNumber].id = inventoryItemMovement.movingItemID;
            //If we are moving an item set the item ID in this slot to the item we are moving.
            inventoryItemMovement.movingItemID = 0;
            //Set our movingItemId to 0 as we have placed item in slot.
            inventoryItemMovement.moveItemImage.enabled = false;
            //Set our movingItemImage to enabled false due to us placing the item down.

            //This if statement runs if we are currently moving an item but there is not an item in the slot.
        }
        else if(inventoryItemMovement.movingItemID != 0 && inventoryList.Inventory[slotNumber].id != 0)
        {
            int itemInSlot = inventoryList.Inventory[slotNumber].id;
            UnityEngine.UI.Image imageToChange = inventoryList.images[slotNumber].GetComponent<UnityEngine.UI.Image>();
            //Get the slotImage, we have to get the component so we have to do it on its own line.
            imageToChange.sprite = gameDatabase.itemImages[inventoryItemMovement.movingItemID];
            //Set that slotImage we gotComponent of to the item image for the item that we are moving.
            inventoryList.Inventory[slotNumber].id = inventoryItemMovement.movingItemID;
            //If we are moving an item set the item ID in this slot to the item we are moving.
            inventoryItemMovement.movingItemID = itemInSlot;
            //Set our movingItemId to the item that was in the slot,
            inventoryItemMovement.moveItemImage.sprite = gameDatabase.itemImages[itemInSlot];
            //Set our movingItemImage to the image of the item that was in the slot.

            //This if statement will run if there is an item in the slot we click and we are currently moving an item, it swaps the two.

        }

        if(playerScript.inSlot == slotNumber)
        {
            if (playerScript.itemToPlace != null)
            {
                playerScript.destroyForSlotIdentity();
            }
        }


    }
}
