using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;

public class Enviorment : MonoBehaviour
{/// <summary>
/// 
/// </summary>

    public List<GameObject> itemsToSpawn = new List<GameObject>();
    public List<int> numberOfItemsToSpawn = new List<int>();

    public GameObject player;
    
    Vector3 Playerposition;

    Vector3 rangeDistance;

    public bool cave;

    public bool setPosition = false;

    int onNumber;
    int amountSpawned;

    public Collider areaCollider;


    public Vector3 spawnArea;

    Vector3 position;



    // Start is called before the first frame update
    void Start()
    {
        areaCollider = this.GetComponent<Collider>();

        if (cave == false)
        {
            goDownSpawnList();
        }



        //Vector3 position = new Vector3( transform.position.x +  );
        //Instantiate(itemsToSpawn[onNumber], position, Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int goDownSpawnList()
    {
        onNumber = -0;

        for (onNumber = 0; onNumber < itemsToSpawn.Count; onNumber++)
        {
           
                spawnAmount();


        }
        return onNumber;
        //This loop merely goes down the list of items to spawn, onNumber tells us which number in the list the loop is on. If it has not reached the end of the list it runs SpawnAmount();

    }

   public int spawnAmount()
    {

        int i = -0;

        for ( i = numberOfItemsToSpawn[onNumber]; i != 0; i--)
        {
            if (cave == false && setPosition == false)
            {
                 position = new Vector3(UnityEngine.Random.Range(areaCollider.bounds.max.x, areaCollider.bounds.min.x), transform.position.y, UnityEngine.Random.Range(areaCollider.bounds.max.z, areaCollider.bounds.min.z));
            }
            else if (setPosition == false)
            {
                 position = new Vector3(UnityEngine.Random.Range(areaCollider.bounds.max.x, areaCollider.bounds.min.x), transform.position.y + itemsToSpawn[onNumber].transform.localScale.y, UnityEngine.Random.Range(areaCollider.bounds.max.z, areaCollider.bounds.min.z));

            }
            else
            {
                position = new Vector3(this.transform.position.x, this.transform.localScale.y * 2 - this.transform.position.y, this.transform.position.z + this.transform.localScale.z);
            }
            Quaternion rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
            GameObject lastSpawned = Instantiate(itemsToSpawn[onNumber], position, rotation);           
            lastSpawned.transform.parent = this.transform;        
            numberOfItemsToSpawn[onNumber]--;

        }
        return i;

        //Here we get the number of items to spawn by stating numberOfITemsToSpawn[OnNumber], which tells us the amount needed to spawn on whatever number the prior loop is on.
        // From there we spawn the item the loop is on in the list, and subtract 1 from numberOfItemsToSpawn, we locate it with OnNumber.

    }


}


    



