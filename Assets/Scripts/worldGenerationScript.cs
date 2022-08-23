using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.AccessControl;
using UnityEngine;

public class worldGenerationScript : MonoBehaviour
{
    int onNumber;
    int onWidthObjectsNumer;
    //we will use this int to find out which object on the widthObjects int we are on.
    public int xChunkMax;
    public int xChunkMin;

    public int yArea;

    public float waterLevel;

    public GameObject objectToBeSpawned;
    // This is used when we are spawning chunks. I made this public so multiple functions can use it I guess.
    public chunkObjectPropetries[] objectsToSpawn;
    // We use array here for our probability system.
    public List<int> numberOfObjectsToSpawn = new List<int>();
    //NumberofObjectsToSpawn[0] = width.x, NumberofObjectsToSpawn[1] = width.z
    public List<GameObject> widthObjects = new List<GameObject>();
    //We are going to use this for further generation, this is used to locate the width objects spawned.

    public GameObject lastSpawnedObject;

    public playerTracker cullingScript;

    public bool On;



    // Start is called before the first frame update
    void Start()
    {
        goDownSpawnList();

    }



    // Update is called once per frame
    void Update()
    {

    }


    void goDownSpawnList()
    {
        onNumber = -0;

        for (onNumber = 0; onNumber < objectsToSpawn.Count(); onNumber++)
        {
            if (onNumber == 0)
            {
                WidthX();
            }
            else
            {
                WidthZ();
            }


        }
        //This loop merely goes down the list of items to spawn, onNumber tells us which number in the list the loop is on. If it has not reached the end of the list it runs SpawnAmount();

    }


    void WidthX()
    {
        int i = -0;

        bool waterChunk = false;

        Vector3 spawnPositionX;

        for (i = numberOfObjectsToSpawn[onNumber]; i != 0; i--)
        {
            int randomNumber = UnityEngine.Random.Range(0, 100);
            //We start with this to choose a random number between 0 and 100.
            for (int j = 0; j < objectsToSpawn.Count(); j++)
            // This loop goes through each thing in our objectsToSpawn array.
            {

                if (randomNumber >= objectsToSpawn[j].minChance && randomNumber <= objectsToSpawn[j].maxChance)
                // here we check if our randomNumber is less than or equal to our selected objects minChance and MaxChance, it will do this for each object in the list.
                {
                    objectToBeSpawned = objectsToSpawn[j].chunk;
                    if(objectsToSpawn[j].isWater == true)
                    {
                        waterChunk = true;
                    }
                    else
                    {
                        waterChunk = false;
                    }


                    // We then set objectsToSpawn to the number which fits inside this particular objects min and maxchances.
                    break;
                    // add break to make sure it doesn't keep running.
                }
            }

            // goDownSpawnList();
            float x = lastSpawnedObject.transform.position.x + lastSpawnedObject.transform.localScale.x;
            // sets position to spawn object next to lastSpawnedObject.transform on x scale.
            if (waterChunk == false)
            {
                if (lastSpawnedObject.name == "waterChunk(Clone)")
                {
                     spawnPositionX = new Vector3(x, UnityEngine.Random.Range(lastSpawnedObject.transform.position.y, yArea) - waterLevel, lastSpawnedObject.transform.position.z);
                    //sets the spawnPosition to what we want it to be, here it is the right.
                }
                else
                {
                     spawnPositionX = new Vector3(x, UnityEngine.Random.Range(lastSpawnedObject.transform.position.y, yArea), lastSpawnedObject.transform.position.z);

                }
            }
            else
            {
                spawnPositionX = new Vector3(x, lastSpawnedObject.transform.position.y + waterLevel, lastSpawnedObject.transform.position.z);

            }
            lastSpawnedObject = Instantiate(objectToBeSpawned, spawnPositionX, Quaternion.identity);
            //Spawns object, and sets are most recently spawned object to last spawned.
            cullingScript.chunks.Add(lastSpawnedObject);
            // Adding all spawned chunk, which here is under lastSpawnedObject, to our chunk list for culling.
            cullingScript.lastChunkObject = lastSpawnedObject;
            cullingScript.checkChunkDistance();
            //here we run the function to see the distance of the last chunk spawned from the player, for procedural generation.


            numberOfObjectsToSpawn[onNumber]--;
        }


    }




