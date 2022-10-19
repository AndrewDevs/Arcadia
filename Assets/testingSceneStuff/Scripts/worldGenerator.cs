using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.AccessControl;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Mirror;
using Unity.Jobs;
using System.IO;
using UnityEngine.UI;
using System;
using UnityEngine.AI;

public class worldGenerator : NetworkBehaviour
{

    public GameObject player;

    public string seedFile = "worldSeed";
    public string worldDataFile = "worldData";
    public string worldName;

    public int renderDistance;
    public int trueRenderDistance = 5;
    public int chunk;
    public int onNumber;
    int worldNumber = 0;
    int calls = 0;
    int maxDistance = 200;

    public bool start = false;


    public Vector3 newChunkPosition = new Vector3(0, 0, 0);
    Vector3 chunkPos = new Vector3(0, 0, 0);
    Vector3 position = new Vector3(0, 0, 0);

    Quaternion rotation;
    
    bool water = false;

    float amountLeft;
    float xRounded;
    float xRemainder;
    float zRemainder;
    float zRounded;

    float lastX;
    float lastZ;
    public float worldSeed;
    float y;

    private IEnumerator coroutine;

    int generalT;
    int choose;

    public spawnedChunkInfo spawnedChunkHolder;

    NetworkConnection currentClient;

    [SyncVar]
    public Database itemDatabase;

    public GameObject databaseObject;

    public Collider areaCollider;

    public chunkObjectPropetrys[] chunkChances;

    public chunkTypes[] ChunkTypes;

    public List<Vector3> spawnedChunkPositions = new List<Vector3>();
    public List<spawnedChunkInfo> spawnedChunks = new List<spawnedChunkInfo>();
    public SyncList<playerInfo> players = new SyncList<playerInfo>();

    public List<spawnedEnviornmentInfo> localItemsToRemove = new List<spawnedEnviornmentInfo>();




