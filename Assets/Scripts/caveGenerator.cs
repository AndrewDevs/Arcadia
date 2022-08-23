using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class caveGenerator : MonoBehaviour
{
    public GameObject caveBlock;
    public GameObject lastCaveObject;
    public GameObject lastBottomSpawned;
    public GameObject lastLeftWallSpawned;
    public GameObject turnCaveBlock;
    public int variationX;
    public float variationY;
    public int variationZ;
    public int lengthOfCave;

    Vector3 caveScale = new Vector3(0, 0, 0);

    public Enviorment objectSpawner;

    // Start is called before the first frame update
    void Start()
    {
        caveGeneration();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void caveGeneration() {

        int turnAt = UnityEngine.Random.Range(2, lengthOfCave);
        bool turned = false;
        Vector3 bottomPosition;

        for (int i = 0; i <= lengthOfCave; i++){


            if(i == turnAt)
            {
                turned = true;
                Vector3 turnPosition = new Vector3(lastBottomSpawned.transform.position.x + turnCaveBlock.transform.localScale.x, UnityEngine.Random.Range(lastBottomSpawned.transform.position.y - variationY, lastBottomSpawned.transform.position.y + variationY), lastBottomSpawned.transform.position.z);
                lastBottomSpawned = Instantiate(turnCaveBlock, turnPosition, Quaternion.Euler(0,90,0));
                //In this part we spawn the turnCaveBlock, and set turned to true.
            }
            else if (turned == false)
            {
                if (i != lengthOfCave)
                {
                    bottomPosition = new Vector3(lastBottomSpawned.transform.position.x + caveBlock.transform.localScale.x, UnityEngine.Random.Range(lastBottomSpawned.transform.position.y - variationY, lastBottomSpawned.transform.position.y + variationY), lastBottomSpawned.transform.position.z);
                    lastBottomSpawned = Instantiate(caveBlock, bottomPosition, Quaternion.identity);
                    //Spawns normal cave object, which is not turned.
                }
                else if (i == lengthOfCave)
                {
                    bottomPosition = new Vector3(lastBottomSpawned.transform.position.x + caveBlock.transform.localScale.x, UnityEngine.Random.Range(lastBottomSpawned.transform.position.y - variationY, lastBottomSpawned.transform.position.y + variationY), lastBottomSpawned.transform.position.z);
                    lastBottomSpawned = Instantiate(lastCaveObject, bottomPosition, Quaternion.identity);
                    //spawn the lastCaveObject without rotation
                }

            }
            else if(turned == true)
            {
                if (i != lengthOfCave)
                {
                    bottomPosition = new Vector3(lastBottomSpawned.transform.position.x, UnityEngine.Random.Range(lastBottomSpawned.transform.position.y - variationY, lastBottomSpawned.transform.position.y + variationY), lastBottomSpawned.transform.position.z + caveBlock.transform.localScale.x);
                    lastBottomSpawned = Instantiate(caveBlock, bottomPosition, Quaternion.Euler(0, -90, 0));
                    //Here, if turned == true and we have not reached the end of the cave, we spawn the caveBlock but we turn them by 90.
                }else if (i == lengthOfCave)
                {
                    bottomPosition = new Vector3(lastBottomSpawned.transform.position.x, UnityEngine.Random.Range(lastBottomSpawned.transform.position.y - variationY, lastBottomSpawned.transform.position.y + variationY), lastBottomSpawned.transform.position.z + lastCaveObject.transform.localScale.x);
                    lastBottomSpawned = Instantiate(lastCaveObject, bottomPosition, Quaternion.Euler(0, -90, 0));
                    //Here we spawn the lastCaveObject, as we are at the end of the cave, and turn it 90 due to turned being true.
                }

            }

                caveScale = new Vector3 (lastBottomSpawned.transform.localScale.x, 1, UnityEngine.Random.Range(lastBottomSpawned.transform.localScale.z, variationZ));

            lastBottomSpawned.transform.localScale = caveScale;
                lastBottomSpawned.transform.parent = this.transform;
            // This just changes the caveBlocks scale and makes the newly instantiated block a child of the object the script is on.
            objectSpawner = lastBottomSpawned.GetComponent<Enviorment>();
            objectSpawner.goDownSpawnList(); 

            
            


        }

            


    }
}