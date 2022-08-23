using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class playerTracker : MonoBehaviour
{

    public GameObject player;
    public GameObject lastChunkObject;

    public float chunkRange;
    public List<GameObject> chunks = new List<GameObject>();
    Vector3 playerPosition;
    public Vector3 lastChunkSpawnedDistance;
    public float distanceForLastchunk;
    public bool toFar;
    public worldGenerationScript biomeScript;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkChunkDistance();

        foreach (GameObject chunkObject in chunks)
        {
           
            float distance = Vector3.Distance(chunkObject.transform.position, player.transform.position);
            //Here we calculate the distance between the player and the chunk we are currently on in our loop.
            if (distance > chunkRange)
            {
                chunkObject.SetActive(false);
            }
            else
            {
                chunkObject.SetActive(true);
            }

        }

    }

    public void checkChunkDistance()
    {

        distanceForLastchunk = Vector3.Distance(lastChunkObject.transform.position, player.transform.position);
        //This calculates the distance of the lastchunkObject and the player.
        if (distanceForLastchunk > chunkRange)
        {
            biomeScript.On = false;

        }
        else
        {
            biomeScript.On = true;
        }

    }
}