        //Here we get the number of items to spawn by stating numberOfITemsToSpawn[OnNumber], which tells us the amount needed to spawn on whatever number the prior loop is on.
        // From there we spawn the item the loop is on in the list, and subtract 1 from numberOfItemsToSpawn, we locate it with OnNumber.

    

    void WidthZ()
    {
        Vector3 spawnPositionX;
        // We set this in the beginning of our function so it can be used in any part of it, we have to do this due to our water if statements when we set spawnPositionX.
        bool waterChunk = false;
        // this is out waterchunk book, which will be used to see if we are spawning a water chunk or not, it is set when we chose a chunk to spawn.
        int i = -0;

        for (i = numberOfObjectsToSpawn[onNumber]; i != 0; i--)
        {
            int randomNumber = UnityEngine.Random.Range(0, 100);
            //We start with this to choose a random number between 0 and 100.
            for (int j = 0; j < objectsToSpawn.Count(); j++)
                // This loop goes through each thing in our objectsToSpawn array.
            {

                if ( randomNumber >= objectsToSpawn[j].minChance && randomNumber <= objectsToSpawn[j].maxChance)
                    // here we check if our randomNumber is less than or equal to our selected objects minChance and MaxChance, it will do this for each object in the list.
                {
                    objectToBeSpawned = objectsToSpawn[j].chunk;
                    if (objectsToSpawn[j].isWater == true)
                    // here we will check if the chunk to be spawned is in fact water.
                    {
                        waterChunk = true;
                    }
                    else
                    {
                        waterChunk = false;
                    }


                    // We then set objectsToSpawn to the number which fits inside this particular objects min and maxchances.
                    break;
                    // add break to make sure it doesn't keep running.
                }
            }
           
            float z = lastSpawnedObject.transform.position.z + lastSpawnedObject.transform.localScale.z;
            // sets position to spawn object next to lastSpawnedObject.transform on x scale.
            if (waterChunk == false)
            {
                if (lastSpawnedObject.name == "waterChunk(Clone)")
                {
                    
                    spawnPositionX = new Vector3(lastSpawnedObject.transform.position.x, UnityEngine.Random.Range(lastSpawnedObject.transform.position.y, yArea) - waterLevel, z);
                    //sets the spawnPosition to what we want it to be, here it is the right.
                }
                else
                {
                    spawnPositionX = new Vector3(lastSpawnedObject.transform.position.x, UnityEngine.Random.Range(lastSpawnedObject.transform.position.y, yArea), z);

                }
            }
            else
            {
                 spawnPositionX = new Vector3(lastSpawnedObject.transform.position.x, lastSpawnedObject.transform.position.y + waterLevel, z);

            }
            lastSpawnedObject = Instantiate(objectToBeSpawned, spawnPositionX, Quaternion.identity);
                //Spawns object, and sets are most recently spawned object to last spawned.
                widthObjects.Add(lastSpawnedObject);
                //Here we add all of our width objects spawned to the widthObjects list, we do this so we can spawn further objects next to these.
                cullingScript.chunks.Add(lastSpawnedObject);
                // Adding all spawned chunk, which here is under lastSpawnedObject, to our chunk list for culling.
                cullingScript.lastChunkObject = lastSpawnedObject;
                //Add the last spawned Object to our culling, playerTracker, script to see if we should contine generation for now.
                cullingScript.checkChunkDistance();
                //here we run the function to see the distance of the last chunk spawned from the player, for procedural generation.
                moveDownWidthObjectsList();
                //We add the most recent spawned width object and add it to the width item list. We then call moveDownWidthObjectsList to do, just that.

                numberOfObjectsToSpawn[onNumber]--;
            

            //Here we get the number of items to spawn by stating numberOfITemsToSpawn[OnNumber], which tells us the amount needed to spawn on whatever number the prior loop is on.
            // From there we spawn the item the loop is on in the list, and subtract 1 from numberOfItemsToSpawn, we locate it with OnNumber.

        }

    }
        void moveDownWidthObjectsList()
        {
            //This number is used to decide the amount we will spawn next to each object in our widthObjects.

            int i = -0;
            for (i = onWidthObjectsNumer; i < widthObjects.Count(); i++)
            {

                spawnWidthObjectAmount();
                //Here we are going through the list of width objects, and calling the function spawnWidthObjectAmount to spawn a certain amount next to each WidthObject.
                onWidthObjectsNumer++;
                //Here we get the number of items to spawn by stating numberOfITemsToSpawn[OnNumber], which tells us the amount needed to spawn on whatever number the prior loop is on.
                // From there we spawn the item the loop is on in the list, and subtract 1 from numberOfItemsToSpawn, we locate it with OnNumber.
            
            }


        }






