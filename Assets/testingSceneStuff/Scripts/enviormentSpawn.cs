using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;
using Mirror;

public class enviormentSpawn : NetworkBehaviour
{ 
    /*
    public List<int> itemsToSpawn = new List<int>();
    public List<int> numberOfItemsToSpawn = new List<int>();

    public GameObject objectList;
    GameObject databaseObject;

    cullScriptHolder objectListScript;

    worldGenerator worldGenScript;

    GameObject worldGenObject;

    Vector3 Playerposition;

    Vector3 rangeDistance;

    public bool setPosition = false;
    public bool cave;
    public bool rotate;
    [SyncVar]
    public bool go; 

    int onNumber;
    int amountSpawned;


    public Collider areaCollider;

    public Database itemDatabase;

    Quaternion rotation;

    public Vector3 spawnArea;

    Vector3 position;



    // use OnStartServer so this only runs at the start of the server.
    public void Start()
    {

            areaCollider = this.GetComponent<Collider>();

            databaseObject = GameObject.Find("Database(Clone)");
            itemDatabase = databaseObject.GetComponent<Database>();
            worldGenObject = GameObject.Find("worldGenerator(Clone)");
            //Find our worldGeneration object so we can access its script.
            worldGenScript = worldGenObject.GetComponent<worldGenerator>();
            //Acces worldGenObject's script so we can reference it.
            objectList = GameObject.Find("objectList(Clone)");
            //Find the networked list which we keep all of the worlds spawned objects in.
            objectListScript = objectList.GetComponent<cullScriptHolder>();
            //Derive that networked list's script from the found object.


            if (cave == false)
            {
            if (go == false)
            {
                goDownSpawnList();
            }
            }


    }



    public int goDownSpawnList()
    {
        onNumber = -0;

        for (onNumber = 0; onNumber < itemsToSpawn.Count; onNumber++)
        {

            spawnAmount();

            if(onNumber == itemsToSpawn.Count - 1)
            {
                go = true;
            }

        }


        return onNumber;
        //This loop merely goes down the list of items to spawn, onNumber tells us which number in the list the loop is on. If it has not reached the end of the list it runs SpawnAmount();

    }

    public int spawnAmount()
    {

        int i = -0;

        for (i = numberOfItemsToSpawn[onNumber]; i != 0; i--)
        {
            if (cave == false && setPosition == false)
            {
                position = new Vector3(UnityEngine.Random.Range(areaCollider.bounds.max.x, areaCollider.bounds.min.x), transform.position.y + itemDatabase.items[itemsToSpawn[onNumber]].transform.localScale.y, UnityEngine.Random.Range(areaCollider.bounds.max.z, areaCollider.bounds.min.z));
            }
            else if(setPosition == false)
            {
                position = new Vector3(UnityEngine.Random.Range(areaCollider.bounds.max.x, areaCollider.bounds.min.x), transform.position.y + itemDatabase.items[itemsToSpawn[onNumber]].transform.localScale.y, UnityEngine.Random.Range(areaCollider.bounds.max.z, areaCollider.bounds.min.z));

            }
            else
            {
                position = new Vector3(this.transform.position.x + 31, this.transform.position.y - 22, this.transform.localScale.z / 4 + this.transform.position.z);
            }
            if (rotate == true) { 
                rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
            }
            else
            {
                rotation = Quaternion.Euler(0, 0,0);

            }

                worldGenScript.CmdspawnItems(itemsToSpawn[onNumber], position, rotation);
            //Here we are going to spawn our object through the worldGenScript as the worldGenScript has authority and this object does not.  

            numberOfItemsToSpawn[onNumber]--;


        }
        return i;

        //Here we get the number of items to spawn by stating numberOfITemsToSpawn[OnNumber], which tells us the amount needed to spawn on whatever number the prior loop is on.
        // From there we spawn the item the loop is on in the list, and subtract 1 from numberOfItemsToSpawn, we locate it with OnNumber.
    }

    */

}