    public void OnStart()
    {
        UnityEngine.Random.seed = System.Environment.TickCount;
        /*With enviornment spawning we edit Random.initState a lot, and that can cause the random.range accross worlds to be the same.
         To counteract that we add this line, which sets the random.seed to a truly random state and thus the initState is also changed. */
        start = true;
        if (File.Exists(Application.persistentDataPath + "/" + seedFile + ".dat"))
        {
            bool exists = false;

            worlds world = new worlds();

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + seedFile + ".dat", FileMode.Open);
            seedInfo savedSeedInfo = (seedInfo)bf.Deserialize(file);
            file.Close();
            //Open the file that contains the worlds list.
            foreach (worlds worldInstance in savedSeedInfo.worldInfo) //Seach in said worlds list for one that equals our world's name, and is thus our world.
            {
                if (worldInstance.worldName == worldName)
                {
                    world = worldInstance;
                    worldName = worldInstance.worldName;
                    exists = true;
                    break;

                }
                worldNumber++;

            }
            if (exists == true) //If this world does already exist then we will merely load the world seed.
            {
                worldSeed = world.worldSeed;
                databaseObject = GameObject.Find("Database(Clone)");
                itemDatabase = databaseObject.GetComponent<Database>();
            }
            else if (exists == false) //If the file with our worlds list in it does exist but this world does not exist yet.
            {
                worldName = "World " + worldNumber;
                worldSeed = UnityEngine.Random.Range(0, 100000);
                //Generate all components to the seed, as this is the first time this would has existed..


                databaseObject = GameObject.Find("Database(Clone)");
                itemDatabase = databaseObject.GetComponent<Database>();

                worlds worldInfo = new worlds();
                //Create a worlds variable that we can use to add to our savedSeedInfo's worlds list.

                worldInfo.worldName = worldName;
                worldInfo.worldSeed = worldSeed;

                //Set the necessary variables to our new world class so it can be serialized.

                savedSeedInfo.worldInfo.Add(worldInfo);
                //Finally add our data we have compiled above to the list.\

                BinaryFormatter Bf = new BinaryFormatter();
                FileStream dataFile = File.Create(Application.persistentDataPath + "/" + seedFile + ".dat");
                Bf.Serialize(dataFile, savedSeedInfo);
                file.Close();
                //And serialize the savedSeedInfo with our added world instance to it.


            }
            //Here we are opening the worldData file that already exists and serializing new chunk data to it, then closing it. Now we merely add to and already existant file, instead of completly remaking it.


            /*    XmlSerializer serializer = new XmlSerializer(typeof(seedInfo));
                //Create a XmlSerializer that takes the form of our chunkInfo class.
                StreamWriter translator = new StreamWriter(seedFile +  ".xml");
                //Create a StreamWriter that writes our data into a file that it will name "worldData.xml".
                serializer.Serialize(translator.BaseStream, savedSeedInfo);
                //This line actually takes our translator, and makes it serialize the class chunkInformation.
                translator.Close();
                //Stop translating/serializing.  */
        }
        else //If there is not a file that contains all the worlds.
        {

            worldName = "World " + worldNumber;

            //Generate all components to the seed.
            databaseObject = GameObject.Find("Database(Clone)");
            itemDatabase = databaseObject.GetComponent<Database>();

            seedInfo savedSeedInfo = new seedInfo();
            //Create a new seedInfo var so that we can create a list/fine.

            worldSeed = UnityEngine.Random.Range(0, 100000);

            worlds worldInfo = new worlds();

            worldInfo.worldSeed = worldSeed;
            worldInfo.worldName = "World " + worldNumber;
            //Set all the necessary variables to our list.

            savedSeedInfo.worldInfo.Add(worldInfo);
            //Finally add the data to an instance in the list.

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/" + seedFile + ".dat");
            bf.Serialize(file, savedSeedInfo);
            file.Close();
            //Create the file, serialize the seedInfo (savedSeedInfo) with all our need instance of the list in it, and close the file.
        }
    }


    void Update()
    {
        if (start == true)
        {
            if (isServer == true) //We add the isServer bool as we only want worldGeneration to run on the server for security reasons.
            {
                spawnChunks();
            }
        }
    }




    public void spawnChunks()
    {
        foreach (playerInfo player in players) //We add a for loop so this math etc. runs for each player on the server.
        {
            if (player.player != null)
            {
                Transform cameraObject = player.player.transform.GetChild(0);

                Camera cam = cameraObject.GetComponent<Camera>();

                float x = Mathf.Round(player.player.transform.position.x);
                //Round our player's transform.position.x to the nearest tenth.
                xRemainder = x % 15;
                if (xRemainder > 15)
                {
                    xRounded = x - xRemainder + 30;
                }
                else
                {
                    xRounded = x - xRemainder;
                }
                //This code rounds out given x to the nearest 15th. I don't understand how it works.
                float z = Mathf.Round(player.player.transform.position.z);
                //Round our player's transform.position.x to the nearest tenth.
                zRemainder = z % 15;
                if (zRemainder > 15)
                {
                    zRounded = z - zRemainder + 30;
                }
                else
                {
                    zRounded = z - zRemainder;
                }
                //This code rounds the given z to the nearest 15th. Again, I don't understand how it works.

                if(player.playerRenderDistance == 0) //If the render distance is 0, which is a glitch, then set it to 4 to resolve the issue.
                {
                    player.playerRenderDistance = 4;
                }

                for (int r = 0; r < player.playerRenderDistance; r++) //Loop for renderDistance. Render distance is local to the player - we access it through the player list.
                {
                    int renderSideMultiplier = 15 * r;

                    for (int i = 0; i < 3; i++)
                    {

                        if(r > 1)  //All this does is makes sure that, if we've already spawned forward chunks (which we do on r 1), to not do that again. We avoid it by increasing i to 1.
                        {
                            if(i == 0)
                            {
                                i = 1;
                            }
                        }

                        for (int j = 0; j < 10; j++) //Going through our render amount array that times our given chunk position by what is necessary to spawn a certain amount of chunks.
                        {
                            float Multiplier = 15 * j;
                            //This multiplier is sort-of like rendering distance.

                            Vector3 roundedPos = new Vector3();
                            Vector3 soonChunkPosition = new Vector3();
                            //This vector is just the player's position (roundedPos) multiplied by a certain amount in the forward direction of the camera.

                            if (i == 0) //Forward Chunks
                            {
                                roundedPos = new Vector3(xRounded, 0, zRounded);
                                soonChunkPosition = roundedPos + cameraObject.transform.forward * Multiplier;
                                //This vector is just the player's position (roundedPos) multiplied by a certain amount in the forward direction of the camera.
                            }
                            if (i == 1) //Right Chunks
                            {
                                roundedPos = new Vector3(xRounded, 0, zRounded);
                                roundedPos = roundedPos + cameraObject.transform.right * renderSideMultiplier;
                                soonChunkPosition = roundedPos + cameraObject.transform.forward * Multiplier;
                                //This vector is just the player's position (roundedPos) multiplied by a certain amount in the forward direction of the camera.
                            }
                            if (i == 2) //Left Chunks
                            {
                                roundedPos = new Vector3(xRounded, 0, zRounded);
                                roundedPos = roundedPos - cameraObject.transform.right * renderSideMultiplier;
                                soonChunkPosition = roundedPos + cameraObject.transform.forward * Multiplier;
                                //This vector is just the player's position (roundedPos) multiplied by a certain amount in the forward direction of the camera.
                            }


                            #region rounding the front-view chunk positions to spawn
                            float soonX = Mathf.Round(soonChunkPosition.x);
                            //Round our player's transform.position.x to the nearest tenth.
                            float sXRemainder = soonX % 15;
                            float sXRounded = new float();
                            if (sXRemainder > 15)
                            {
                                sXRounded = soonX - sXRemainder + 30;
                            }
                            else
                            {
                                sXRounded = soonX - sXRemainder;
                            }
                            //This code rounds the given x to the nearest 15th. Again, I don't understand how it works.

                            float soonZ = Mathf.Round(soonChunkPosition.z);
                            //Round our player's transform.position.x to the nearest tenth.
                            float sZRemainder = soonZ % 15;
                            float sZRounded = new float();
                            if (sZRemainder > 15)
                            {
                                sZRounded = soonZ - sZRemainder + 30;
                            }
                            else
                            {
                                sZRounded = soonZ - sZRemainder;
                            }
                            //This code rounds the given x to the nearest 15th. Again, I don't understand how it works.
                            #endregion
                            //This region just rounds the soonChunkPosition to 15 so chunks can spawn in a grid like fasion.

                            #region math for biome generation

                            float posX = new float();
                            float posZ = new float();

                            posZ = sZRounded + worldSeed;
                            posX = sXRounded + worldSeed;


                            float biomeValue = Mathf.PerlinNoise(posX / 400.2f, posZ / 400.3f);


                            if (biomeValue > 0 && biomeValue < 0.3)
                            {
                                chunk = ChunkTypes[0].type; //spawn a forest chunk. 

                            }
                            else if (biomeValue > 0.3 && biomeValue < 0.45)
                            {
                                chunk = ChunkTypes[2].type; //Spawn a plains chunk.

                            }
                            else if (biomeValue > 0.45 && biomeValue < 0.5)
                            {
                                chunk = ChunkTypes[6].type; //Spawn a redwood chunk.

                            }
                            else if (biomeValue > 0.5 && biomeValue < 0.6)
                            {
                                chunk = ChunkTypes[3].type; //Spawn a rock chunk.

                            }
                            else if (biomeValue > 0.6 && biomeValue < 0.7)
                            {
                                chunk = ChunkTypes[5].type; //Spawn a flower chunk.

                            }
                            else if (biomeValue > 0.7)
                            {
                                chunk = ChunkTypes[5].type; //Spawn a cave chunk.

                            }




                            #endregion


                            #region Y axis     

                            float mountain = Mathf.PerlinNoise(posX / 310.2f, posZ / 330.2f) * 280;
                            float smoothness = Mathf.PerlinNoise(posX / 730.2f, posZ / 730.2f);
                            float frequency = Mathf.PerlinNoise(posX / 330.2f, posZ / 330.2f) * 130;
                            float waveLength = Mathf.PerlinNoise(posX / 730.2f, posZ / 730.2f) * 500;
                            y = Mathf.PerlinNoise(mountain / waveLength, smoothness / waveLength) * frequency;


                            if (y < 18) //If Y is greater than -1 but less than 5, then it will be a water chunk.
                            {
                                chunk = ChunkTypes[4].type;
                                //Here we are assigning our chunk int to the water chunk type int, so we can spawn a water chunk.
                                y = 18;
                                //Change y to 18 as this is for a waterChunk and we want all waterChunks on the same level.
                            }
                            
                            /*
                            if(chunk == ChunkTypes[7].type)
                            {
                                int random = Mathf.RoundToInt(posX + posZ * y);
                                UnityEngine.Random.InitState(random);
                                int result = UnityEngine.Random.Range(1, 10);
                                if(result == 2)
                                {
                                    chunk = ChunkTypes[8].type;
                                    y = y - 35;

                                }
                            } */



                            #endregion


                            newChunkPosition = new Vector3(sXRounded, 0, sZRounded);
                            //Set the newChunkPosition, which is the position the chunk will be spawned at, with our Rounded floats in addition to our Added floats,3.
                            //0Y is determined elsewhere.
                            float distance = Vector3.Distance(newChunkPosition, player.player.transform.position);

                            if (distance < renderDistance)
                            {
                                checkIfSpawned(chunk, newChunkPosition, y, player.player);
                                //Check if a chunk has already been spawned in this position, and if not spawn one there.
                            }




                        }

                    }
                }
            }
        }
    }


    public void checkIfSpawned(int type, Vector3 chunkPosition, float y, GameObject playerSpawn)
    {

        if (spawnedChunkPositions.Contains(chunkPosition) == false) //We check the synced list (chunk positions) and our local list (spawnedChunkPositions) as sync lists have a tendency to be unreliable due to server lag.
        {
            spawnedChunkPositions.Add(chunkPosition);
            //Add this chunk position to our spawnedchunkPositions list as it has not been spawned and so we will not spawn another chunk in the same position.
            Vector3 chunkPos = new Vector3(chunkPosition.x, y, chunkPosition.z);
            //Create a vector3 named "chunkPos" and add our chunk position data to it plus the y, we do this as in our spawnedChunkPositions list we do not "Save" the y.
            float distance = Vector3.Distance(playerSpawn.transform.position, chunkPosition);
            if (distance < maxDistance)
            {
                spawnObject(type, chunkPos, chunkPosition, 0, playerSpawn);
            }
        }


    }
    [TargetRpc]
    public void TargetgoDownSpawnList(NetworkConnection client, GameObject chunkObject, int index, int type, bool saved)
    {
        areaCollider = chunkObject.GetComponent<Collider>();
        //Get the areaCollider of the chunk so we can spawn the enviornmentObject within the bounds of the chunk (i.e on the chunk). We do this here so we don't getComponent for every iteration in the loop.
        for (onNumber = 0; onNumber < ChunkTypes[type].numberOfItemsToSpawn.Count; onNumber++) //Here we are refrencing the list of chunk/biome types, [type] being the type we have chosen, and checking the number of item types to spawn and going through the list accordingly.
        {
            /*NOTE: onNumber is merely the index of the number of items we are supposed to count. So onNumber 0, when applied to the list
            as an index, may return as 24, so we have 24 items to spawn etc. */
            spawnAmount(chunkObject, index, type, onNumber,saved,  client);
        }

    }


    public void spawnAmount(GameObject chunkObject, int index, int type, int onNumber, bool saved, NetworkConnection client)
    {

        /*Refrencing the specific chunk/biome type we have chosen, then checking the number of specific items to spawn, *then* checking the instance on this list [onNumber] to 
         dictate how many times to go through this loop. Below */

        for (int i = ChunkTypes[type].numberOfItemsToSpawn[onNumber]; i > 0; i--)
        {
            #region setting envObject position, rotation, creating holder, and spawning envObject

            int random = Mathf.RoundToInt(i + chunkObject.transform.position.x + chunkObject.transform.position.z + type * 20 + onNumber * 25);
            UnityEngine.Random.InitState(random);

            position = new Vector3(UnityEngine.Random.Range(areaCollider.bounds.max.x, areaCollider.bounds.min.x), chunkObject.transform.position.y + itemDatabase.items[ChunkTypes[type].itemsToSpawn[onNumber]].transform.localScale.y, UnityEngine.Random.Range(areaCollider.bounds.max.z, areaCollider.bounds.min.z));

            position.x = (float)Math.Round((double)position.x, 1);
            position.y = (float)Math.Round((double)position.y, 1);
            position.z = (float)Math.Round((double)position.z, 1);


            float distance = Vector3.Distance(chunkObject.transform.position, client.identity.gameObject.transform.position);
            //This float will be used to check the distance between the player and a specific chunk (the one we are spawning items on), if the distance is too far then we will not spawn it for performance reasons.

            rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
            //Set the rotation as something random.`

            enviornmentInfo infoHolder = new enviornmentInfo(); //Create an enviornmentInfo custom class type, so we can add our enviornmentObject's info in it for adding to the enviornment list on the chunk instance.
            infoHolder.x = position.x;
            infoHolder.y = position.y;
            infoHolder.z = position.z;
            infoHolder.rX = rotation.eulerAngles.x;
            infoHolder.rY = rotation.eulerAngles.y;
            infoHolder.rZ = rotation.eulerAngles.z;


            infoHolder.id = ChunkTypes[type].itemsToSpawn[onNumber];
            //Add all of our data to thus custom class variable type.
            Vector3 objectPosition = new Vector3(); //Create a new, local, objectPosition vector3 for spawning additional items below.
            int id = new int(); //Create a new int named id for usage of spawning additional items, if necessary for the object we are already spawning, below.

            if (i == 1 && onNumber == ChunkTypes[type].numberOfItemsToSpawn.Count() - 1) //If this function is done spawning a chunk.
            {

                if (distance < maxDistance)
                {
                    spawnItems(client, ChunkTypes[type].itemsToSpawn[onNumber], position, rotation, chunkObject, index, saved);
                    //CmdaddSpawnItemToServer(ChunkTypes[type].itemsToSpawn[onNumber], position, rotation, index); //then set the bool to true, so it will cull ISSUE HERE
                    break;
                }

            }
            else
            {
                //this spawns the object which we have chosen in our chunks spawn enviornment list...everything below is stuff to add if it is a tree, bush, etc.
                if (distance < maxDistance)
                {
                    //CmdaddSpawnItemToServer(ChunkTypes[type].itemsToSpawn[onNumber], position, rotation, index);
                    spawnItems(client, ChunkTypes[type].itemsToSpawn[onNumber], position, rotation, chunkObject, index, saved);
                }

            }

            #endregion

            #region spawn accessory items
            //From here on out this is where we will spawn accessory items to the original enviornmentObject (or envObject). We base this off name mainly.
            if (itemDatabase.items[ChunkTypes[type].itemsToSpawn[onNumber]].name == "Tree Trunk 2")
            {
                Vector3 largeFoliagePos = new Vector3();

                largeFoliagePos.x = position.x;
                largeFoliagePos.y = position.y + itemDatabase.items[2].transform.localScale.y;
                largeFoliagePos.z = position.z;
                id = 2;

                spawnItems(client, id, largeFoliagePos, rotation, chunkObject, index, saved);

                objectPosition.x = position.x;
                objectPosition.y = largeFoliagePos.y + itemDatabase.items[1].transform.localScale.y / 2;
                objectPosition.z = position.z;
                id = 1;
            }
            if (itemDatabase.items[ChunkTypes[type].itemsToSpawn[onNumber]].name == "Medium Tree Trunk")
            {
                objectPosition.x = position.x;
                objectPosition.y = position.y + itemDatabase.items[5].transform.localScale.y * 1.5f;
                objectPosition.z = position.z;
                id = 5;
            }
            if (itemDatabase.items[ChunkTypes[type].itemsToSpawn[onNumber]].name == "BigTreeTrunk")
            {
                objectPosition.x = position.x;
                objectPosition.y = position.y + itemDatabase.items[18].transform.localScale.y * 1.5f;
                objectPosition.z = position.z;
                id = 18;
            }
            if (itemDatabase.items[ChunkTypes[type].itemsToSpawn[onNumber]].name == "bushStalk")
            {
                objectPosition.x = position.x;
                objectPosition.y = position.y + itemDatabase.items[19].transform.localScale.y * 1.5f;
                objectPosition.z = position.z;
                id = 20;
                //spawnFruit(rotation, index, objectPosition); //FRUIT SPAWNING IS PROBLEMATIC..RESOLVE LATER.
                //UnityEngine.Debug.Log("X: " + objectPosition.x + " Y: " + objectPosition.y + " Z: " + objectPosition.z);

            }
            if (itemDatabase.items[ChunkTypes[type].itemsToSpawn[onNumber]].name == "redWoodTrunk")
            {
                objectPosition.x = position.x;
                objectPosition.y = position.y + itemDatabase.items[19].transform.localScale.y * 1.5f;
                objectPosition.z = position.z;
                id = 26;

            }
            if (itemDatabase.items[ChunkTypes[type].itemsToSpawn[onNumber]].name == "pineTrunk")
            {
                objectPosition.x = position.x;
                objectPosition.y = position.y + itemDatabase.items[19].transform.localScale.y * 1.5f;
                objectPosition.z = position.z;
                id = 27;

            }



            if (id != 0)
            {
                if (i == 1 && onNumber == ChunkTypes[type].numberOfItemsToSpawn.Count() - 1 && id != 20) //If this is the end of the loop, and thus we are done spawning and it is not spawning a bush foliage as we must then spawn berries, we will break the loop.
                {
                    //Here we will finally spawn the accessory item to our enviornmentObject. These will *only* run if the last item is one with an accessory, which seems to not be very common for whatever reason.

                    if (distance < maxDistance)
                    {
                        spawnItems(client, id, objectPosition, rotation, chunkObject, index, saved);
                        //CmdaddSpawnItemToServer(id, objectPosition, rotation, index);
                    }

                    break;
                }
                else
                {
                    //Here we will finally spawn the accessory item to our enviornmentObject.

                    if (distance < maxDistance)
                    {
                        spawnItems(client, id, objectPosition, rotation, chunkObject, index, saved);
                        //CmdaddSpawnItemToServer(id, objectPosition, rotation, index);
                    }

                }
            }
            #endregion

        }
    }


    public void spawnFruit(Quaternion rotation, int index, Vector3 objectPosition, NetworkConnection client)
    {

        GameObject foliageObject = itemDatabase.items[20];

        for (int i = 20; i > 0; i--) {

            areaCollider = foliageObject.GetComponent<BoxCollider>(); //Get the area collider of our foliageObject that is passed as a paramater, so we can spawn within the bounds of the object (i.e on the object.)

            Vector3 position = new Vector3(); //Create new Vector3 to use as the berry objects position, it is set below.

            int chance = UnityEngine.Random.Range(1, 5); //Decide the chance on what side the berry object will spawn randomly (using random.range).

            /*All the below region does is set the berry to a specific side of the object depending on the return of our chance int, it does by getting the
            foliage object's scale and its own gameObject's scale. The math is relative to what the position we are spawning on is. */
            #region setting berry position based on the chance int above
            if (chance == 1) {

                float berryPositionX = (UnityEngine.Random.Range(objectPosition.x - (foliageObject.transform.localScale.x / 2), objectPosition.x + (foliageObject.transform.localScale.x / 2)));
                float berryPositionZ = (UnityEngine.Random.Range(objectPosition.z - (foliageObject.transform.localScale.z / 2), objectPosition.z + (foliageObject.transform.localScale.z / 2)));
                position.y = objectPosition.y + itemDatabase.items[21].transform.localScale.y * 3f;
                position.x = berryPositionX;
                position.z = berryPositionZ;
            }
            else if (chance == 2)
            {
                float berryPositionY = (UnityEngine.Random.Range(objectPosition.y - (foliageObject.transform.localScale.y / 2), objectPosition.y + (foliageObject.transform.localScale.y / 2)));
                float berryPositionZ = (UnityEngine.Random.Range(objectPosition.z - (foliageObject.transform.localScale.z / 2), objectPosition.z + (foliageObject.transform.localScale.z / 2)));
                position.y = berryPositionY;
                position.x = objectPosition.x + itemDatabase.items[21].transform.localScale.x * 3f;
                position.z = berryPositionZ;
            }
            else if (chance == 3)
            {
                float berryPositionY = (UnityEngine.Random.Range(objectPosition.y - (foliageObject.transform.localScale.y / 2), objectPosition.y + (foliageObject.transform.localScale.y / 2)));
                float berryPositionX = (UnityEngine.Random.Range(objectPosition.x - (foliageObject.transform.localScale.x / 2), objectPosition.x + (foliageObject.transform.localScale.x / 2)));
                position.y = berryPositionY;
                position.x = berryPositionX;
                position.z = objectPosition.z + itemDatabase.items[21].transform.localScale.z * 3f;
            }
            else if (chance == 4)
            {
                float berryPositionY = (UnityEngine.Random.Range(objectPosition.y - (foliageObject.transform.localScale.y / 2), objectPosition.y + (foliageObject.transform.localScale.y / 2)));
                float berryPositionX = (UnityEngine.Random.Range(objectPosition.x - (foliageObject.transform.localScale.x / 2), objectPosition.x + (foliageObject.transform.localScale.x / 2)));
                position.y = berryPositionY;
                position.x = berryPositionX;
                position.z = objectPosition.z + itemDatabase.items[21].transform.localScale.z * 3f;
            }
            else if (chance == 5)
            {
                float berryPositionY = (UnityEngine.Random.Range(objectPosition.y - (foliageObject.transform.localScale.y / 2), objectPosition.y + (foliageObject.transform.localScale.y / 2)));
                float berryPositionZ = (UnityEngine.Random.Range(objectPosition.z - (foliageObject.transform.localScale.z / 2), objectPosition.z + (foliageObject.transform.localScale.z / 2)));
                position.y = berryPositionY;
                position.x = objectPosition.x - itemDatabase.items[21].transform.localScale.x * 3f;
                position.z = berryPositionZ;
            }
            #endregion

            #region spawn berry          

            //CmdaddSpawnItemToServer(21, position, rotation, index);
            //finally spawn the berry object. We do not let the berry bush have the finishedSpawning bool true as it causes issues with culling.

            #endregion

        }

    }

    public void culling()
    {
        int localRender = maxDistance;

        for (int j = 0; j < spawnedChunks.Count(); j++) //For each chunk in our spawnedChunks list.
        {
            bool destroy = false; //We put the bool in here so it is set to false every time the loop "updates".

            foreach (playerInfo player in players) //Foreach player that exists on the server.
            {
                if (player.player != null) //If this player instance is not null, and thus exists then go ahead.
                {
                    float distance = 0f; //Declare the float distance here so it can be used below.
                    if (spawnedChunks[j].chunkObject != null)
                    {
                        /*
                            Transform cameraObj = player.player.transform.GetChild(0);
                            Camera camera = cameraObj.GetComponent<Camera>();

                            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
                            if (GeometryUtility.TestPlanesAABB(planes, spawnedChunks[j].chunkObject.GetComponent<Collider>().bounds))
                            {
                                destroy = false;
                                break;
                            }
                            else
                            {
                                destroy = true;
                            }
                        */
                        
                        
                        distance = Vector3.Distance(spawnedChunks[j].chunkObject.transform.position, player.player.transform.position); //Get the distance of each player relative to the chunk we are on (spawnedChunks[j].

                        if (distance > renderDistance) // If the chunk distance is greater then the render distance, then we will set destroy to true. If the else statement does not run the chunk will be destroyed.
                        {
                            destroy = true;
                            //Set destroy to true so it will be destroyed.
                        }
                        else //If the chunk distance is not greater than the render distance then the chunk will *not* be destroyed, as a player is near it; we make sure it will not be destroyed by adding a break so true is not set again.
                        {
                            destroy = false; //Set destroy to false as it will not be destroyed.
                            break; //Break the loop as there is a player close to the chunk.
                        } 
                        
                    }
                }
                else //If the player is null, and thus has left the game and does not exist, then we will remove them from this list.
                {
                    players.Remove(player); //Remove the empty player instance.
                    break;
                }
            }
            foreach (playerInfo localPlayer in players)
            {
                if (localPlayer.player != null) //If the player we are looking to check distance on is not null, and is this still in the server.
                {
                    float distance = Vector3.Distance(spawnedChunks[j].chunkObject.transform.position, localPlayer.player.transform.position); //Get the distance of each player relative to the chunk we are on (spawnedChunks[j].

                    NetworkIdentity netId = localPlayer.player.GetComponent<NetworkIdentity>();
                    //Getting the player's networkidentity so we can pass get the connection for the below targetRpc.

                    if (distance > localRender)
                    {
                        TargetlocalCulling(netId.connectionToClient, spawnedChunks[j].chunkObject, true);
                        //Call the targetRpc that will do the local culling.
                    }
                    else
                    {
                        TargetlocalCulling(netId.connectionToClient, spawnedChunks[j].chunkObject, false);
                        //Call the targetRpc that will do the local culling.
                    }
                }
                else //If the player that we wish to check distance on is null, and has thus left the server, we will remove from from our players list and break the loop. We break the loop as we have edited the list it uses.
                {
                    players.Remove(localPlayer);
                    break;
                }

            }
            if (destroy == true) //If we are done with our above loop and there is no player close to the chunk, then we will go ahead and destroy it.
            {
                RpcdestroyEnviornment(spawnedChunks[j].chunkObject);
                //Here we are calling an rpc to destroy the chunks *local* enviornment, as they have access to it - we pass the chunkObject to access its chunkScript in the rpc.


                Vector3 chunkPosition = new Vector3();
                //Create a new vector3 to store our spawnedchunks[j] x and z position in.
                chunkPosition.x = spawnedChunks[j].x;
                chunkPosition.z = spawnedChunks[j].z;
                //Set those floats to our vector3's.
                //Remove that poisition from our chunkPositions list so we can spawn a new chunk there.
                if (spawnedChunks[j].deleted == false) //If this chunk has yet to be deleted.
                {
                    spawnedChunks[j].deleted = true;
                    //Set this chunk deleted to true, as it is going to be deleted.
                    NetworkIdentity netID = spawnedChunks[j].chunkObject.GetComponent<NetworkIdentity>();
                    //Get the netID of the chunk, this is how we locate, and pass it as a paramater to our destroyObject function.
                    spawnedChunks.Remove(spawnedChunks[j]);
                    //Remove this chunk from our spawnedChunks list.
                    spawnedChunkPositions.Remove(chunkPosition);
                    //Remove this chunk position from our spawnedChunkPositions list as it is going to be deleted.


                }
            }
        }
    }

    [ClientRpc]
    public void RpcdestroyEnviornment(GameObject chunk) //We use an rpc to destroy the enviornment as the enviornment *only* exists locally - so the server does not know anything about these objects (including their existince!)
    {
        chunkScript script = chunk.GetComponent<chunkScript>();
        //Get the chunkScript from our chunk we are going to destroy to we can call the destroyEnviornmentFunction.
        script.destroyEnviornment();
        //Call the function that will finally destroy the enviornment (As the name entails).

    }


    public void save(int chunkType, Vector3 position, spawnedChunkInfo chunkInstance)
    {
        saveInfo chunkInformation = new saveInfo();       

        chunkInfo holderForSaving = new chunkInfo();

        /*chunkInformation is of the type saveInfo, which has a general list (chunks) that we will serialize. This list is made up of...chunkinfos. This we use
          holderForSaving to add our specific chunks info to, then we add the chunk to chunkInformation for proper serialization.*/

        holderForSaving.x = position.x;
        holderForSaving.y = position.y;
        holderForSaving.z = position.z;
        holderForSaving.chunkType = chunkType;
        //Add our chunks info to our holderForSaving so it may be serialized...this info is passed as a paramater and located in the playerMovement Script.

        chunkInformation.chunks.Add(holderForSaving);
        //Add the holderForSaving to chunkInfo as chunkInfo. Read why above.

        float originalRegion = position.x + position.z;
        float region = 0;
        float roundedRegion = originalRegion % 100;
        if (roundedRegion > 100)
        {
            region = originalRegion - roundedRegion + 200;
        }
        else
        {
            region = originalRegion - roundedRegion;
        }
        //Figuring out the "region" the chunk is, this just rounds to the nearest 250 - I don't understand the technacalities of it. This is so we can save the world in regions rather than one big file.


        if (File.Exists(Application.persistentDataPath + "/" + worldName + "/" + region + chunkType + ".dat")) //If a worldData file exists for this world.
        {
            bool exists = false;
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + worldName + "/" + region + chunkType + ".dat", FileMode.Open);
            saveInfo savedChunkInfo = bf.Deserialize(file) as saveInfo;
            file.Close();
            //And then we will deserialize the saveInfo section of the file, so we can use it to locate our chunk, remove data, and add whatever data we want.

            for (int i = 0; i < savedChunkInfo.chunks.Count; i++) //Go through our saved files chunk Data list.
            {
                if (savedChunkInfo.chunks[i].x == holderForSaving.x && savedChunkInfo.chunks[i].z == holderForSaving.z) //If this chunk has already been saved.
                {
                    for(int p = 0; p < savedChunkInfo.chunks[i].chunkItems.Count; p++) //for each instance in chunk items.
                    {

                        for (int k = 0; k < chunkInstance.removeSerializationObjects.Count; k++) //for each instance in removeSerializationObjects.
                        {
                            if (savedChunkInfo.chunks[i].chunkItems[p].id == chunkInstance.removeSerializationObjects[k].id && savedChunkInfo.chunks[i].chunkItems[p].x == chunkInstance.removeSerializationObjects[k].x && savedChunkInfo.chunks[i].chunkItems[p].y == chunkInstance.removeSerializationObjects[k].y && savedChunkInfo.chunks[i].chunkItems[p].z == chunkInstance.removeSerializationObjects[k].z)
                            {
                                //If they match...
                                savedChunkInfo.chunks[i].chunkItems.Remove(savedChunkInfo.chunks[i].chunkItems[p]); //Remove the instance from our file as it has been removed and we...consequently don't want it spawned again.                            
                                break; 
                            }
                        }
                        /* here we just check if any of our removeSerializationObjects (item's we've removed from the chunk) are in our file. 
                            If they are then we wish to remove them. */
                    }

                    for (int j = 0; j < chunkInstance.serializationObjects.Count(); j++) //Go through the list on our chunkInstance, the chunkinstance is just the chunk instance on the spawnedChunks server list. Passed from playerMovement.
                    {
                        spawnedEnviornmentInfo envInf = new spawnedEnviornmentInfo();
                        //envIf class here used to add objects to our holderForSaving.itemsToAdd list.              

                        envInf.x = chunkInstance.serializationObjects[j].x;
                        envInf.y = chunkInstance.serializationObjects[j].y;
                        envInf.z = chunkInstance.serializationObjects[j].z;
                        envInf.id = chunkInstance.serializationObjects[j].id;
                        envInf.rX = chunkInstance.serializationObjects[j].rX;
                        envInf.rY = chunkInstance.serializationObjects[j].rY;
                        envInf.rZ = chunkInstance.serializationObjects[j].rZ;

                        envInf.itemName = chunkInstance.serializationObjects[j].itemName;
                        //Compile the data of this particular enviornmentInformation into our envInf.

                        savedChunkInfo.chunks[i].chunkItems.Add(envInf);
                        //Add this specific savedChunkInfo to our saved chunk Items (items on the chunk) list.

                    }

                    exists = true;
                    break;

                }
            }

            if (exists == false)
            {
                savedChunkInfo.chunks.Add(holderForSaving);

            }

            XmlSerializer serializer = new XmlSerializer(typeof(saveInfo));
            //Create a XmlSerializer that takes the form of our chunkInfo class.
            StreamWriter translator = new StreamWriter(worldName + ".xml");
            //Create a StreamWriter that writes our data into a file that it will name "worldData.xml".
            serializer.Serialize(translator.BaseStream, savedChunkInfo);
            //This line actually takes our translator, and makes it serialize the class chunkInformation.
            translator.Close();
            //Stop translating/serializing. 


            BinaryFormatter Bf = new BinaryFormatter();
            FileStream file0 = File.Open(Application.persistentDataPath + "/" + worldName + "/" + region + chunkType + ".dat", FileMode.Open);
            Bf.Serialize(file0, savedChunkInfo);
            file0.Close();
            //Here we are opening the worldData file that already exists and serializing new chunk data to it, then closing it. Now we merely add to and already existant file, instead of completly remaking it.

        }
        else //if a save file does not exist for this world
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/" + worldName);
            //Create a folder with the name of our worldname.
            BinaryFormatter Bf = new BinaryFormatter();
            FileStream file0 = File.Create(Application.persistentDataPath + "/" + worldName + "/" + region + chunkType + ".dat");
            //Creates file within the folder we created above.
            Bf.Serialize(file0, chunkInformation);
            file0.Close();

            XmlSerializer serializer = new XmlSerializer(typeof(saveInfo));
            //Create a XmlSerializer that takes the form of our chunkInfo class.
            StreamWriter translator = new StreamWriter(worldName + ".xml");
            //Create a StreamWriter that writes our data into a file that it will name "worldData.xml".
            serializer.Serialize(translator.BaseStream, chunkInformation);
            //This line actually takes our translator, and makes it serialize the class chunkInformation.
            translator.Close();
            //Stop translating/serializing.




        }
    }



    [TargetRpc]
    public void TargetlocalCulling(NetworkConnection client, GameObject chunk, bool cull) //This function will do culling locally on a specific client, we do it this way because we only want the effects locally, on one specific client.
    {
        chunk.SetActive(true);
        //Set the chunk to true so we can access the chunkScript component.
        chunkScript script = chunk.GetComponent<chunkScript>();
        //Access the chunkScript component so we can access the spawned boolean and the chunks enviornment list, we will use the enviornment list to destroy said enviornment locally.

        if (cull == true) //If, when we called this function, we want to cull.
        {
            script.spawned = false; //Set spawned to false, as we are about to "despawn" (we really just disable) the enviornment.

            foreach (GameObject envObject in script.enviornmentObjects) //Loop through each enviornment object in the script component we got above.
            {
                Destroy(envObject); //Set each enviornmentObject to false.
            }
            chunk.SetActive(false); //Then set the chunk, which the enviornment was ontop of, to false when we are done "despawning" the enviornment (we really just disable the enviornment).
        }
        else if (cull == false && script.spawned == false) //If we do not want to cull, and the chunk does not have a spawned (active) enviornment.
        {
            chunk.SetActive(true); //Set the chunk to true, as we are re-spawning it and everything on it (remember, just activating it in reality, re-spawning just sounds better).

            script.OnStartClient(); //Set each enviornmentObject to false.           
        }
    }


    /*The below function is how all enviornments are spawned. Once this script has spawned the chunk, the chunk has a script (chunkScript) attached to it that will, OnClientStart, call this function through a command to
      spawn the enviornment ontop of that chunk. */
    public void SpawnEnviornment(GameObject chunkObject, int type, NetworkConnection client)
    {
        culling(); //Here we are going to cull chunks before we spawn any new ones. This is to avoid ArgumentOutOfRangeExceptions happening with the spawnedChunks list, as we use it to add enviornment objects to instances etc.

        spawnedChunkInfo holder = new spawnedChunkInfo(); //Create a holder in the form of the custom class spawnedChunkInfo so we can find it on our spawnedChunks list.
        holder.chunkType = type;
        holder.chunkObject = chunkObject;
        holder.x = chunkObject.transform.position.x;
        holder.z = chunkObject.transform.position.z;
        int index; //Declare an int index here so it can be used in the loop.

        bool exists = false; //This bool will be used to figure out if the chunk has been saved previously.


        float originalRegion = chunkObject.transform.position.x + chunkObject.transform.position.z;
        float region = 0;
        float roundedRegion = originalRegion % 100;
        if (roundedRegion > 100)
        {
            region = originalRegion - roundedRegion + 200;
        }
        else
        {
            region = originalRegion - roundedRegion;
        }
        //Figuring out the "region" the chunk is in, this just rounds to the nearest 250 - I don't understand the technacalities of it.

        if (File.Exists(Application.persistentDataPath + "/" + worldName + "/" + region + type + ".dat")) //If a worldData file exists for this world, then we will deserialize it so we can look through it.
        {

            int saveIndex = 0; //This int will be used to locate the specific chunk instance in our world data file when we are finally loading in the chunk.

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + worldName + "/" + region + type + ".dat", FileMode.Open);
            saveInfo savedChunkInfo = bf.Deserialize(file) as saveInfo;
            file.Close();

            for (int i = 0; i < savedChunkInfo.chunks.Count; i++) //This look goes through every chunk instance in our worldData file, under the name of savedChunkInfo.
            {
                if (savedChunkInfo.chunks[i].x == chunkObject.transform.position.x && savedChunkInfo.chunks[i].z == chunkObject.transform.position.z) //If the chunk instance we are on is equal to our chunkObject's position, then we will go ahead and load it as it has been saved (and thus edited) previously.
                {
                    exists = true; //Set exists to true so below in this function we can load the selected chunk data in.
                    saveIndex = i;
                    break;
                }

            }


            for (int i = 0; i < spawnedChunks.Count; i++) //Go through each instance in spawnedChunks, as we are looking for the chunk in spawnedChunks for the list index so we can associate the spawned enviornment with the chunk instance in the list.
            {
                if (spawnedChunks[i].x == holder.x && spawnedChunks[i].z == holder.z) //If we have found our chunk, through comparing x and z values.
                {
                    index = i; //Set the index to the matched index in the list.

                    if (exists == false) //If the chunk has not been saved (and thus exists = false), then we will just spawn it based on seed etc.
                    {
                        TargetgoDownSpawnList(client, chunkObject, index, type, false); //Now we will call our goDownSpawnList function, which spawns the enviornment.
                        break; //Break the loop.
                    }
                    else if (exists == true) //Otherwise, if this chunk has been saved prior then we will load the chunk in.
                    {

                        foreach (spawnedEnviornmentInfo envObject in savedChunkInfo.chunks[saveIndex].chunkItems)
                        {
                            Quaternion rotation = Quaternion.Euler(envObject.rX, envObject.rY, envObject.rZ);
                            Vector3 envPosition = new Vector3(envObject.x, envObject.y, envObject.z);
                            TargetLoadAddItems(client, envObject.id, envPosition, rotation, chunkObject, index);
                            //We use TargetLoadAddItems as we need to spawn the itemsToAdd locally for the client *only*, so we use a TargetFunction and *then* call spawnItems.

                        }
                        TargetgoDownSpawnList(client, chunkObject, index, type, true);
                        break;
                    }
                }
            }

        }
        else //If there is not a saved worldData file for this world.
        {
            for (int i = 0; i < spawnedChunks.Count; i++) //Go through each instance in spawnedChunks, as we are looking for the chunk in spawnedChunks for the list index so we can associate the spawned enviornment with the chunk instance in the list.
            {
                if (spawnedChunks[i].x == holder.x && spawnedChunks[i].z == holder.z) //If we have found our chunk, through comparing x and z values.
                {
                    index = i; //Set the index to the matched index in the list.

                    TargetgoDownSpawnList(client, chunkObject, index, type, false); //Now we will call our goDownSpawnList function, which spawns the enviornment.
                    break; //Break the loop.


                }
            }
        }
    }


    public void spawnObject(int chunkType, Vector3 position, Vector3 originalPosition, int saveIndex, GameObject playerSpawn) //Save index is only used when we are loading a chunk, outside of that just set it to 0 as it isin't used and thus has no effect.
    {
        #region spawning object
        GameObject spawnObj = Instantiate(ChunkTypes[chunkType].Chunk, position, Quaternion.identity);
        NetworkServer.Spawn(spawnObj); //Spawn the object, based on the given information above, onto the server.
        #endregion

        #region adding chunk to list, spawning enviornment, start Coroutine for mobs (not spawned yet)
        spawnedChunkInfo holder = new spawnedChunkInfo(); //Create a holder in the form of the custom class spawnedChunkInfo so it can be added to our spawnedChunks list.
        holder.chunkObject = spawnObj;
        holder.chunkType = chunkType;
        holder.x = position.x;
        holder.y = position.y;
        holder.z = position.z;
        holder.spawnedByPlayer = playerSpawn;
        //Add the data to the holder.

        spawnedChunks.Add(holder);
        //Add the holder to our list.
        int spawnedChunksIndex = spawnedChunks.IndexOf(holder);
        //Get the index of our holder in the spawnedChunks list so we can add the chunkObject after spawning it. We spawn it before so the chunkScript runs after it has been added to the list.    

        //Here we are getting the index of the chunk we have just spawned, and added to our spawnedChunks list, so we can successfully associate the spawned enviornment Objects with that chunk/list instance.
        spawnedChunkInfo emptyChunkHolder = new spawnedChunkInfo();
        spawnedChunkHolder = emptyChunkHolder; //We create a new spawnedChunkInfo class type or whatever and set it to spawnedChunkHolder to empty out spawnedChunkHolder so it does not just add the same info/data every time (this was an issue idk why).

        #endregion
    }

    [Command(requiresAuthority = false)]
    public void CmdaddSpawnItemToServer(int type, Vector3 position, Quaternion rotation, int index, NetworkConnectionToClient conn = null)
    {
        #region Create holder for our custom variable type so we can add all of our enviornment object's data to our list server-side *before* we spawn it
        //Create a custom variable, based on our custom class, so we can add this enviornment object's information/data to our spawnedChunk's enviornment list. Below.
        spawnedEnviornmentInfo spawnedEnv = new spawnedEnviornmentInfo();


        position.x = (float)Math.Round((double)position.x, 2);
        position.y = (float)Math.Round((double)position.y, 2);
        position.z = (float)Math.Round((double)position.z, 2);


        spawnedEnv.id = type;
        spawnedEnv.x = position.x;
        spawnedEnv.y = position.y;
        spawnedEnv.z = position.z;
        spawnedEnv.rX = rotation.eulerAngles.x;
        spawnedEnv.rY = rotation.eulerAngles.y;
        spawnedEnv.rZ = rotation.eulerAngles.z;

        spawnedEnv.itemName = type + " " + position;
        //Compile all the enviornmentObject's info/data into the custom variable.
        GameObject chunkObj = spawnedChunks[index].chunkObject;
        //spawnedChunks[index].enviornmentInformation.Add(spawnedEnv);
        //Add the custom variable, based on a custom class, to our chunk's enviornmentInformation list. We locate the chunk based on the passed index.

        //Here we are getting the chunk Object so we can grab the script in the below RPC, as such, we pass it as a paramater.
        #endregion

        #region throttling
        //CODE FOR THROTTLING TARGENTSPAWNITEMS CALLS, MAY BE USEFUL IN FUTURE.

        /*  if (distance < maxDistance)
          {

                  if (calls > 8000) // if the calls are greater than 9000, we want to throttle.
                  {
                      coroutine = throttle(client, type, position, rotation, index, finishedSpawning, chunkObj);
                      StartCoroutine(coroutine);
                      calls--;


                  }
                  else //If they are not greater than 800, we'll just call the targetRpc and not throttle as we don't need to.
                  {
                      TargetspawnItems(client, type, position, rotation, finishedSpawning, chunkObj, index);
                      calls++;
                  }


          }
          */




        #endregion


    }

    public IEnumerator throttle(NetworkConnection client, int type, Vector3 position, Quaternion rotation, int index, GameObject chunk)
    {
        yield return new WaitForSeconds(0.2f);
        spawnItems(client, type, position, rotation, chunk, index, false);

    }

    [TargetRpc]
    public void TargetLoadAddItems(NetworkConnection client, int type, Vector3 position, Quaternion rotation, GameObject chunk, int index)
    {
        spawnItems(client, type, position, rotation, chunk, index, true);
    }


    [TargetRpc]
    public void TargetAddLocalItemRemove(NetworkConnection client, int type, Vector3 position)
    {
        spawnedEnviornmentInfo obj = new spawnedEnviornmentInfo();
        obj.id = type;
        obj.x = (float)Math.Round((double)position.x, 1);
        obj.y = (float)Math.Round((double)position.y, 1);
        obj.z = (float)Math.Round((double)position.z, 1);
        
        localItemsToRemove.Add(obj);
        //Here we are adding the items name to a *local*, targeted, list so that when this client attempts to spawn the chunk we can not spawn this item, as it has been removed.
    }

    public void spawnItems (NetworkConnection client, int type, Vector3 position, Quaternion rotation, GameObject chunk, int index, bool saved)
    {
        #region spawn the item/object     

        //Here we just look through the localItemsToRemove list, and make sure the item we are going to spawn is not on that list. If it is, then we return and won't run the spawnItems function.
        for (int i = 0; i < localItemsToRemove.Count(); i++)
        {
            if (saved == true) //If the chunk we are spawning our item on has been saved.
            {
                spawnedEnviornmentInfo obj = new spawnedEnviornmentInfo();

                obj.x = (float)Math.Round((double)position.x, 1);
                obj.y = (float)Math.Round((double)position.y, 1);
                obj.z = (float)Math.Round((double)position.z, 1);

                if (localItemsToRemove[i].x <  obj.x + 1 && localItemsToRemove[i].x > obj.x - 1 && localItemsToRemove[i].y < obj.y + 1 && localItemsToRemove[i].y > obj.y - 1 && localItemsToRemove[i].z < obj.z + 1 && localItemsToRemove[i].z > obj.z - 1 && localItemsToRemove[i].id == type) // && localItemsToRemove[i].rX == obj.rX && localItemsToRemove[i].rY == obj.rY && localItemsToRemove[i].rZ == obj.rZ && localItemsToRemove[i].itemName == obj.itemName)
                {
                    return; //if that is true, we will stop the current iteration of the for loop and move to the next.
                }
            }
        }

            GameObject spawnObj = Instantiate(itemDatabase.items[type], position, rotation);
            //Spawn the object of the type that we have passed.
            chunkScript script = chunk.GetComponent<chunkScript>();
            //Get the chunkScript from our chunkObject.
            script.enviornmentObjects.Add(spawnObj);
            //Then, add our spawnObj to the enviornmentObjects list of the chunkScript we have inherited.

            spawnObj.name = type + " " + position;    

            #endregion
        
    }

    public void addUsernameToServer(GameObject player, string username)
    {
        CmdaddUsername(player, username);
    }

    [Command(requiresAuthority = false)]
    public void CmdaddUsername(GameObject player, string username)
    {
        playerInfo playerInf = new playerInfo();
        playerInf.player = player;
        playerInf.username = username;
        players.Add(playerInf);
        //Add newly joined player's info to our players list, for culling and information keeping.

        GameObject seasonObject = GameObject.Find("serverTime");
        timeScript serverTimeScript = seasonObject.GetComponent<timeScript>();

        serverTimeScript.RpcChangeSeason(serverTimeScript.abstractTime);
        //Then we will check the season, by just calling RpcChangeSeason which automatically checks the season. We do this as a new player has joined and seasons are not "networked" as materials are local.      
    }

}



