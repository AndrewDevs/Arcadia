using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class caveScript : NetworkBehaviour
{

    public int maxSpawn;
    public int minSpawn;
    public int yDeviation;
    public int xDeviation;
    public int amountToSpawn;
    public int ravineHeight;
    public int heightAmount;
    public int ravineFloorWidth;
    public int amountToSpawnRoomRight;
    public int amountToSpawnRoomLeft;
    public int amountToSpawnRoomWidth;
    int ravineFloorAmount;

    public List<GameObject> lengthObjects = new List<GameObject>();
    public List<GameObject> widthObjects = new List<GameObject>();
    public List<GameObject> ravineRightFloor = new List<GameObject>();
    public List<GameObject> ravineLeftFloor = new List<GameObject>();
    public List<GameObject> ravineFloors = new List<GameObject>();
    public List<GameObject> ravineWidthObjects = new List<GameObject>();

    public GameObject lastSpawnedObject;
    public GameObject caveObject;
    public GameObject lastRoomObject;
    public GameObject firstRoomObject;
    public GameObject objectList;
    public GameObject lastRightWall;
    public GameObject lastLeftWall;
    public GameObject caveSpawner;
    public GameObject lastFloor;
    public GameObject ravineFloorObject;
    public GameObject firstRavineFloor;
    public GameObject lastRavineRightForwardFloor;
    public GameObject lastRavineLeftForwardFloor;
    public GameObject lastRavineRightBackFloor;
    public GameObject lastRavineLeftBackFloor;
    public GameObject lastRavineMiddleFloorBack;

    public cullScriptHolder objectListScript;

    // Start is called before the first frame update
    public override void OnStartServer()
    {
        objectList = GameObject.Find("objectList(Clone)");
        //Find the networked list which we keep all of the worlds spawned objects in.
        objectListScript = objectList.GetComponent<cullScriptHolder>();
        //Derive that networked list's script from the found object.

        amountToSpawn = Random.Range(minSpawn, maxSpawn);
            spawnCave();
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region cave tunnel and room 
    public void spawnCave() {

        for (int i = amountToSpawn; i > 0; i--)
        {
            float yDifference = UnityEngine.Random.Range(1.5f, 2);
            float z = lastSpawnedObject.transform.position.z + caveObject.transform.localScale.z / 2;
            float floorY = lastSpawnedObject.transform.position.y - caveObject.transform.localScale.z / yDifference;
            Vector3 floorPosition = new Vector3(lastSpawnedObject.transform.position.x, floorY, z);
            Quaternion floorRotation = Quaternion.Euler(45, 0, 0);
            lastSpawnedObject = Instantiate(caveObject, floorPosition, floorRotation);
            NetworkServer.Spawn(lastSpawnedObject);
            objectListScript.cullObjects.Add(lastSpawnedObject);
            //Spawn Tunnel Floor

            float leftWallPosX = lastSpawnedObject.transform.position.x - caveObject.transform.localScale.x / 2;
            float leftWallPosY = lastSpawnedObject.transform.position.y + caveObject.transform.localScale.x / 2;
            Vector3 leftWallPosition = new Vector3(leftWallPosX, leftWallPosY, lastSpawnedObject.transform.position.z);
            Quaternion leftRotation = Quaternion.Euler(45, 0, 90);
            GameObject leftWall = Instantiate(caveObject, leftWallPosition, leftRotation);
            NetworkServer.Spawn(leftWall);
            objectListScript.cullObjects.Add(leftWall);
            //Spawn Left Wall

            float rightWallPosX = lastSpawnedObject.transform.position.x + caveObject.transform.localScale.x / 2;
            float rightWallPosY = lastSpawnedObject.transform.position.y + caveObject.transform.localScale.x / 2;
            Vector3 rightWallPosition = new Vector3(rightWallPosX, rightWallPosY, lastSpawnedObject.transform.position.z);
            Quaternion rightRotation = Quaternion.Euler(45, 0, 90);
            GameObject rightWall = Instantiate(caveObject, rightWallPosition, rightRotation);
            NetworkServer.Spawn(rightWall);
            objectListScript.cullObjects.Add(rightWall);
            //Spawn Right Wall

            float y = lastSpawnedObject.transform.position.y + lastSpawnedObject.transform.localScale.z;
            Vector3 roofPosition = new Vector3(lastSpawnedObject.transform.position.x, y, lastSpawnedObject.transform.position.z);
            Quaternion roofRotation = Quaternion.Euler(45, 0, 0);
            GameObject Roof = Instantiate(caveObject, roofPosition, roofRotation);
            NetworkServer.Spawn(Roof);
            objectListScript.cullObjects.Add(Roof);
            amountToSpawn--;
            //Spawn Tunnel Roof.

            if(amountToSpawn == 0)
            {
                spawnRoomRight();
                //When look is completed call the spawnRoom Function, this function will spawn a cave room.
            }

        }



    }

    public void spawnRoomRight()
    {
        float yPosition = yDeviation + lastSpawnedObject.transform.position.y;
        Vector3 firstPosition = new Vector3(lastSpawnedObject.transform.position.x, UnityEngine.Random.Range(yPosition, lastSpawnedObject.transform.position.y), lastSpawnedObject.transform.position.z + caveObject.transform.localScale.z / 2);
        firstRoomObject = Instantiate(caveObject, firstPosition, Quaternion.identity);
        NetworkServer.Spawn(firstRoomObject);
        lastRoomObject = firstRoomObject;
        lengthObjects.Add(lastRoomObject);
        widthObjects.Add(lastRoomObject);
        objectListScript.cullObjects.Add(lastRoomObject);
        //This above code spawns the very first cave floor, which is the starting point for all of our others.

        for (int i = amountToSpawnRoomRight; i > 0; i--)
        {
            float roomRight = lastRoomObject.transform.position.x + caveObject.transform.localScale.x;
            Vector3 roomFloorPosition = new Vector3(roomRight, lastRoomObject.transform.position.y, lastRoomObject.transform.position.z);
            lastRoomObject = Instantiate(caveObject, roomFloorPosition, Quaternion.identity);
            NetworkServer.Spawn(lastRoomObject);
            lengthObjects.Add(lastRoomObject);
            widthObjects.Add(lastRoomObject);
            objectListScript.cullObjects.Add(lastRoomObject);
            //Spawn Floor
            float roomRightWall = lastRoomObject.transform.position.z - caveObject.transform.localScale.x /2;
            Vector3 roomRightWallPosition = new Vector3(lastRoomObject.transform.position.x, lastRoomObject.transform.position.y + caveObject.transform.localScale.z / 2, roomRightWall);
            Quaternion rightWallRotation = Quaternion.Euler(0, 90, 90);
            GameObject rightWallObject = Instantiate(caveObject, roomRightWallPosition, rightWallRotation);
            NetworkServer.Spawn(rightWallObject);
            objectListScript.cullObjects.Add(rightWallObject);
            //Spawn Right Wall


        }

        spawnRoomLeft();

    }

    public void spawnRoomLeft()
    {
        lastRoomObject = firstRoomObject;
        for (int i = amountToSpawnRoomLeft; i > 0; i--)
        {
            float yPosition = yDeviation + lastSpawnedObject.transform.position.y;
            float roomLeft = lastRoomObject.transform.position.x - caveObject.transform.localScale.x;
            Vector3 roomFloorPosition = new Vector3(roomLeft, UnityEngine.Random.Range(yPosition, lastSpawnedObject.transform.position.y), lastRoomObject.transform.position.z);
            lastRoomObject = Instantiate(caveObject, roomFloorPosition, Quaternion.identity);
            NetworkServer.Spawn(lastRoomObject);
            lengthObjects.Add(lastRoomObject);
            widthObjects.Add(lastRoomObject);
            objectListScript.cullObjects.Add(lastRoomObject);
            //Spawn Floor

            float roomLeftWall = lastRoomObject.transform.position.z - caveObject.transform.localScale.x / 2;
            Vector3 roomLeftWallPosition = new Vector3(lastRoomObject.transform.position.x, lastRoomObject.transform.position.y + caveObject.transform.localScale.z / 2, roomLeftWall);
            Quaternion leftWallRotation = Quaternion.Euler(0, 90, 90);
            GameObject leftWallObject = Instantiate(caveObject, roomLeftWallPosition, leftWallRotation);
            NetworkServer.Spawn(leftWallObject);
            objectListScript.cullObjects.Add(leftWallObject);
            //Spawn Back Wall
            if (i == 1)
            {
                spawnCaveWalls();
            }
        }
        spawnRoomForward();
    }

    public void spawnCaveWalls()
    {
        lastRightWall = lastRoomObject;
        lastLeftWall = lastRoomObject;
        for (int i = amountToSpawnRoomWidth; i > -1; i--)
        {
            int tunnelChance = Random.Range(1, 13);
            if (tunnelChance != 3)
            {
                float roomRightSideWallX = caveObject.transform.localScale.x * amountToSpawnRoomLeft;
                float roomRightSideWallZ = caveObject.transform.localScale.x* i;
                Vector3 roomRightSideWallPosition = new Vector3(firstRoomObject.transform.position.x + roomRightSideWallX, firstRoomObject.transform.position.y + caveObject.transform.localScale.z / 2, firstRoomObject.transform.position.z + roomRightSideWallZ);
                Quaternion roomRightSideWallRotation = Quaternion.Euler(0, 0, 90);
                lastRightWall = Instantiate(caveObject, roomRightSideWallPosition, roomRightSideWallRotation);
                NetworkServer.Spawn(lastRightWall);
                objectListScript.cullObjects.Add(lastRightWall);
            }
            else
            {
                spawnTunnellRight(i, lastRightWall);
            }

            float roomLeftSideWallX = caveObject.transform.localScale.x * amountToSpawnRoomLeft;
            //find out the amount left we must move to set the wall on the X Axis, we just times the localScale.x * the amount of caveObjects we have spawned X.
            float roomLeftSideWallZ = caveObject.transform.localScale.x* i + firstRoomObject.transform.position.z;
            /*Find the amount forward (Z) we must move to set the wall along the z Axis, here all we are doing is multiplaying the caveObject.localScale.x times the i, we multiple by the I so we can multiply backwards
              say i at start is 20, then the float will be set to the position at 20, and it will move down to 19, etc. We then add that to the firstRoomObject's position to set it somewhere "local"/*/
            Vector3 roomLeftSideWallPosition = new Vector3(firstRoomObject.transform.position.x - roomLeftSideWallX, firstRoomObject.transform.position.y + caveObject.transform.localScale.z / 2, roomLeftSideWallZ);
            //Set the position for the wall, subtract the position.x by the roomLeftSideWall as we are moving negative X, same logic applies to the rest.
            Quaternion roomLeftSideWallRotation = Quaternion.Euler(0, 0, 90);
            lastLeftWall = Instantiate(caveObject, roomLeftSideWallPosition, roomLeftSideWallRotation);
            NetworkServer.Spawn(lastLeftWall);
            objectListScript.cullObjects.Add(lastLeftWall);
        }
    }

    public void spawnRoomForward()
    {
        for (int i = 0; i < lengthObjects.Count; i++)
        {
            lastRoomObject = lengthObjects[i];

            for (int j = amountToSpawnRoomWidth; j > 0; j--)
            {
                float yPosition = yDeviation + lastSpawnedObject.transform.position.y;
                float roomForward = lastRoomObject.transform.position.z + caveObject.transform.localScale.z;
                Vector3 roomFloorWidthPosition = new Vector3(lastRoomObject.transform.position.x, UnityEngine.Random.Range(yPosition, lastSpawnedObject.transform.position.y), roomForward);
                lastRoomObject = Instantiate(caveObject, roomFloorWidthPosition, Quaternion.identity);
                NetworkServer.Spawn(lastRoomObject);
                widthObjects.Add(lastRoomObject);
                objectListScript.cullObjects.Add(lastRoomObject);
                //Spawn forward Floor

                int BlockChance = UnityEngine.Random.Range(1, 8);
                if (BlockChance == 2)
                {
                    Vector3 scale = new Vector3(lastRoomObject.transform.localScale.x, 20, lastRoomObject.transform.localScale.z);
                    lastRoomObject.transform.localScale = scale;
                }
            }

            float roomForwardWallZ = caveObject.transform.localScale.z * amountToSpawnRoomWidth;
            Vector3 roomForwardWallPosition = new Vector3(lengthObjects[i].transform.position.x, firstRoomObject.transform.position.y + caveObject.transform.localScale.z / 2, lengthObjects[i].transform.position.z + roomForwardWallZ);
            Quaternion roomForwardWallRotation = Quaternion.Euler(90, 0, 0);
            GameObject forwardWall = Instantiate(caveObject, roomForwardWallPosition, roomForwardWallRotation);
            NetworkServer.Spawn(forwardWall);
            objectListScript.cullObjects.Add(forwardWall);
            //Forward Wall


            spawnCaveRoof();

        }
    }

    public void spawnCaveRoof()
    {
        for (int i = 0; i < widthObjects.Count; i++)
        {
            float y = widthObjects[i].transform.position.y + caveObject.transform.localScale.z;
            Vector3 roofPosition = new Vector3(widthObjects[i].transform.position.x, y, widthObjects[i].transform.position.z);
            GameObject Roof = Instantiate(caveObject, roofPosition, Quaternion.identity);
            NetworkServer.Spawn(Roof);
            objectListScript.cullObjects.Add(Roof);
        }

    }

    #endregion


    public void spawnTunnellRight(int j, GameObject lastObject)
    {
        lastFloor = lastObject;

        float roomRightSideWallX = caveObject.transform.localScale.x * amountToSpawnRoomLeft;
        float roomRightSideWallZ = caveObject.transform.localScale.x * j;
        Vector3 roomRightSideWallPosition = new Vector3(firstRoomObject.transform.position.x + roomRightSideWallX, lastFloor.transform.position.y - caveObject.transform.localScale.z / 2, firstRoomObject.transform.position.z + roomRightSideWallZ);
        Quaternion roomRightSideWallRotation = Quaternion.Euler(0, 0, 0);
        lastFloor = Instantiate(caveObject, roomRightSideWallPosition, roomRightSideWallRotation);
        NetworkServer.Spawn(lastFloor);
        objectListScript.cullObjects.Add(lastFloor);
        //First caveRightObject to set a position for the loop to begin off of.

        float firstLeftWallPosX = lastFloor.transform.position.x + caveObject.transform.localScale.x / 2;
        float firstLeftWallPosY = lastFloor.transform.position.y + caveObject.transform.localScale.z / 2;
        float firstLeftWallPosZ = lastFloor.transform.position.z + caveObject.transform.localScale.z / 2;
        Vector3 firstLeftWallPosition = new Vector3(firstLeftWallPosX, firstLeftWallPosY, firstLeftWallPosZ);
        Quaternion firstLeftRotation = Quaternion.Euler(90, 0, 0);
        GameObject firstLeftWall = Instantiate(caveObject, firstLeftWallPosition, firstLeftRotation);
        NetworkServer.Spawn(firstLeftWall);
        objectListScript.cullObjects.Add(firstLeftWall);
        //Spawn Left Wall
        float firstRightWallPosX = lastFloor.transform.position.x + caveObject.transform.localScale.x / 2;
        float firstRightWallPosY = lastFloor.transform.position.y + caveObject.transform.localScale.z / 2;
        float firstRightWallPosZ = lastFloor.transform.position.z - caveObject.transform.localScale.z / 2;
        Vector3 firstRightWallPosition = new Vector3(firstRightWallPosX, firstRightWallPosY, firstRightWallPosZ);
        Quaternion firstRightRotation = Quaternion.Euler(90, 0, 0);
        GameObject firstRightWall = Instantiate(caveObject, firstRightWallPosition, firstRightRotation);
        NetworkServer.Spawn(firstRightWall);
        objectListScript.cullObjects.Add(firstRightWall);
        //Spawn Right Wall
        float firsty = lastFloor.transform.position.y + caveObject.transform.localScale.z;
        Vector3 firstRoofPosition = new Vector3(lastFloor.transform.position.x, firsty, lastFloor.transform.position.z);
        GameObject firstRoof = Instantiate(caveObject, firstRoofPosition, Quaternion.identity);
        NetworkServer.Spawn(firstRoof);
        objectListScript.cullObjects.Add(firstRoof);
        //Spawn Roof

        int amount = UnityEngine.Random.Range(minSpawn, maxSpawn);

        for (int i = amount; i > 0; i--)
        {
            float floorYUp = lastFloor.transform.position.y + yDeviation;
            float floorYDown = lastFloor.transform.position.y - yDeviation;
            Vector3 rightFloorPosition = new Vector3(lastFloor.transform.position.x + caveObject.transform.localScale.x, UnityEngine.Random.Range(floorYUp, floorYDown), lastFloor.transform.position.z - 5);
            lastFloor = Instantiate(caveObject, rightFloorPosition, Quaternion.identity);
            NetworkServer.Spawn(lastFloor);
            objectListScript.cullObjects.Add(lastFloor);

            float leftWallPosY = lastFloor.transform.position.y + caveObject.transform.localScale.z / 2;
            float leftWallPosZ = lastFloor.transform.position.z + caveObject.transform.localScale.z / 2;
            Vector3 leftWallPosition = new Vector3(lastFloor.transform.position.x, leftWallPosY, leftWallPosZ);
            Quaternion leftRotation = Quaternion.Euler(90, 0, 0);
            GameObject leftWall = Instantiate(caveObject, leftWallPosition, leftRotation);
            NetworkServer.Spawn(leftWall);
            objectListScript.cullObjects.Add(leftWall);
            //Spawn Left Wall

            float rightWallPosY = lastFloor.transform.position.y + caveObject.transform.localScale.z / 2;
            float rightWallPosZ = lastFloor.transform.position.z - caveObject.transform.localScale.z / 2;
            Vector3 rightWallPosition = new Vector3(lastFloor.transform.position.x, rightWallPosY, rightWallPosZ);
            Quaternion rightRotation = Quaternion.Euler(90, 0, 0);
            GameObject rightWall = Instantiate(caveObject, rightWallPosition, rightRotation);
            NetworkServer.Spawn(rightWall);
            objectListScript.cullObjects.Add(rightWall);
            //Spawn Right Wall

            float y = lastFloor.transform.position.y + caveObject.transform.localScale.z;
            Vector3 RoofPosition = new Vector3(lastFloor.transform.position.x, y, lastFloor.transform.position.z);
            GameObject Roof = Instantiate(caveObject, RoofPosition, Quaternion.identity);
            NetworkServer.Spawn(Roof);
            objectListScript.cullObjects.Add(Roof);
            //Spawn Roof

            if (i == 1)
            {
                int chance = UnityEngine.Random.Range(0, 5);
                if(chance == 3)
                {
                    spawnCaveHole();
                }
            }
        }


    }


    public void spawnCaveHole()
    {

        float forwardWallX = lastFloor.transform.position.x + caveObject.transform.localScale.z / 2;
        float forwardWallPosY = lastFloor.transform.position.y + caveObject.transform.localScale.z / 2;
        Vector3 forwardWallPosition = new Vector3(forwardWallX, forwardWallPosY, lastFloor.transform.position.z);
        Quaternion forwardWallRotation = Quaternion.Euler(0, 0, 90);
        GameObject forwardWall = Instantiate(caveObject, forwardWallPosition, forwardWallRotation);
        NetworkServer.Spawn(forwardWall);
        objectListScript.cullObjects.Add(forwardWall);
        //Spawn Forward Wall to end tunnel right.

        float firstFloorYUp = lastFloor.transform.position.y + yDeviation;
        float firstYDown = lastFloor.transform.position.y - yDeviation;
        float firstXFloor = lastFloor.transform.position.x - caveObject.transform.localScale.z / 2;
        Vector3 firstRightFloorPosition = new Vector3(firstXFloor, lastFloor.transform.position.y - caveObject.transform.localScale.z / 2, lastFloor.transform.position.z - 5);
        Quaternion firstFloorDownRotation = Quaternion.Euler(0, 0, 90);
        GameObject firstFloor = Instantiate(caveObject, firstRightFloorPosition, firstFloorDownRotation);
        NetworkServer.Destroy(lastFloor);
        lastFloor = firstFloor;
        NetworkServer.Spawn(lastFloor);
        objectListScript.cullObjects.Add(lastFloor);

        float firstLeftWallPosX = lastFloor.transform.position.x + caveObject.transform.localScale.z / 2;
        float firstLeftWallPosZ = lastFloor.transform.position.z + caveObject.transform.localScale.z / 2;
        Vector3 firstLeftWallPosition = new Vector3(firstLeftWallPosX, lastFloor.transform.position.y, firstLeftWallPosZ);
        Quaternion firstLeftRotation = Quaternion.Euler(90, 0, 0);
        GameObject firstLeftWall = Instantiate(caveObject, firstLeftWallPosition, firstLeftRotation);
        NetworkServer.Spawn(firstLeftWall);
        objectListScript.cullObjects.Add(firstLeftWall);
        //Spawn Left Wall

        float firstRightWallPosX = lastFloor.transform.position.x + caveObject.transform.localScale.z / 2;
        float firstRightWallPosZ = lastFloor.transform.position.z - caveObject.transform.localScale.z / 2;
        Vector3 firstRightWallPosition = new Vector3(firstRightWallPosX, lastFloor.transform.position.y, firstRightWallPosZ);
        Quaternion firstRightRotation = Quaternion.Euler(90, 0, 0);
        GameObject firstRightWall = Instantiate(caveObject, firstRightWallPosition, firstRightRotation);
        NetworkServer.Spawn(firstRightWall);
        objectListScript.cullObjects.Add(firstRightWall);
        //Spawn Right Wall

        float firstX = lastFloor.transform.position.x + caveObject.transform.localScale.z;
        Vector3 firstRoofPosition = new Vector3(firstX, lastFloor.transform.position.y, lastFloor.transform.position.z);
        Quaternion firstRoofDownRotation = Quaternion.Euler(0, 0, 90);
        GameObject firstRoof = Instantiate(caveObject, firstRoofPosition, firstRoofDownRotation);
        NetworkServer.Spawn(firstRoof);
        objectListScript.cullObjects.Add(firstRoof);

        int amount = UnityEngine.Random.Range(minSpawn, maxSpawn);

        for (int i = amount; i > 0; i--)
        {
            float floorYUp = lastFloor.transform.position.y + yDeviation;
            float floorYDown = lastFloor.transform.position.y - yDeviation;
            Vector3 rightFloorPosition = new Vector3(lastFloor.transform.position.x, lastFloor.transform.position.y - caveObject.transform.localScale.z, lastFloor.transform.position.z - 5);
            Quaternion floorDownRotation = Quaternion.Euler(0, 0, 90);
            lastFloor = Instantiate(caveObject, rightFloorPosition, floorDownRotation);
            NetworkServer.Spawn(lastFloor);
            objectListScript.cullObjects.Add(lastFloor);

            float leftWallPosX = lastFloor.transform.position.x + caveObject.transform.localScale.z / 2;
            float leftWallPosZ = lastFloor.transform.position.z + caveObject.transform.localScale.z / 2;
            Vector3 leftWallPosition = new Vector3(leftWallPosX, lastFloor.transform.position.y, leftWallPosZ);
            Quaternion leftRotation = Quaternion.Euler(90, 0, 0);
            GameObject leftWall = Instantiate(caveObject, leftWallPosition, leftRotation);
            NetworkServer.Spawn(leftWall);
            objectListScript.cullObjects.Add(leftWall);
            //Spawn Left Wall

            float rightWallPosX = lastFloor.transform.position.x + caveObject.transform.localScale.z / 2;
            float rightWallPosZ = lastFloor.transform.position.z - caveObject.transform.localScale.z / 2;
            Vector3 rightWallPosition = new Vector3(rightWallPosX, lastFloor.transform.position.y, rightWallPosZ);
            Quaternion rightRotation = Quaternion.Euler(90, 0, 0);
            GameObject rightWall = Instantiate(caveObject, rightWallPosition, rightRotation);
            NetworkServer.Spawn(rightWall);
            objectListScript.cullObjects.Add(rightWall);
            //Spawn Right Wall

            float x = lastFloor.transform.position.x + caveObject.transform.localScale.z;
            Vector3 RoofPosition = new Vector3(x, lastFloor.transform.position.y, lastFloor.transform.position.z);
            Quaternion roofDownRotation = Quaternion.Euler(0, 0, 90);
            GameObject Roof = Instantiate(caveObject, RoofPosition, roofDownRotation);
            NetworkServer.Spawn(Roof);
            objectListScript.cullObjects.Add(Roof);
            //Spawn Roof

            if (i == 1)
            {
                spawnRavine();
            }
        }

    }

    public void spawnRavine()
    {
        float firstRavineFloorY = caveObject.transform.localScale.z * ravineHeight * -1;
        //Multiple firstRavineFloorY by -1 so that we are adding a negative to a negative to go lower, instead of a negative to a positive which would raise the Y value.
        float firstRavineFloorX = lastFloor.transform.position.x + caveObject.transform.localScale.x;
        Vector3 firstRavineFloorPos = new Vector3(firstRavineFloorX, lastFloor.transform.position.y + firstRavineFloorY, lastFloor.transform.position.z);
        Quaternion firstRavineFloorRot = Quaternion.Euler(0, 0, 0);
        firstRavineFloor = Instantiate(ravineFloorObject, firstRavineFloorPos, firstRavineFloorRot);
        NetworkServer.Spawn(firstRavineFloor);
        ravineWidthObjects.Add(firstRavineFloor);
        lastFloor = firstRavineFloor;
        lastRavineMiddleFloorBack = lastFloor;
        objectListScript.cullObjects.Add(firstRavineFloor);
        //Spawn first ravine Floor

        float firstRavineFloorXLeft = lastFloor.transform.position.x - caveObject.transform.localScale.x;
        Vector3 firstRavineFloorLeftPos = new Vector3(firstRavineFloorXLeft, lastFloor.transform.position.y, lastFloor.transform.position.z);
        GameObject firstRavineFloorLeft = Instantiate(ravineFloorObject, firstRavineFloorLeftPos, Quaternion.identity);
        NetworkServer.Spawn(firstRavineFloorLeft);
        ravineLeftFloor.Add(firstRavineFloorLeft);
        ravineWidthObjects.Add(firstRavineFloorLeft);
        ravineFloors.Add(firstRavineFloorLeft);
        lastRavineLeftBackFloor = firstRavineFloorLeft;
        lastRavineLeftForwardFloor = firstRavineFloorLeft;
        objectListScript.cullObjects.Add(firstRavineFloorLeft);
        //Spawn first left ravine Floor

        float firstRavineFloorXRight = lastFloor.transform.position.x + caveObject.transform.localScale.x;
        Vector3 firstRavineFloorRightPos = new Vector3(firstRavineFloorXRight, lastFloor.transform.position.y, lastFloor.transform.position.z);
        GameObject firstRavineFloorRight = Instantiate(ravineFloorObject, firstRavineFloorRightPos, Quaternion.identity);
        NetworkServer.Spawn(firstRavineFloorRight);
        ravineRightFloor.Add(firstRavineFloorRight);
        ravineWidthObjects.Add(firstRavineFloorRight);
        ravineFloors.Add(firstRavineFloorRight);
        lastRavineRightBackFloor = firstRavineFloorRight;
        lastRavineRightForwardFloor = firstRavineFloorRight;
        objectListScript.cullObjects.Add(firstRavineFloorRight);
        //Spawn first right ravine Floor

        ravineFloorAmount = UnityEngine.Random.Range(minSpawn, maxSpawn);

        for (int i = ravineFloorAmount; i > 0; i--)
        {
            float ravineFloorForwardZ = lastFloor.transform.position.z + caveObject.transform.localScale.x;
            Vector3 ravineForwardFloorPos = new Vector3(lastFloor.transform.position.x, lastFloor.transform.position.y, ravineFloorForwardZ);
            GameObject ravineMiddleForward = Instantiate(ravineFloorObject, ravineForwardFloorPos, Quaternion.identity);
            NetworkServer.Spawn(ravineMiddleForward);
            lastFloor = ravineMiddleForward;
            ravineFloors.Add(ravineMiddleForward);
            objectListScript.cullObjects.Add(ravineMiddleForward);

            float ravineFloorBackZ = lastRavineMiddleFloorBack.transform.position.z - caveObject.transform.localScale.x;
            Vector3 ravineBackFloorPos = new Vector3(lastRavineMiddleFloorBack.transform.position.x, lastRavineMiddleFloorBack.transform.position.y, ravineFloorBackZ);
            GameObject ravineMiddleBack = Instantiate(ravineFloorObject, ravineBackFloorPos, Quaternion.identity);
            NetworkServer.Spawn(ravineMiddleBack);
            lastRavineMiddleFloorBack = ravineMiddleBack;
            ravineFloors.Add(ravineMiddleBack);
            objectListScript.cullObjects.Add(ravineMiddleBack);
            //Spawn the middle floors, back and forwards, on the ravine Bottom.

            float ravineFloorLeftZ = lastRavineLeftForwardFloor.transform.position.z + caveObject.transform.localScale.x;
            Vector3 ravineFloorLeftPos = new Vector3(lastRavineLeftForwardFloor.transform.position.x, lastRavineLeftForwardFloor.transform.position.y, ravineFloorLeftZ);
            GameObject ravineLeftForward = Instantiate(ravineFloorObject, ravineFloorLeftPos, Quaternion.identity);
            NetworkServer.Spawn(ravineLeftForward);
            lastRavineLeftForwardFloor = ravineLeftForward;
            ravineLeftFloor.Add(ravineLeftForward);
            ravineFloors.Add(ravineLeftForward);
            objectListScript.cullObjects.Add(ravineLeftForward);

            float ravineLeftBackZ = lastRavineLeftBackFloor.transform.position.z - caveObject.transform.localScale.x;
            Vector3 ravineBackLeftPos = new Vector3(lastRavineLeftBackFloor.transform.position.x, lastRavineLeftBackFloor.transform.position.y, ravineLeftBackZ);
            GameObject ravineLeftBack = Instantiate(ravineFloorObject, ravineBackLeftPos, Quaternion.identity);
            NetworkServer.Spawn(ravineLeftBack);
            lastRavineLeftBackFloor = ravineLeftBack;
            ravineLeftFloor.Add(ravineLeftBack);
            ravineFloors.Add(ravineLeftBack);
            objectListScript.cullObjects.Add(ravineLeftBack);
            //Spawn the left floors, back and forwards, on the bottom of the ravine.

            float ravineFloorRightZ = lastRavineRightForwardFloor.transform.position.z + caveObject.transform.localScale.x;
            Vector3 ravineFloorRightPos = new Vector3(lastRavineRightForwardFloor.transform.position.x, lastRavineRightForwardFloor.transform.position.y, ravineFloorRightZ);
            GameObject ravineRightForward = Instantiate(ravineFloorObject, ravineFloorRightPos, Quaternion.identity);
            NetworkServer.Spawn(ravineRightForward);
            lastRavineRightForwardFloor = ravineRightForward;
            ravineRightFloor.Add(ravineRightForward);
            ravineFloors.Add(ravineRightForward);
            objectListScript.cullObjects.Add(ravineRightForward);


            float ravineRightBackZ = lastRavineRightBackFloor.transform.position.z - caveObject.transform.localScale.x;
            Vector3 ravineBackRightPos = new Vector3(lastRavineRightBackFloor.transform.position.x, lastRavineRightBackFloor.transform.position.y, ravineRightBackZ);
            GameObject ravineRightBack = Instantiate(ravineFloorObject, ravineBackRightPos, Quaternion.identity);
            NetworkServer.Spawn(ravineRightBack);
            lastRavineRightBackFloor = ravineRightBack;
            ravineRightFloor.Add(ravineRightBack);
            ravineFloors.Add(ravineRightBack);
            objectListScript.cullObjects.Add(ravineRightBack);
            //Spawn the right floors, back and forwards, on the bottom of the ravine.

            if (i == 1)
            {
                ravineWallsHeight();
                spawnRavineRoof();
            }
        }



    }

    public void ravineWallsHeight()
    {
        for (int i = ravineHeight; i > 0; i--)
        {
            heightAmount = i;

                spawnRavineWalls();           
        }

    }

   

    public void spawnRavineWalls()
    {
        for (int i = 0; i < ravineLeftFloor.Count; i++)
        {
            float xDevLeft = Random.Range(ravineLeftFloor[i].transform.position.x - xDeviation, ravineLeftFloor[i].transform.position.x + xDeviation);
            float leftWallPosX = xDevLeft - caveObject.transform.localScale.x / 2;
            float leftWallPosY =  caveObject.transform.localScale.x * heightAmount;
            Vector3 leftWallPosition = new Vector3(leftWallPosX, ravineLeftFloor[i].transform.position.y + leftWallPosY, ravineLeftFloor[i].transform.position.z);
            Quaternion leftRotation = Quaternion.Euler(0, 0, 90);
            GameObject leftWall = Instantiate(caveObject, leftWallPosition, leftRotation);
            NetworkServer.Spawn(leftWall);
            objectListScript.cullObjects.Add(leftWall);
            //Spawn Left Wall
        }

        for (int i = 0; i < ravineRightFloor.Count; i++)
        {
            float xDevRight = Random.Range(ravineRightFloor[i].transform.position.x - xDeviation, ravineRightFloor[i].transform.position.x + xDeviation);
            float rightWallPosX = xDevRight + caveObject.transform.localScale.x / 2;
            float rightWallPosY = caveObject.transform.localScale.x * heightAmount;
            Vector3 rightWallPosition = new Vector3(rightWallPosX, ravineRightFloor[i].transform.position.y + rightWallPosY, ravineRightFloor[i].transform.position.z);
            Quaternion rightRotation = Quaternion.Euler(0, 0, 90);
            GameObject rightWall = Instantiate(caveObject, rightWallPosition, rightRotation);
            NetworkServer.Spawn(rightWall);
            objectListScript.cullObjects.Add(rightWall);
            //Spawn Right Wall
        }

        for (int i = 0; i < ravineWidthObjects.Count; i++)
        {
            float zDevForward = Random.Range(ravineWidthObjects[i].transform.position.z - xDeviation, ravineWidthObjects[i].transform.position.z + xDeviation);
            float ravineForwardWallZ = caveObject.transform.localScale.z * ravineFloorAmount;
            float ravineForwardWallY = caveObject.transform.localScale.x* heightAmount;
            Vector3 ravineForwardWallPos = new Vector3(ravineWidthObjects[i].transform.position.x, ravineWidthObjects[i].transform.position.y + ravineForwardWallY, zDevForward + ravineForwardWallZ);
            Quaternion forwardRotation = Quaternion.Euler(90, 0, 0);
            GameObject forwardWall = Instantiate(caveObject, ravineForwardWallPos, forwardRotation);
            NetworkServer.Spawn(forwardWall);
            objectListScript.cullObjects.Add(forwardWall);
            //Forward wall

            float zDevBack = Random.Range(ravineWidthObjects[i].transform.position.z - xDeviation, ravineWidthObjects[i].transform.position.z + xDeviation);
            float ravineBackWallZ = caveObject.transform.localScale.z * ravineFloorAmount;
            float ravineBackWallY = caveObject.transform.localScale.x * heightAmount;
            Vector3 ravineBackWallPos = new Vector3(ravineWidthObjects[i].transform.position.x, ravineWidthObjects[i].transform.position.y + ravineBackWallY, zDevBack - ravineBackWallZ);
            Quaternion BackRotation = Quaternion.Euler(90, 0, 0);
            GameObject BackWall = Instantiate(caveObject, ravineBackWallPos, BackRotation);
            NetworkServer.Spawn(BackWall);
            objectListScript.cullObjects.Add(BackWall);
            //Back wall
        }

    }

    public void spawnRavineRoof()
    {
        for (int i = 0; i < ravineFloors.Count; i++)
        {
            float ravineRoofY = caveObject.transform.localScale.x * ravineHeight;
            Vector3 ravineRoofPos = new Vector3(ravineFloors[i].transform.position.x, lastFloor.transform.position.y + ravineRoofY, ravineFloors[i].transform.position.z);
            GameObject ravineRoof = Instantiate(caveObject, ravineRoofPos, Quaternion.identity);
            NetworkServer.Spawn(ravineRoof);
            objectListScript.cullObjects.Add(ravineRoof);
        }

    }

}
