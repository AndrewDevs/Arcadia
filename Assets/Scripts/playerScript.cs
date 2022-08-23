using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine.UI;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    public GameObject cameraObject;
    public GameObject itemToPlace;
    public GameObject objectItemDatabase;
    public GameObject empty;
    public GameObject bigInventory;
    GameObject hitObject;


    public float speed = 1f;
    public float jump = 2f;
    float cameraVerticalSpeed = 3f;
    float CameraHorizontalSpeed = 3f;
    public float itemDistance;
    public float x;
    public float y;
    public float z;

    public Rigidbody rb;

    public Text buildText;

    public bool snapMode = false;
    public bool inventoryOpen = false;

    float verticalCameraPosition;
    float horizontalCameraPosition;

    Collider hitObjectCollider;


    Color CustomGrey = new Color(0.3f, 0.3f, 0.3f, 1f);
    Color DarkGrey = new Color(0.4f, 0.4f, 0.4f, 1f);

    public int id;
    public int inSlot = 1;
    int itemLocation;

    bool canPlace;

    Vector3 fwd = new Vector3(0, 0, 0);

    RaycastHit hit;



    UnityEngine.UI.Image imageToChange;

    itemDatabase gameItemDatabase;

    inventoryScript playersInventory;

    itemScript itemHit;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //Locks cursor to camera for movement purposess
        Cursor.visible = false;
        //Sets cursor to invisible.
        gameItemDatabase = objectItemDatabase.GetComponent<itemDatabase>();
        // Gets game Database for item number refrence, inventory reasons.
        playersInventory = this.gameObject.GetComponent<inventoryScript>();
        // Gets Player's inventory script for inventory reasons, is on same gameObject.
        imageToChange = playersInventory.images[inSlot].GetComponent<UnityEngine.UI.Image>();
        // Gains acces to the first slots image so we can change the color, if we do not do this then it will be set to null and will cause an error.
        bigInventory.SetActive(inventoryOpen);
        // We set the bigInventory to false so it's not open at the start.
        inSlot = 1;


        ColorSlot();

    }

    // Update is called once per frame
    void Update()
    {
        UnityEngine.Debug.DrawRay(cameraObject.transform.position, fwd * hit.distance, Color.yellow);

        if (inventoryOpen == false)
        {
            if (Input.GetKey(KeyCode.W) == true)
            {
                transform.position += transform.forward * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S) == true)
            {
                transform.position -= transform.forward * speed * Time.deltaTime;

            }
            if (Input.GetKey(KeyCode.A) == true)
            {
                transform.position -= transform.right * speed * Time.deltaTime;

            }
            if (Input.GetKey(KeyCode.D) == true)
            {
                transform.position += transform.right * speed * Time.deltaTime;

            }
           if (Input.GetKeyDown(KeyCode.Space) == true)
            {
                transform.position += transform.up * jump;

            } 
            // we use transform rather than vector3 as vector3 is based on worldspace, whilst transform is relative to player's transform.
            // This is just the player movement code. we change the player's transform.position if a certain key is pressed (if they click d then the transform's Vector3 will move right multiplied by our set speed, etc.)
            // GetKey is used instead of GetKeyDown as GetKey checks if the key is down, GetKeyDown only checks if the key is pressed once.
        }

        //GetKeyDown is used instead of GetKey as we do not want the player floating up as long as space is pressed. We also use the jump variable instead of speed as speed is to insignificant to make a proper jump.
        // To move the player we merely change the transform.position Vector3 by making a new Vector3 which changes the desired position (Either XYZ) by adding or subtracting the Speed float.
        if (inventoryOpen == false)
        {
            verticalCameraPosition -= cameraVerticalSpeed * Input.GetAxis("Mouse Y");
            //Here we change the verticalCameraPosition float to the position of the mouse verticaly + our cameraVerticalSpeed.
            horizontalCameraPosition += CameraHorizontalSpeed * Input.GetAxis("Mouse X");
            //Here we do the same idea as above, except we do this horizontally.
            cameraObject.transform.eulerAngles = new Vector3(verticalCameraPosition, horizontalCameraPosition, 0f);
            // This merely applies the float changes to our cameras positioning.

            transform.eulerAngles = new Vector3(0f, horizontalCameraPosition, 0f);
            //Here we rotatate the transform of the player (transform.eulerAngles) horizontally, not vertically. We do not need the player moving up and down as the camera does, this also causes collisions to act weird.
        }
        ColorSlot();

        //ITEM SLOT STUFF
        if (Input.GetKeyDown(KeyCode.Alpha1) == true)
        {    
            if(itemToPlace != null)
            {
                itemToPlace.SetActive(false);
            }
            itemToPlace = playersInventory.inventory[1].itemObject;
                inSlot = 1;
                ColorSlot();


        }
        if (Input.GetKeyDown(KeyCode.Alpha2) == true)
        {
            if (itemToPlace != null)
            {
                itemToPlace.SetActive(false);
            }
            itemToPlace = playersInventory.inventory[2].itemObject;
            inSlot = 2;
                ColorSlot();

        }
        if (Input.GetKeyDown(KeyCode.Alpha3) == true)
        {
            if (itemToPlace != null)
            {
                itemToPlace.SetActive(false);
            }
            itemToPlace = playersInventory.inventory[3].itemObject;
            inSlot = 3;
                ColorSlot();

        }
        if (Input.GetKeyDown(KeyCode.Alpha4) == true)
        {
            if (itemToPlace != null)
            {
                itemToPlace.SetActive(false);
            }
            itemToPlace = playersInventory.inventory[4].itemObject;
            inSlot = 4;
                ColorSlot();

        }
        if (Input.GetKeyDown(KeyCode.Alpha5) == true)
        {
            if (itemToPlace != null)
            {
                itemToPlace.SetActive(false);
            }
            itemToPlace = playersInventory.inventory[5].itemObject;
            inSlot = 5;
            ColorSlot();

        }
        if (Input.GetKeyDown(KeyCode.Alpha6) == true)
        {
            if (itemToPlace != null)
            {
                itemToPlace.SetActive(false);
            }
            itemToPlace = playersInventory.inventory[6].itemObject;
            inSlot = 6;
            ColorSlot();

        }
        if (Input.GetKeyDown(KeyCode.Alpha7) == true)
        {
            if (itemToPlace != null)
            {
                itemToPlace.SetActive(false);
            }
            itemToPlace = playersInventory.inventory[7].itemObject;
            inSlot = 7;
            ColorSlot();

        }
        if (Input.GetKeyDown(KeyCode.Alpha8) == true)
        {
            if (itemToPlace != null)
            {
                itemToPlace.SetActive(false);
            }
            itemToPlace = playersInventory.inventory[8].itemObject;
            inSlot = 8;
            ColorSlot();

        }
        if (Input.GetKeyDown(KeyCode.Alpha9) == true)
        {
            if (itemToPlace != null)
            {
                itemToPlace.SetActive(false);
            }
            itemToPlace = playersInventory.inventory[9].itemObject;
            inSlot = 9;
            ColorSlot();

        }
        if (Input.GetKeyDown(KeyCode.Alpha0) == true)
        {
            if (itemToPlace != null)
            {
                itemToPlace.SetActive(false);
            }
            itemToPlace = playersInventory.inventory[10].itemObject;
            inSlot = 10;
            ColorSlot();

        }
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.R) == true)
        {
            if(itemToPlace != null)
            {
                itemToPlace.transform.rotation *= Quaternion.AngleAxis(45, Vector3.left);
                //idk why we multiple it we really are adding. Moves it 45 degrees left.
            }
        }
        if (Input.GetKeyDown(KeyCode.T) == true)
        {
            if (itemToPlace != null)
            {
                itemToPlace.transform.rotation *= Quaternion.AngleAxis(45, Vector3.back);
                //idk why we multiple it we really are adding. Moves it 45 degrees backwards.
            }
        }
        if (Input.GetKeyDown(KeyCode.F) == true)
        {
            snapMode = !snapMode;
            //We use this to toggle between build modes.
        }
        if (Input.GetKeyDown(KeyCode.E) == true)
        {
            inventoryOpen = !inventoryOpen;
            bigInventory.SetActive(inventoryOpen);
            if (inventoryOpen == true)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;

            }


            //Locks cursor to camera for movement purposess
            Cursor.visible = inventoryOpen;
            //This is used to open and close the inventory.
        }

        //When player clicks number key, the id int will be equal to that number. The id is how we figure out which slot to use in the player's inventory. 
        // inSlot is used to see which slot the player is on so we can set the slot to 0 after the item has been placed.



        if (snapMode == false)
        {
            buildText.text = "Building Mode: Free Build";
            if (itemToPlace != null)
            {
                itemToPlace.transform.position = cameraObject.transform.position + cameraObject.transform.forward * itemDistance;
                //This is "free mode", where the itemToPlace can be freely moved around infront of our player.
            }
        }
        else
        {
            buildText.text = "Building Mode: Snap Build";

            if (itemToPlace != null)
            {
                if (Physics.Raycast(transform.position, (cameraObject.transform.forward * itemDistance), out hit, itemDistance))
                {
                    hitObject = hit.transform.gameObject;
                    //We cannot get the object hit by raycasts position directly idk why, so we have to set it to a gameObject then get the position.
                    //  itemToPlace.transform.position = hitObject.transform.position - hit.normal + itemToPlace.transform.localScale;


                    hitObjectCollider = hitObject.GetComponent<Collider>();
                    itemToPlace.transform.position = hitObject.transform.position;

                    if (hit.normal.x != 0 && hit.normal.x < 0)
                    {
                        x = hitObject.transform.position.x - hitObject.transform.localScale.x / 2 - itemToPlace.transform.localScale.x / 2;
                    }
                    if (hit.normal.x != 0 && hit.normal.x > 0)
                    {
                        x = hitObject.transform.position.x + hitObject.transform.localScale.x / 2 + itemToPlace.transform.localScale.x / 2;

                    }
                    if (hit.normal.z != 0 && hit.normal.z < 0)
                    {
                        z = hitObject.transform.position.z - hitObject.transform.localScale.x / 2 - itemToPlace.transform.localScale.z / 2;
                    }
                    else if (hit.normal.z != 0 && hit.normal.z > 0)
                    {
                        z = hitObject.transform.position.z + hitObject.transform.localScale.z / 2 + itemToPlace.transform.localScale.z / 2;

                    }
                /*    if(hit.normal.y != 0 && hit.normal.y > 0)
                    {
                        y = hitObject.transform.position.y + hitObject.transform.localScale.y / 2 + itemToPlace.transform.localScale.y / 2;
                    }else if(hit.normal.y != 0 && hit.normal.y < 0)
                    {
                        y = hitObject.transform.position.y - hitObject.transform.localScale.y / 2 - itemToPlace.transform.localScale.y / 2;
                    } */

                    if (hit.normal.x == 0)
                    {
                        x = hitObject.transform.position.x;
                    }
                    if (hit.normal.z == 0)
                    {
                        z = hitObject.transform.position.z;
                    }
                  /*  if(hit.normal.y == 0)
                    {
                        y = hitObject.transform.position.y;
                    }*/
                    itemToPlace.transform.rotation = hitObject.transform.rotation;
                    itemToPlace.transform.position = new Vector3(x, hitObject.transform.position.y, z);
                    

                    /*Here, in snap build, we just shoot a raycast from a player and get the normal from the hit(hit.normal), we use the hit.normal to figure out which side
                     of the object the player is hitting, and put the itemToPlace accordingly.*/
                }
            }
        }
        //Here we set the item to place infront of our camera, the distance away is determined by *itemDistance,
        if (itemToPlace != null)
        {
            itemToPlace.SetActive(true);
            itemToPlace.layer = 2;
            //Set layer to one the rayCast doesn't detect so it cannot be picked up whilst it's being placed - avoiding dupliation.
        }
        
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                if (itemDistance < 15f)
                {
                    itemDistance = itemDistance + 0.5f;
                }
            }
            //Moves itemToPlace farther away if scroll up.

            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                if(itemDistance > 5f){
                    itemDistance = itemDistance - 0.5f;
                }
            }
        //Moves itemToPlace closer if scroll down.







    }


    void FixedUpdate()
    {
        itemLocation = playersInventory.inventory[inSlot].itemInSlot;
        //Here we look in the players Inventory, get the slot that they are on, and make ItemLocation equal to the number of the number of [inSlot] (i.e int in slot 3 = 2 = 3 we look for slot 2 and set itemLocation to 2).
        fwd = cameraObject.transform.TransformDirection(Vector3.forward);
        //Creates a vector3 named "fwd", who's variables are the foward direction relative to the player's transform.

        itemToPlace = playersInventory.inventory[inSlot].itemObject;
        //Here we get that int from the slot the player is on and pair it with the gameItemDatabse.





        if (inventoryOpen == false)
        {

            //Pick up item function.
            if (Input.GetMouseButtonDown(0))
            {

                if (Physics.Raycast(cameraObject.transform.position, fwd, out hit, 15))
                {
                    UnityEngine.Debug.DrawRay(cameraObject.transform.position, fwd * hit.distance, Color.yellow);

                    itemHit = hit.transform.gameObject.GetComponent<itemScript>();
                    if (itemHit != null)
                    {
                        // here we are accessing the local variable itemScript, named scriptname, by getting the component script on the hit object, raycast stuff is explained below.

                        CheckFirstEmptySlot();
                        id = itemHit.itemNumber;
                        //set id of item to the itemNumber on the item itself.
                        imageToChange.sprite = gameItemDatabase.itemImages[id];
                        // We take that local variable we made, and change the sprite of it to the id of our item that we clicked.

                        int itemLocationNumber = itemHit.itemNumber;
                        hit.transform.gameObject.SetActive(false);
                        //hit Object is deActivated;
                        hit.transform.rotation = Quaternion.Euler(0, 0, 0);
                        //Set the item we picked up's rotation to 0,0,0 because we generate objects at random rotation and it can act funky when player tries to rotate it when that original, generated, rotation.

                        id = 0;
                    }
                }
            }

            // If right click, creates a raycast from the cameras, not the players, transform. The direction of this raycast is defined by "fwd", and it shoots out 15.
            //If right click and a hit then an item will be placed at the hit.point, hit.point is a vector3, hit is not.
            // I used the cameras transform instead of the player's as the camera's transform moves up and down, whilst the player's does not to avoid collission issues.

            //Place down item function
            if (Input.GetMouseButtonDown(1))
            {
                RaycastHit hit;



                UnityEngine.UI.Image imageToChange = playersInventory.images[inSlot].GetComponent<UnityEngine.UI.Image>();
                imageToChange.sprite = null;

                // Sets image of whatever slot player is on to null.
                playersInventory.inventory[inSlot].itemInSlot = 0;
                //Change the itemInSlot number to 0 so we know it's empty.
                playersInventory.inventory[inSlot].itemObject = null;
                //Change the itemInSlot number to 0 so we know it's empty.
                itemToPlace.layer = 0;
                //Set layer back to 0 so rayCast can hit it.
                id = 0;
                //set player inventory slot to empty
                itemToPlace = null;
                // set itemToplace to null so no more items can be placed.


                //The itemToPlace object already exists and it's position is set in the update code, all this does is remove it from the players inventory, and thus stopping the player's ability to move it.
            }

        }


        int CheckFirstEmptySlot()
        {
            int onSlot = -0;
            for (int i = 1; i < playersInventory.inventory.Length; i++)
            // while i is less then 0 it will keep checkng the player inventory. this is to make sure it gets to all slots. i.e i starts at 0 then goes up each slot until it has went through the full length of the inventory.
            {
                if (playersInventory.inventory[i].itemInSlot == 0)
                // checks whatever slot i is on to see if the itemInSlot on our array is equal to 0.
                {
                    onSlot = i;
                    // if this is true then we will set the id equal to i, which is the number of the slot we are on.
                    playersInventory.inventory[onSlot].itemObject = hit.transform.gameObject;
                    // We have get the GameObject that we have clicked on and set it to the itemObject type in our player's inventory.
                    imageToChange = playersInventory.images[onSlot].GetComponent<UnityEngine.UI.Image>();
                    //We create a local image variable, imageToChange, and set that equal to the image of the slot the player is on.
                    playersInventory.inventory[i].itemInSlot = itemHit.itemNumber;
                    //Here we set the itemInSlot number equal to the item which we picked up's itemNumber, so we know what type of item is in the slot and so we know it is occupied.
                    hit.transform.parent = null;
                    //We remove the picked up objects parent so that it will not deActive with the chunk.
                    break;
                    //break is used to stop the for loop once an empty slot has been found.
                }
            }
            return onSlot;
        }

    }
    int ColorSlot()
    {
        for (int i = 1; i < playersInventory.inventory.Length; i++)
        // while i is less then 0 it will keep checkng the player inventory. this is to make sure it gets to all slots. i.e i starts at 0 then goes up each slot until it has went through the full length of the inventory.
        {
            GameObject SlotimageToChange = playersInventory.images[i];
            //Finds whatever slot gameObject we are currently on, i.e if the loop is on slot 5 then SlotimageToChange will be set to the 5th gameObject in the images List.

            if (i != inSlot)
            // checks whatever slot i is on to see if it is equal to 0, which is the number we assign to empty slots.
            {
                SlotimageToChange.transform.parent.GetComponent<UnityEngine.UI.Image>().color = DarkGrey;
            }
            //If the slot the loop is currently on, located by i, is not equal to the slot the player is on, located by inSlot, then the color will be changed.
             if (i == inSlot)
             {
                SlotimageToChange.transform.parent.GetComponent<UnityEngine.UI.Image>().color = CustomGrey;
                // if we are on the slot that the player is slected on then the color will change to whatver it is set too.
             }

             if(i == playersInventory.inventory.Length)
             {
                break;
                // if we have went through all slots then loop will breka.
             }
            
                
                
        }
        return inSlot;
    }


}