[System.Serializable]
public class saveInfo
{
    public List<chunkInfo> chunks = new List<chunkInfo>();

}

[System.Serializable]
public class chunkTypes
{
    public int type;
    public GameObject Chunk;
    public List<int> itemsToSpawn = new List<int>();
    public List<int> numberOfItemsToSpawn = new List<int>();
}

[System.Serializable]
public class renderDistanceAmounts
{
    public float x;
    public float z;
}

[System.Serializable]
public class chunkObjectPropetrys
{
    public int chunkType;
    public int minChance;
    public int maxChance;
    public int spawnAmount;

    public bool isWater;
}

[Serializable]
 public class chunkInfo // This class is the backbone of our saving system as everything we save is put into this class and serialized.
{
    public float x;
    public float y;
    public float z;
    public float time;
    public int chunkType;
    public List<spawnedEnviornmentInfo> chunkItems = new List<spawnedEnviornmentInfo>();
}
[Serializable]
public class enviornmentInfo //This class is for our envorment info, it's a custom variable that holds a enviornment object's position and id.
{
    public float x;
    public float y;
    public float z;
    public float rX;
    public float rY;
    public float rZ;
    public int id;
    public string itemName;
}

[System.Serializable]
public class chunkInfoSerializable
{
    public float X;
    public float Y;
    public float Z;
    public float id;
}

[Serializable]
public class spawnedChunkInfo
{
    public bool deleted;
    public bool edited;
    public float x;
    public float y;
    public float z;
    public int chunkType;
    public GameObject chunkObject;
    public GameObject spawnedByPlayer; //Here we are adding a spawnedByPlayer variable to know *who* spawned the chunk first, and we can further use that to know if two players are in the same chunk area, this will be used for syncing mob movements with time, etc.
    public List<spawnedEnviornmentInfo> serializationObjects = new List<spawnedEnviornmentInfo>();
    public List<spawnedEnviornmentInfo> removeSerializationObjects = new List<spawnedEnviornmentInfo>();


}
[Serializable]
public class spawnedEnviornmentInfo
{
    public bool deleted;
    public string itemName;
    public float rX;
    public float rY;
    public float rZ;
    public float x;
    public float y;
    public float z;
    public int id;
}

[Serializable]
public class worlds
{
    public string worldName;
    public float worldSeed;
}
[Serializable]
public class seedInfo
{
    public List<worlds> worldInfo = new List<worlds>();
}

[Serializable]
public class playerInfo
{
    public GameObject player;
    public int playerRenderDistance;
    public String username;
}