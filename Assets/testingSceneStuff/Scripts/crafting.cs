using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crafting : MonoBehaviour
{
    public craftingInfo[] recipe;
    public craftingInfo[] itemCrafted;

    int idToFind;
    int amountToDestroy;

    public inventoryUI inventoryUi;

    public inventory playerInventory;

    public playerMovement playerScript;

    public GameObject Player;
    public GameObject inventoryUIObject;

    bool goAhead = false;

    // Start is called before the first frame update
    void Start()
    {
        inventoryUIObject = GameObject.Find("bigInventory");
        inventoryUi = inventoryUIObject.GetComponent<inventoryUI>();
        Player = inventoryUi.Player;
        playerInventory = Player.GetComponent<inventory>();
        playerScript = Player.GetComponent<playerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void craft()
    {
        for (int i = 0; i < recipe.Length; i++)
        {
            if(inventoryUi.itemAmounts[recipe[i].objectID] >= recipe[i].objectAmount)
            {
                goAhead = true;
                //If the inventory has theid we need in the recipe, and more than or equal to the amount we need, we will set it to true.
            }
            else
            {
                goAhead = false;
                //If the inventory does not have the id we need in the recipe,  or and more than or equal to the amount we need, we will set it to false and the removeObjectsFromInventory will not run.
                break;
                //We as well will break the loop here so the goAhead bool will not be set to true in this instance.
            }
        }

        if(goAhead == true)
        {
            findWhattoRemove();
            //This only runs if the goAhead bool is never set to false, and thus we have all the stuff we need for crafting, we call the function that finds what item id's need to be removed.
        }

    }

    public void findWhattoRemove()
    {
        for (int i = 0; i < recipe.Length; i++)
        {
            idToFind = recipe[i].objectID;
            //id to find is the id in the recipe array instance that we are currently on.
            amountToDestroy = recipe[i].objectAmount;
            //set amountToDestroy to the object amount necessary for crafting so we know how much to destroy in removeObject.
            removeObject();
        }
        addItemToInventory();
        //We call addItemToInventory here because this line of code is only ran *after* we have removed every item from the inventory, and it is only called once.
        
        
    }

    void removeObject()
    {
        for (int i = 1; i < playerInventory.Inventory.Length; i++)
        {
            if (playerInventory.Inventory[i].id == idToFind && amountToDestroy != 0)
            {
                playerInventory.Inventory[i].id = 0;
                //Set found inventory slot id to 0.
                UnityEngine.UI.Image imageToChange = playerInventory.images[i].GetComponent<UnityEngine.UI.Image>();
                imageToChange.sprite = null;
                //Set found inventory slot's image to null.
                amountToDestroy--;
                //Remove one from amountToDestroy.
            }
            /* For every slot in the Inventory we check for a slot that has the id we are trying to find, if it does have the id and we are still searching for said id then we
             will set that inventory slot's id to 0 and we will change it's image to null */

        }

    }

    void addItemToInventory()
    {
        int Amount = itemCrafted[0].objectAmount;

        for (int i = 0; i < Amount; Amount--)
        {
            playerScript.itemToAdd = itemCrafted[0].objectID;
            //Set the itemToAdd in the playerMovement script (playerscript) to the item's id that we are making here.
            playerScript.CheckFirstEmptySlot();
            //Run the function in playerMovement (playerScript) that adds an item to the inventory.
            inventoryUi.emptyItemAmounts();
            //call the inventoryUI script to empty the amount of item id's we have in the inventory, so we can check it again and add from 0.
            inventoryUi.checkItemAmounts();
            //Call the inventoryUI script to add the amount of item id's we have in player inventory.
        }


    }

}


[System.Serializable]
public class craftingInfo
{
    public int objectID;
    public int objectAmount;
}