    void spawnWidthObjectAmount()
    {
        bool waterChunk = false;
        Vector3 spawnPositionX;
        //Vector3 spawnPositionX;

        int spawnNumber = UnityEngine.Random.Range(xChunkMin, xChunkMax);
        //This number is used to decide the amount we will spawn next to each object in our widthObjects.
        GameObject lastWidthSpawnedObject = widthObjects[onWidthObjectsNumer];
        //When this function is ran, the object spawn it's first chunk off is based on the widthObject we are on, which is determined by moveDownWidthObjectsList for loop.
        int i = -0;
        for (i = spawnNumber; i != 0; i--)
            // if the number to spawn does not equal 0, run this loop and decrease i by 1.
        {



            int randomNumber = UnityEngine.Random.Range(0, 100);
            //We start with this to choose a random number between 0 and 100.
            for (int j = 0; j < objectsToSpawn.Count(); j++)
            // This loop goes through each thing in our objectsToSpawn array.
            {

                if (randomNumber >= objectsToSpawn[j].minChance && randomNumber <= objectsToSpawn[j].maxChance)
                // here we check if our randomNumber is less than or equal to our selected objects minChance and MaxChance, it will do this for each object in the list.
                {
                    objectToBeSpawned = objectsToSpawn[j].chunk;
                    if(objectsToSpawn[j].isWater == false){
                        waterChunk = false;
                    }
                    else
                    {
                        waterChunk = true;
                    }


                    // We then set objectsToSpawn to the number which fits inside this particular objects min and maxchances.
                    break;
                    // add break to make sure it doesn't keep running.
                }
            }
            //We set lastspawnedObject to the number of width object in the list we are on the object can spawn next to it.
            float x = lastWidthSpawnedObject.transform.position.x - lastWidthSpawnedObject.transform.localScale.x;
            // sets position to spawn object next to lastSpawnedObject.transform on x scale.
            if (waterChunk == false)
            {
                if (lastWidthSpawnedObject.name == "waterChunk(Clone)")
                {
                    spawnPositionX = new Vector3(x, UnityEngine.Random.Range(lastSpawnedObject.transform.position.y, yArea) - waterLevel, lastWidthSpawnedObject.transform.position.z);
                }
                else
                {
                    spawnPositionX = new Vector3(x, UnityEngine.Random.Range(lastSpawnedObject.transform.position.y, yArea), lastWidthSpawnedObject.transform.position.z);

                }
                //sets the spawnPosition to what we want it to be, here it is the right.
            }
            else
            {
                spawnPositionX = new Vector3(x, lastSpawnedObject.transform.position.y + waterLevel, lastWidthSpawnedObject.transform.position.z);

            }
            lastWidthSpawnedObject = Instantiate(objectToBeSpawned, spawnPositionX, Quaternion.identity);
            //Spawns object, and sets are most recently spawned object to last spawned. 
            cullingScript.chunks.Add(lastWidthSpawnedObject);
            // Adding all spawned chunk, which here is under lastWidthSpawnedObject, to our chunk list for culling.
            cullingScript.lastChunkObject = lastWidthSpawnedObject;
            //Add the last spawned Object to our culling, playerTracker, script to see if we should contine generation for now.
            cullingScript.checkChunkDistance();
            //here we run the function to see the distance of the last chunk spawned from the player, for procedural generation.
        }

        //Here we get the number of items to spawn by stating numberOfITemsToSpawn[OnNumber], which tells us the amount needed to spawn on whatever number the prior loop is on.
        // From there we spawn the item the loop is on in the list, and subtract 1 from numberOfItemsToSpawn, we locate it with OnNumber.
    }



        }

    




[System.Serializable]
public class chunkObjectPropetries
{
    public GameObject chunk;
    public int minChance;
    public int maxChance;

    public bool isWater;
}
