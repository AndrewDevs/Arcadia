using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BushScript : NetworkBehaviour
{
    public List<GameObject> itemsToSpawn = new List<GameObject>();
    public List<int> amountsToSpawn = new List<int>();

    Vector3 position = new Vector3(0, 0, 0);

    int onNumber;

    public float variation;

    public Collider areaCollider;

    float xBounds;
    float yBounds;
    float zBounds;


    // Start is called before the first frame update
    void Start()
    {
        areaCollider = this.GetComponent<Collider>();
        goDownSpawnList();
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

    public void spawnAmount()
    {
        for (int i = amountsToSpawn[onNumber]; amountsToSpawn[onNumber] != 0; amountsToSpawn[onNumber]--)
        {
            int chosenNumber = UnityEngine.Random.Range(1, 5);

            if(chosenNumber == 1)
            {
                xBounds = UnityEngine.Random.Range(areaCollider.bounds.max.x, areaCollider.bounds.min.x);
                zBounds = UnityEngine.Random.Range(areaCollider.bounds.max.z, areaCollider.bounds.min.z);
                position = new Vector3(UnityEngine.Random.Range(xBounds, zBounds), this.transform.position.y + this.transform.localScale.y / variation, this.transform.position.z);
            }else if (chosenNumber == 2)
            {
                position = new Vector3( this.transform.position.x + this.transform.localScale.x / variation, UnityEngine.Random.Range(areaCollider.bounds.max.y, areaCollider.bounds.min.y),  this.transform.position.z);

            }
            else if (chosenNumber == 3)
            {
            position = new Vector3(this.transform.position.x - this.transform.localScale.x / variation, UnityEngine.Random.Range(areaCollider.bounds.max.y, areaCollider.bounds.min.y), this.transform.position.z);
    
            }
            else if (chosenNumber == 4)
            {
                position = new Vector3( this.transform.position.x, UnityEngine.Random.Range(areaCollider.bounds.max.y, areaCollider.bounds.min.y), this.transform.position.z - this.transform.localScale.z / variation);

            }
            else if (chosenNumber == 5)
            {
                position = new Vector3(this.transform.position.x, UnityEngine.Random.Range(areaCollider.bounds.max.y, areaCollider.bounds.min.y), this.transform.position.z + this.transform.localScale.z / variation);

            }

            GameObject lastSpawned = Instantiate(itemsToSpawn[onNumber], position, Quaternion.identity);
            CmdSpawn(lastSpawned);
        }
    }

    [Command]
    public void CmdSpawn(GameObject objectToSpawn)
    {
        NetworkServer.Spawn(objectToSpawn);
    }

}
