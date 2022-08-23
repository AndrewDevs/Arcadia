using System.Collections;
using System.Collections.Generic;
using System;
using Mirror;
//Put this so we can refrence stuff like NetworkBehavior.
using UnityEngine;
using static worldGenerator;
using UnityEngine.UI;
using static spawnedEnviornmentInfo;
using static playerInfo;


public class playerMovement : NetworkBehaviour
    //use NetworkBehavior instead of MonoBehavior so we can do stuff with the network.
{

    public int speed;
    public int jump;
    public int inSlot;
    public int itemToAdd;
    int itemLocation;
    int id;

    float verticalCameraPosition;
    float horizontalCameraPosition;
    public float horizontalCameraSpeed;
    public float cameraVerticalSpeed;
    public float itemDistance;
    public float gravity;
    float x;
    float y;
    float z;

    public GameObject cameraObject;
    public GameObject bigInventory;
    public GameObject itemToPlace;
    public GameObject canvasToSpawn;
    public GameObject cullingObject;
    public GameObject hitObject;
    public GameObject Null;
    public GameObject player;
    public GameObject pauseMenu;
    public GameObject worldGeneratorObj;
    public GameObject fThreeMenu;
    public GameObject seasonObject;
    GameObject chatBox;

    public NetworkIdentity objectNetId;

    CharacterController characterController;

    public bool snapMode = false;
    public bool inventoryOpen = false;
    public bool pause = false;
    public bool f3 = false;
    bool onGround = false;
    public bool fly = false;

    public inventory playersInventory;

    public inventoryUI inventoryUserI;

    public Database gameItemDatabase;

    public worldGenerator worldGenScript;

    chatScript chatscript;

    public Text buildText;

    public TextMesh floatingUsername;

    public String username;

    Vector3 fwd = new Vector3(0, 0, 0);
    Vector3 position = new Vector3(0, 0, 0);
    Vector3 itemToPlacePos = new Vector3(0, 0, 0);
    Vector3 itemToPlaceRot = new Vector3(0, 0, 0);
    Vector3 velocity;

    Collider hitObjectCollider;

    RaycastHit hit;

    UnityEngine.UI.Image imageToChange;

    item itemHit;

    Color CustomGrey = new Color(0.3f, 0.3f, 0.3f, 1f);
    Color DarkGrey = new Color(0.4f, 0.4f, 0.4f, 1f);

    public List<GameObject> clouds = new List<GameObject>();
    public List<GameObject> spawnedClouds = new List<GameObject>();


    // Start is called before the first frame update

    public override void OnStartLocalPlayer()
    {
        characterController = GetComponent<CharacterController>();
        //Getting player's characterController component for usage with movement.
        Cursor.lockState = CursorLockMode.Locked;
        //Locks cursor to camera for movement purposess
        Cursor.visible = false;
        //Sets cursor to invisible.
        GameObject playerCanvas = Instantiate(canvasToSpawn);
        //We must spawn the canvas through the local player so the script can refrence it due to the Player being a prefab, and we do it this way so only the local player can view it. Also set it to playerCanvas so we an access it.
        playerLocator playerCanvasFind = playerCanvas.GetComponent<playerLocator>();
        //Get the playerLocator script from the playerCanvas so we can set it to this player Object.
        playerCanvasFind.player = player;
        //Set player on player's canvas to this player, we do this so the slots can know to access this player's inventory, rather than other player's.
        pauseMenu = GameObject.Find("pauseMenu");
        //Find the pause menu, which is apart of our player's Canvas (canvasToSpawn).
         pauseMenu.SetActive(pause);
        //this makes the pauseMenu unActive at start.
        GameObject Database = GameObject.Find("Database(Clone)");
        gameItemDatabase = Database.GetComponent<Database>();
        //Spawn the gameItemDatabase.
        fThreeMenu = GameObject.Find("F3MENU");
        //Find the fThreeMenu so we can access it for enabiling/disabiling it in update.
        fThreeMenu.SetActive(false);
        //Set the fThreeMenu to false so it is not open when the player first launches the game.
        worldGeneratorObj = GameObject.Find("worldGenerator(Clone)");
        //Find world generator object.
        worldGenScript = worldGeneratorObj.GetComponent<worldGenerator>();
        worldGenScript.addUsernameToServer(player, username);
        chatBox = GameObject.Find("chatBox");
        chatscript = chatBox.GetComponent<chatScript>();
        Instantiate(seasonObject);
        //This season object will be called by our servers timeScript, and will be used to change the player's materials. We have to have a local object do it beacause the materials are local, and it's too much info for an rpc to use due to the amount of colors and materials necessary to set/change.
        CmdsetUsername(username, this, worldGenScript);
        cameraObject.SetActive(true);
        //Set camera to true, we do this here to avoid weird camera bugs that happened before this line was added.

        #region Finding Stuff
        buildText = GameObject.Find("buildingText").GetComponent<Text>();
        //Locate the buildingText.
        bigInventory = GameObject.Find("bigInventory");
        //Find the bigInventory.
        gameItemDatabase = GameObject.Find("Database(Clone)").GetComponent<Database>();
        //Find item Database
        inventoryUserI = bigInventory.GetComponent<inventoryUI>();
        inventoryUserI.Player = player;


        playersInventory.images[0] = null;
        playersInventory.images[1] = GameObject.Find("item1");
        playersInventory.images[2] = GameObject.Find("item2");
        playersInventory.images[3] = GameObject.Find("item3");
        playersInventory.images[4] = GameObject.Find("item4");
        playersInventory.images[5] = GameObject.Find("item5");
        playersInventory.images[6] = GameObject.Find("item6");
        playersInventory.images[7] = GameObject.Find("item7");
        playersInventory.images[8] = GameObject.Find("item8");
        playersInventory.images[9] = GameObject.Find("item9");
        playersInventory.images[10] = GameObject.Find("item10");
        playersInventory.images[11] = GameObject.Find("item11");
        playersInventory.images[12] = GameObject.Find("item12");
        playersInventory.images[13] = GameObject.Find("item13");
        playersInventory.images[14] = GameObject.Find("item14");
        playersInventory.images[15] = GameObject.Find("item15");
        playersInventory.images[16] = GameObject.Find("item16");
        playersInventory.images[17] = GameObject.Find("item17");
        playersInventory.images[18] = GameObject.Find("item18");
        playersInventory.images[19] = GameObject.Find("item19");
        playersInventory.images[20] = GameObject.Find("item20");
        playersInventory.images[21] = GameObject.Find("item21");
        playersInventory.images[22] = GameObject.Find("item22");
        playersInventory.images[23] = GameObject.Find("item23");
        playersInventory.images[24] = GameObject.Find("item24");
        playersInventory.images[25] = GameObject.Find("item25");
        playersInventory.images[26] = GameObject.Find("item26");
        playersInventory.images[27] = GameObject.Find("item27");
        playersInventory.images[28] = GameObject.Find("item28");
        playersInventory.images[29] = GameObject.Find("item29");
        playersInventory.images[30] = GameObject.Find("item30");

        bigInventory.SetActive(inventoryOpen);
        //Here we are closing the bigInventory, due to it being set to active at the start so the script can find it (inventoryOpen is set to false at start).

        #endregion

    }

    void OnDrawGizmosSelected()
    {
        Camera camera = cameraObject.GetComponent<Camera>();
        Vector3 p = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(p, 200F);
    }

    // Update is called once per frame
    void Update()
    {

        if (itemToPlace != null)
        {
            itemMovement();
        }
        
        if (isLocalPlayer == true)
        {
            if (chatscript.typing == false)
            {
                if (inventoryOpen == false)
                {
                    if (pause == false)
                    {
                        float xAxis = Input.GetAxis("Horizontal");
                        float zAxis = Input.GetAxis("Vertical");

                        Vector3 direction = transform.right * xAxis + transform.forward * zAxis;

                        characterController.Move(direction * speed * Time.deltaTime);

                        if (characterController.isGrounded == false)
                        {
                            velocity.y += gravity * Time.deltaTime;
                        }
                        else
                        {
                            velocity.y = 0;
                        }



                        if (Physics.Raycast(cameraObject.transform.position, -Vector3.up, out hit, 3))
                        {
                            if (Input.GetKeyDown(KeyCode.Space) == true)
                            {
                                UnityEngine.Debug.DrawRay(cameraObject.transform.position, -Vector3.up * hit.distance, Color.yellow);

                                velocity.y = Mathf.Sqrt(jump * -2f * gravity);
                            }
                        }

                        characterController.Move(velocity * Time.deltaTime);

                        //Movement code.

                        verticalCameraPosition -= cameraVerticalSpeed * Input.GetAxis("Mouse Y");
                        //Here we change the verticalCameraPosition float to the position of the mouse verticaly + our cameraVerticalSpeed.
                        horizontalCameraPosition += horizontalCameraSpeed * Input.GetAxis("Mouse X");
                        //Here we do the same idea as above, except we do this horizontally.
                        verticalCameraPosition = Mathf.Clamp(verticalCameraPosition, -90f, 90f);
                        //Clamp rotation at 90 degrees so player can not rotate 360 on the y axis.
                        cameraObject.transform.eulerAngles = new Vector3(verticalCameraPosition, horizontalCameraPosition, 0f);
                        // This merely applies the float changes to our cameras positioning.
                        transform.eulerAngles = new Vector3(0f, horizontalCameraPosition, 0f);
                        //Here we rotatate the transform of the player (transform.eulerAngles) horizontally, not vertically. We do not need the player moving up and down as the camera does, this also causes collisions to act weird.

                        #region Hotbar

                        //We do not let the building system operate when we are adding items to inventory because whenever we do so the inventory messes up (inventory adds item to slot then building removes it, causes confusion).
                        if (Input.GetKeyDown(KeyCode.Alpha1) == true)
                        {
                            if (inSlot != 1)
                            {
                                inSlot = 1;
                                itemToPlaceRot = new Vector3(0, 0, 0);
                                //Set the itemToPlacePos to 0 when we click on a new slot, so the rotation of item in slot 1 does not carry over to the item in slot 2.
                                hotBarSpawnItem();
                                //Call function that will spawn the item in this inventory/hotbar slot.
                                ColorSlot();
                            }

                        }
                        if (Input.GetKeyDown(KeyCode.Alpha2) == true)
                        {
                            if (inSlot != 2)
                            {
                                inSlot = 2;
                                itemToPlaceRot = new Vector3(0, 0, 0);
                                //Set the itemToPlacePos to 0 when we click on a new slot, so the rotation of item in slot 1 does not carry over to the item in slot 2.
                                hotBarSpawnItem();
                                //Call function that will spawn the item in this inventory/hotbar slot.
                                ColorSlot();
                            }

                        }
                        if (Input.GetKeyDown(KeyCode.Alpha3) == true)
                        {
                            if (inSlot != 3)
                            {
                                inSlot = 3;
                                itemToPlaceRot = new Vector3(0, 0, 0);
                                //Set the itemToPlacePos to 0 when we click on a new slot, so the rotation of item in slot 1 does not carry over to the item in slot 2.
                                hotBarSpawnItem();
                                //Call function that will spawn the item in this inventory/hotbar slot.
                                ColorSlot();
                            }

                        }
                        if (Input.GetKeyDown(KeyCode.Alpha4) == true)
                        {
                            if (inSlot != 4)
                            {
                                inSlot = 4;
                                itemToPlaceRot = new Vector3(0, 0, 0);
                                //Set the itemToPlacePos to 0 when we click on a new slot, so the rotation of item in slot 1 does not carry over to the item in slot 2.
                                hotBarSpawnItem();
                                //Call function that will spawn the item in this inventory/hotbar slot.
                                ColorSlot();
                            }

                        }
                        if (Input.GetKeyDown(KeyCode.Alpha5) == true)
                        {
                            if (inSlot != 5)
                            {
                                inSlot = 5;
                                itemToPlaceRot = new Vector3(0, 0, 0);
                                //Set the itemToPlacePos to 0 when we click on a new slot, so the rotation of item in slot 1 does not carry over to the item in slot 2.
                                hotBarSpawnItem();
                                //Call function that will spawn the item in this inventory/hotbar slot.
                                ColorSlot();
                            }

                        }
                        if (Input.GetKeyDown(KeyCode.Alpha6) == true)
                        {
                            if (inSlot != 6)
                            {
                                inSlot = 6;
                                itemToPlaceRot = new Vector3(0, 0, 0);
                                //Set the itemToPlacePos to 0 when we click on a new slot, so the rotation of item in slot 1 does not carry over to the item in slot 2.
                                hotBarSpawnItem();
                                //Call function that will spawn the item in this inventory/hotbar slot.
                                ColorSlot();
                            }

                        }
                        if (Input.GetKeyDown(KeyCode.Alpha7) == true)
                        {
                            if (inSlot != 7)
                            {
                                inSlot = 7;
                                itemToPlaceRot = new Vector3(0, 0, 0);
                                //Set the itemToPlacePos to 0 when we click on a new slot, so the rotation of item in slot 1 does not carry over to the item in slot 2.
                                hotBarSpawnItem();
                                //Call function that will spawn the item in this inventory/hotbar slot.
                                ColorSlot();
                            }

                        }
                        if (Input.GetKeyDown(KeyCode.Alpha8) == true)
                        {
                            if (inSlot != 8)
                            {
                                inSlot = 8;
                                itemToPlaceRot = new Vector3(0, 0, 0);
                                //Set the itemToPlacePos to 0 when we click on a new slot, so the rotation of item in slot 1 does not carry over to the item in slot 2.
                                hotBarSpawnItem();
                                //Call function that will spawn the item in this inventory/hotbar slot.
                                ColorSlot();
                            }

                        }
                        if (Input.GetKeyDown(KeyCode.Alpha9) == true)
                        {
                            if (inSlot != 9)
                            {
                                inSlot = 9;
                                itemToPlaceRot = new Vector3(0, 0, 0);
                                //Set the itemToPlacePos to 0 when we click on a new slot, so the rotation of item in slot 1 does not carry over to the item in slot 2.
                                hotBarSpawnItem();
                                //Call function that will spawn the item in this inventory/hotbar slot.
                                ColorSlot();
                            }

                        }
                        if (Input.GetKeyDown(KeyCode.Alpha0) == true)
                        {
                            if (inSlot != 10)
                            {
                                inSlot = 10;
                                itemToPlaceRot = new Vector3(0, 0, 0);
                                //Set the itemToPlacePos to 0 when we click on a new slot, so the rotation of item in slot 1 does not carry over to the item in slot 2.
                                hotBarSpawnItem();
                                //Call function that will spawn the item in this inventory/hotbar slot.
                                ColorSlot();
                            }

                        }

                        #endregion
                    }
                }
                if (Input.GetKeyDown(KeyCode.F) == true)
                {
                    snapMode = !snapMode;
                    //We use this to toggle between build modes.
                }
                if (Input.GetKeyDown(KeyCode.R) == true)
                {
                    itemToPlaceRot = new Vector3(itemToPlaceRot.x, itemToPlaceRot.y, itemToPlaceRot.z + 45f);
                    //If player clicks R then move the rotation of our item to be placed by 45 degrees on Z axis.
                }
                if (Input.GetKeyDown(KeyCode.T) == true)
                {
                    itemToPlaceRot = new Vector3(itemToPlaceRot.x + 45f, itemToPlaceRot.y, itemToPlaceRot.z);
                    //If player clicks T then move the rotation of our item to be placed by 45 degrees on X axis.
                }
                if (Input.GetKeyDown(KeyCode.Escape) == true)
                {
                    pause = !pause;
                    //Set the pause bool to whatever it is not, so if the player clicks Escape multiple times it will open and close smoothly.
                    pauseMenu.SetActive(pause);
                    //We allow the bool to dictate whether the pauseMenu is open or not.

                }

                if (Input.GetKeyDown(KeyCode.F3) == true)
                {
                    f3 = !f3;
                    fThreeMenu.SetActive(f3);
                }
                if (Input.GetKeyDown(KeyCode.E) == true)
                {
                    inventoryOpen = !inventoryOpen;
                    bigInventory.SetActive(inventoryOpen);

                    if (inventoryOpen == true)
                    {
                        if (inventoryUserI.playersInventory != null)
                        {
                            inventoryUserI.checkItemAmounts();
                        }
                        else
                        {
                            inventoryUserI.playersInventory = playersInventory;
                        }
                    }
                    else
                    {
                        inventoryUserI.emptyItemAmounts();
                    }


                    //This is used to open and close the inventory.
                }
                if (pause == true || inventoryOpen == true)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    //Unlock the cursor if pause menu is open.

                }
                else if (pause == false || inventoryOpen == false)
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    //Lock the cursor.
                }
                if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                {
                    if (itemDistance < 15f)
                    {
                        itemDistance = itemDistance + 0.5f;
                    }
                }
                //Moves itemDistance farther away if scroll up.

                if (Input.GetAxis("Mouse ScrollWheel") < 0f)
                {
                    if (itemDistance > 5f)
                    {
                        itemDistance = itemDistance - 0.5f;
                    }
                }
                //Moves itemDistance closer if scroll down.
            }
            if (isLocalPlayer == true)
            {
                itemLocation = playersInventory.Inventory[inSlot].id;
                //Here we look in the players Inventory, get the slot that they are on, and make ItemLocation equal to the number of the number of [inSlot] (i.e int in slot 3 = 2 = 3 we look for slot 2 and set itemLocation to 2).
                fwd = cameraObject.transform.TransformDirection(Vector3.forward);
                //Creates a vector3 named "fwd", who's variables are the foward direction relative to the player's transform.

                if (inventoryOpen == false)
                {
                    if (pause == false)
                    {
                        //Pick up item function.
                        if (Input.GetMouseButtonDown(0))
                        {
                            pause = true; //We add pauses here so we do not run this twice at the same time.


                            if (Physics.Raycast(cameraObject.transform.position, fwd, out hit, 15))
                            {
                                UnityEngine.Debug.DrawRay(cameraObject.transform.position, fwd * hit.distance, Color.yellow);

                                itemHit = hit.transform.gameObject.GetComponent<item>();
                                if (itemHit != null)
                                {
                                    itemToAdd = itemHit.itemNumber;
                                    //Set the item hit's id to the item to add to the inventory, we then call the function which adds to inventory below this.;
                                    CheckFirstEmptySlot();

                                    playersInventory.Inventory[inSlot].id = itemHit.itemNumber;
                                    //Set the id in our inventory Slot to the picked up Object's id (itemNumber).

                                    int itemLocationNumber = itemHit.itemNumber;
                                    hitObject = hit.transform.gameObject;


                                    float x = hitObject.transform.position.x;
                                    float z = hitObject.transform.position.z;
                                    Vector3 pos = hitObject.transform.position;

                                    Quaternion empty = new Quaternion();
                                    CmdfindObjectsChunk(x, hitObject.transform.position.y, z, worldGeneratorObj, false, pos, empty,itemToAdd);                                                                                                         

                                    hit.transform.rotation = Quaternion.Euler(0, 0, 0);
                                    //Set the item we picked up's rotation to 0,0,0 because we generate objects at random rotation and it can act funky when player tries to rotate it when that original, generated, rotation.

                                    id = 0;
                                }
                            }
                            pause = false; //Reason for pause being here explained above when we set pause to true.
                        }
                    }

                    // If right click, creates a raycast from the cameras, not the players, transform. The direction of this raycast is defined by "fwd", and it shoots out 15.
                    //If right click and a hit then an item will be placed at the hit.point, hit.point is a vector3, hit is not.
                    // I used the cameras transform instead of the player's as the camera's transform moves up and down, whilst the player's does not to avoid collission issues.

                    //Place down item functions
                    if (Input.GetMouseButtonDown(1))
                    {
                        UnityEngine.UI.Image imageToChange = playersInventory.images[inSlot].GetComponent<UnityEngine.UI.Image>();
                        imageToChange.sprite = null;
                        // Sets image of whatever slot player is on to null.
                        playersInventory.Inventory[inSlot].id = 0;
                        //Change the itemInSlot number to 0 so we know it's empty.
                        playersInventory.Inventory[inSlot].itemObject = null;
                        //Change the itemInSlot number to 0 so we know it's empty.
                        itemToPlace.layer = 0;
                        //Set layer back to 0 so rayCast can hit it.
                        id = 0;
                        //set player inventory slot to empty
                        item itemScript = itemToPlace.GetComponent<item>();
                        Vector3 position = new Vector3();

                        position.x = (float)Math.Round((double)itemToPlace.transform.position.x, 1);
                        position.y = (float)Math.Round((double)itemToPlace.transform.position.y, 1);
                        position.z = (float)Math.Round((double)itemToPlace.transform.position.z, 1);

                        itemToPlace.transform.position = position;

                        CmdplaceItem(itemToPlace, position);
                        CmdfindObjectsChunk(position.x, position.y, position.z, worldGeneratorObj, true, position, itemToPlace.transform.rotation, itemScript.itemNumber);
                        itemToPlace = null;
                        //The itemToPlace object already exists and it's position is set in the update code, all this does is remove it from the players inventory, and thus stopping the player's ability to move it.
                    }

                }

            }             
        }

        if(spawnedClouds.Count < 50)
        {
            int chance = UnityEngine.Random.Range(1, 4);
            GameObject newCloud = Instantiate(clouds[chance], new Vector3(UnityEngine.Random.Range(player.transform.position.x - 800, player.transform.position.x + 800), 250, UnityEngine.Random.Range(player.transform.position.z - 800, player.transform.position.z + 800)), Quaternion.identity);
            spawnedClouds.Add(newCloud);
        }
        foreach (GameObject cloudObject in spawnedClouds)
        {
            float distance = Vector3.Distance(player.transform.position, cloudObject.transform.position);
            if(distance > 1000)
            {
                spawnedClouds.Remove(cloudObject);
                Destroy(cloudObject);
                break;
            }
        }

    }

    [Command]
    public void CmdfindObjectsChunk(float x, float y, float z, GameObject worldGenObject, bool placing, Vector3 position, Quaternion rotation, int id) //Pass the floats and networked object instead of the actual hitObject (envObject) because the hitObject is not networked.
    {
        worldGenScript = worldGenObject.GetComponent<worldGenerator>();
        //We get the worldGenScript from our passed worldGenObject as we need to access the spawnedChunks list to locate an item with a chunk.
        Vector3 oldPos = new Vector3();
        oldPos = position;

        position.x = (float)Math.Round((double)position.x, 1);
        position.y = (float)Math.Round((double)position.y, 1);
        position.z = (float)Math.Round((double)position.z, 1);
        //Here we are rounding the position, as the item positions in our spawnedChunks enviornmentInformation list are also rounded, so we need them to match (we round bc networked positions are off by like 0.00001).


        float X = x / 15;
        X = Mathf.Round(X);
        X = X * 15;

        float Z = z / 15;
        Z = Mathf.Round(Z);
        Z = Z * 15;
        //This math takes our item we are picking up or placing down's position and making it equal to a chunkPosition so we can find the chunk it is on easier.

        if (placing == false) //If we are not placing down an object, and are this picking one up.
        {

            for (int i = 0; i < worldGenScript.spawnedChunks.Count; i++) //check through ever spawnedChunk that exists.
            {
                if (worldGenScript.spawnedChunks[i].chunkObject.transform.position.x == X && worldGenScript.spawnedChunks[i].chunkObject.transform.position.z == Z) //If the instance's x and z match our X and Z we derived above, then we have found the item's chunk.
                {

                            spawnedEnviornmentInfo objectToPickUp = new spawnedEnviornmentInfo();

                            objectToPickUp.rX = rotation.x;
                            objectToPickUp.rY = rotation.y;
                            objectToPickUp.rZ = rotation.z;

                            objectToPickUp.x = x;
                            objectToPickUp.y = y;
                            objectToPickUp.z = z;
                            objectToPickUp.id = id;

                            worldGenScript.spawnedChunks[i].itemsToRemove.Add(objectToPickUp);
                            //Compile our clicked item's info into a class we can add to the itemsToRemove list on the chunk, and then add it.

                            
                            worldGenScript.spawnedChunks[i].edited = true;

                            Vector3 chunkPosition = new Vector3(); //Create a new chunkPosition to pass to our CmdSave.

                            chunkPosition.x = worldGenScript.spawnedChunks[i].chunkObject.transform.position.x;
                            chunkPosition.y = worldGenScript.spawnedChunks[i].chunkObject.transform.position.y;
                            chunkPosition.z = worldGenScript.spawnedChunks[i].chunkObject.transform.position.z;
                            //Compile the data to our chunkPosition.

                            worldGenScript.save(worldGenScript.spawnedChunks[i].chunkType, chunkPosition, worldGenScript.spawnedChunks[i]);
                            /*We use a command in order to do the chunk serialization as we only want the server to have access/control over it (makes life easier). This is located in the worldGenScript as
                              the worldGenScript is the main script which controls the natural world (spawning, culling, loading, and now saving)*/
                            RpcremoveItemFromChunk(worldGenScript.spawnedChunks[i].chunkObject, oldPos, id);
                        
                    
                }
            }
        }
        else
        {
            //Here we will take the itemToPlace's X and Z position, and do some math to make the X and Z equal to that of the chunk the item is placed upon, we will use this to locate said chunk.

            for (int k = 0; k < worldGenScript.spawnedChunks.Count; k++) //For each instance (chunk) in spawned chunks
            {

                if (worldGenScript.spawnedChunks[k].chunkObject.transform.position.x == X && worldGenScript.spawnedChunks[k].chunkObject.transform.position.z == Z) //If the instance's x and z match our X and Z
                {
                    spawnedEnviornmentInfo spawnedEnv = new spawnedEnviornmentInfo();
                    spawnedEnv.x = x;
                    spawnedEnv.y = y;
                    spawnedEnv.z = z;
                    spawnedEnv.rX = rotation.eulerAngles.x;
                    spawnedEnv.rY = rotation.eulerAngles.y;
                    spawnedEnv.rZ = rotation.eulerAngles.z;

                    spawnedEnv.id = id;
                    
                    spawnedEnv.itemName = name;
                    //then we will make a holder class for our item we have placed upon the chunk ^

                    worldGenScript.spawnedChunks[k].itemsToAdd.Add(spawnedEnv);
                    //And we will add that holder class to that chunk's enviornmentInformation on the server, so it may be serialized.

                    Vector3 chunkPosition = new Vector3(); //Create a new chunkPosition to pass to our Save.

                    chunkPosition.x = worldGenScript.spawnedChunks[k].chunkObject.transform.position.x;
                    chunkPosition.y = worldGenScript.spawnedChunks[k].chunkObject.transform.position.y;
                    chunkPosition.z = worldGenScript.spawnedChunks[k].chunkObject.transform.position.z;
                    //Compile the data to our chunkPosition.

                    worldGenScript.save(worldGenScript.spawnedChunks[k].chunkType, chunkPosition, worldGenScript.spawnedChunks[k]);

                    RpcaddItemToChunk(oldPos, id, worldGenScript.spawnedChunks[k].chunkObject); 
                    //This function merely adds our item's data to the chunk locally for culling purposes.

                }
            }
        }
    }
    [ClientRpc]
    public void RpcaddItemToChunk(Vector3 position, int id, GameObject chunk) //This will add our item to the chunk locally for culling purposes.
    {
        GameObject item = GameObject.Find(id + " " + position);
        //Find the item we have placed with our given paramaters.

        chunkScript script = chunk.GetComponent<chunkScript>();
        //Grab the chunk's chunkScript that we have placed our item on so we can add it to its enviornmentObject list for culling reasons.

        script.enviornmentObjects.Add(item);
        //Finally add our item to the chunk's enviornmentObjects list.
    }

    [ClientRpc]
    public void RpcremoveItemFromChunk(GameObject chunk, Vector3 position, int id)
    {
        chunkScript script = chunk.GetComponent<chunkScript>();
        UnityEngine.Debug.Log(chunk + " " + id + " " + position);
        GameObject obj = GameObject.Find(id + " " + position);
        script.enviornmentObjects.Remove(obj);
        Destroy(obj);


        //CmddestroyItem(position, id);
    }



    void hotBarSpawnItem()
    {
        if (itemToPlace != null)
        {
            CmddestroyNetworkedItem(itemToPlace);
        }
        /* ^^^^^^ If there is already a object in itemToPlace, which has been assigned due to it being the item in the last slot we are on, destroy it 
         so we can replace it with the item in this slot, which will be instantiated. */
        if (playersInventory.Inventory[inSlot].id != 0)
        {
                CmdspawnObject(playersInventory.Inventory[inSlot].id, itemToPlacePos, Quaternion.identity);
                //Call our instantiating command so that we may spawn the object type in the player's inventory slot.          
        }
    }

    void itemMovement()
    {
        if (snapMode == false)
        {
            buildText.text = "Building Mode: Free Build";
            if (itemToPlace != null)
            {
                 itemToPlacePos = cameraObject.transform.position + cameraObject.transform.forward * itemDistance;
                //Set position for gameObject to be placed.
                NetworkIdentity netID = itemToPlace.GetComponent<NetworkIdentity>();
                if (itemToPlace != null)
                { 
                    CmditemMovement(itemToPlace, itemToPlacePos, itemToPlaceRot);
                    //Call on the server to set the position for our itemToPlace.
                }          
                
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

                    if (hit.normal.y < 0.5f && hit.normal.y > -0.5f)
                    {
                        if (hit.normal.x != 0 && hit.normal.x < -0.5f)
                        {
                            x = hitObject.transform.position.x - hitObject.transform.localScale.x / 2 - itemToPlace.transform.localScale.x / 2;
                        }
                        if (hit.normal.x != 0 && hit.normal.x > 0.5f)
                        {
                            x = hitObject.transform.position.x + hitObject.transform.localScale.x / 2 + itemToPlace.transform.localScale.x / 2;

                        }
                        if (hit.normal.z != 0 && hit.normal.z < -0.5f)
                        {
                            z = hitObject.transform.position.z - hitObject.transform.localScale.x / 2 - itemToPlace.transform.localScale.z / 2;
                        }
                        else if (hit.normal.z != 0 && hit.normal.z > 0.5f)
                        {
                            z = hitObject.transform.position.z + hitObject.transform.localScale.z / 2 + itemToPlace.transform.localScale.z / 2;

                        }
                    }
                        if(hit.normal.y != 0 && hit.normal.y > 0.5f)
                        {
                            y = hitObject.transform.position.y + hitObject.transform.localScale.y / 2 + itemToPlace.transform.localScale.y / 2;
                        }
                        else if(hit.normal.y != 0 && hit.normal.y < -0.5f)
                        {
                            y = hitObject.transform.position.y - hitObject.transform.localScale.y / 2 - itemToPlace.transform.localScale.y / 2;
                        } 

                    if (hit.normal.x < 0.5f && hit.normal.x > -0.5f)
                    {
                        x = hitObject.transform.position.x;
                    }
                    if (hit.normal.z < 0.5f && hit.normal.z > -0.5f)
                    {
                        z = hitObject.transform.position.z;
                    }
                    if (hit.normal.y < 0.5f && hit.normal.y > -0.5f)
                    {
                        y = hitObject.transform.position.y;
                     }
                    itemToPlacePos = new Vector3(x, y, z);

                    itemToPlaceRot = new Vector3(hitObject.transform.rotation.eulerAngles.x, hitObject.transform.rotation.eulerAngles.y, hitObject.transform.rotation.eulerAngles.z);

                    itemToPlace.transform.position = itemToPlacePos;

                    itemToPlace.transform.eulerAngles = itemToPlaceRot;


                    if (itemToPlace != null)
                    {

                         CmditemMovement(itemToPlace, itemToPlacePos, itemToPlaceRot);
                        
                    }
                    //Call on the server to set the position for our itemToPlace.

                    /*Here, in snap build, we just shoot a raycast from a player and get the normal from the hit(hit.normal), we use the hit.normal to figure out which side
                     of the object the player is hitting, and put the itemToPlace accordingly.*/
                }
            }

        }
    }
 
 
        public int CheckFirstEmptySlot()
        {
            int lastSlot = inSlot;
            inSlot = 0;
            int onSlot = -0;
            for (int i = 1; i < playersInventory.Inventory.Length; i++)
            // while i is less then 0 it will keep checkng the player inventory. this is to make sure it gets to all slots. i.e i starts at 0 then goes up each slot until it has went through the full length of the inventory.
            {
                if (playersInventory.Inventory[i].id == 0)
                // checks whatever slot i is on to see if the itemInSlot on our array is equal to 0.
                {
                    onSlot = i;
                    // if this is true then we will set the id equal to i, which is the number of the slot we are on.
                    imageToChange = playersInventory.images[onSlot].GetComponent<UnityEngine.UI.Image>();
                    //We create a local image variable, imageToChange, and set that equal to the image of the slot the player is on.
                    imageToChange.sprite = gameItemDatabase.itemImages[itemToAdd];
                    // We take that local variable we made, and change the sprite of it to the id of our item that we are adding.
                    playersInventory.Inventory[i].id = itemToAdd;
                    //Here we set the itemInSlot number equal to the item which we picked up's itemNumber, so we know what type of item is in the slot and so we know it is occupied.
                    break;
                   //break is used to stop the for loop once an empty slot has been found.
                   inSlot = lastSlot;
                }
            }
            return onSlot;
        }




    #region commands/rpc's etc. for placing down and picking up objects (building)
    [Command]
    public void CmdspawnObject(int itemId, Vector3 Pos, Quaternion rotation)
    {
        GameObject spawnedObject = Instantiate(gameItemDatabase.items[itemId], Pos, rotation);
        //Referce the databse and get the item type in said database using the id, here we as well instantiate found item type in database at the given position and rotation.
        NetworkServer.Spawn(spawnedObject);
        //Here we are spawning the type of object using the info in the paramaters and spawning it upon the network.
        spawnedObject.layer = 2;
        //Add the ignoreRayCast layer to our spawned object so the rayCast does not hit it whilst we are trying to place it.
        NetworkIdentity networkID = spawnedObject.GetComponent<NetworkIdentity>();
        //Get netId from our spawned object so we can pass it through
        TagetSetItem(connectionToClient, networkID);
    }
    [TargetRpc]
    public void TagetSetItem(NetworkConnection client, NetworkIdentity netID)
    {
        if (NetworkIdentity.spawned[netID.netId].gameObject != null){
            itemToPlace = NetworkIdentity.spawned[netID.netId].gameObject;
            //Here we are using a TargetRpc in order to set the itemToPlace to the object CmdspawnObject has instantiated, so the player can build with it.
            itemToPlace.layer = 2;
            //Add the ignoreRayCast layer to our spanwed object so the rayCast does not hit it whilst we are trying to place it.
        }
        
    }

    [Command]
    public void CmdplaceItem(GameObject item, Vector3 position)
    {
        item itemScript = item.GetComponent<item>();
        //Getting the itemToPlace's itemScript so we can access it's id etc.

        RpcplaceItem(itemScript.itemNumber, position, item.transform.rotation);

        Destroy(item);

    }

    [ClientRpc]
    public void RpcplaceItem(int id, Vector3 Position, Quaternion rotation)
    {
        GameObject spawnObj = Instantiate(gameItemDatabase.items[id], Position, rotation);
        spawnObj.name = id + " " + Position;
    }

    public void destroyForSlotIdentity()
    {
        //CmddestroyItem(itemToPlace);
    }

    [Command]
    public void CmddestroyNetworkedItem(GameObject item)
    {
        NetworkServer.Destroy(item);
    }

    [Command(requiresAuthority = false)]
    public void CmddestroyItem(Vector3 position, int id)
    {
        RpcdestroyItem(position, id);
        //Here we destroy the itemToPlace using the object's network identity, we use this as we cannot pass gameObjects as paramaters to commands.
        itemToPlace = null;

    }
    [ClientRpc]
    public void RpcdestroyItem(Vector3 position, int id)
    {
        GameObject obj = GameObject.Find(id + " " + position);
        if (obj != null)
        {
            Destroy(obj);
        } 
    }

    [Command]
    public void CmdremoveItem()
    {
        TargetplaceItem(connectionToClient);
    }
    [TargetRpc]
    public void TargetplaceItem(NetworkConnection client)
    {
        itemToPlace = null;
        //Here we are using a TargetRpc in order to set the itemToPlace to the object CmdspawnObject has instantiated, so the player can build with it.
    }
    #endregion
    #region building item placement
    [Command]
    public void CmditemMovement(GameObject itemToMove, Vector3 pos, Vector3 rot)
    {
            if (itemToMove != null)
            {
                itemToMove.transform.rotation = Quaternion.Euler(rot);
                itemToMove.transform.position = pos;
                RpcitemMovement(itemToMove, pos, rot);
            }
        
    }
    [ClientRpc]
    public void RpcitemMovement(GameObject itemToMove, Vector3 pos, Vector3 rot)
    {
        if (itemToMove != null)
        {
            itemToMove.transform.position = pos;
            itemToMove.transform.rotation = Quaternion.Euler(rot);
        }
    }
    /* HERE IN THE CMDITEMMOVEMENT AND RPCITEMMOVEMENT all we are doing is finding the itemToPlace gameObject through its netId, then setting its position (pos)
      and rotation (rot) to that of what was given in our itemMovement function, and applying that. We do this on the server, and through an RPC so clients see it. */
    #endregion

    int ColorSlot()
    {
        for (int i = 1; i < playersInventory.Inventory.Length; i++)
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

            if (i == playersInventory.Inventory.Length)
            {
                break;
                // if we have went through all slots then loop will breka.
            }



        }
        return inSlot;
    }

    [Command(requiresAuthority = false)]
    /*This function does a couple of things, 1. it will set the player's local username on it's local script to make it also be on the server's version of this script. 2. It will call an RPC that makes all player's already
      in the server view their correct username (RpcsetUsername), 3. it will loop through the server worldGenScript's list of player's, grab the individual player info (username and player object), and send it to a target rpc
      that will, locally for this client, set all aready joined player's in the server's username to their actual server name. */
    public void CmdsetUsername(string username, playerMovement playerScript, worldGenerator worldGenScript, NetworkConnectionToClient sender = null)
    {
        playerScript.username = username;
        playerScript.floatingUsername.text = username;
        RpcsetUsername(username, playerScript);
        foreach (playerInfo playerInf in worldGenScript.players)
        {
            TargetsetUsernames(sender, playerInf.username, playerInf.player);
        }
    }

    [ClientRpc]
    public void RpcsetUsername(string username, playerMovement playerScript)
    {
        playerScript.username = username;
        playerScript.floatingUsername.text = username;

        if (username == "Mason")
        {
            playerScript.floatingUsername.color = Color.red;
        }

    }

    [TargetRpc]
    public void TargetsetUsernames(NetworkConnection client, string Username, GameObject player)
    {
        GameObject usernameObj = player.transform.GetChild(1).gameObject;
        TextMesh username = usernameObj.GetComponent<TextMesh>();
        username.text = Username;

        if (Username == "Mason")
        {
            username.color = Color.red; 
        }


    }
}

