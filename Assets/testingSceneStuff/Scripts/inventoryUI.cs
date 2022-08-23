using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventoryUI : MonoBehaviour
{

    public inventory playersInventory;

    public playerMovement playerScript;

    public GameObject Player;

    public int movingItemID;

    public Canvas canvas;

    public Image moveItemImage;

    public List<int> itemAmounts = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        moveItemImage.enabled = false;
        playersInventory = Player.GetComponent<inventory>();
        playerScript = Player.GetComponent<playerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
        moveItemImage.transform.position = canvas.transform.TransformPoint(pos);
    }

    public void checkItemAmounts()
    {
        for (int i = 1; i < playersInventory.Inventory.Length; i++)
        {
            itemAmounts[playersInventory.Inventory[i].id]++;
            //Add the type of item id in the players inventory to our int list of item amounts, which is checked through ids.
        }


    }

    public void emptyItemAmounts()
    {
        for (int i = 0; i < itemAmounts.Count; i++)
        {
            itemAmounts[i] = 0;
        }
        //For every slot in our list of item Amounts set the entire thing to 0, we do this so the itemAmounts do not just add up every time we open the inventory.
    }
}
