using System.Collections;
using System.Collections.Generic;
using Mirror;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using static saveInfo;

public class chunkScript : NetworkBehaviour
{
    public bool spawned;

    public bool visible;

    public int type;

    public List<GameObject> enviornmentObjects = new List<GameObject>();


    public override void OnStartClient() //Ran when enabled
    {       
        if (spawned == false)
        {
            if (NetworkManager.singleton.mode == NetworkManagerMode.ClientOnly)//If this is a client that is running this script.
            {
                CmdspawnEnv(type, this.gameObject);
                //We call CmdspawnEnv, which will spawn the enviornment on the server, and pass the chunks type so we know what to spawn and this.gameObject as the chunk.
            }
            else if (NetworkManager.singleton.mode == NetworkManagerMode.Host) //If this is a host that is running this.
            {
                CmdspawnEnv(type, this.gameObject);
                //We call CmdspawnEnv, which will spawn the enviornment on the server, and pass the chunks type so we know what to spawn and this.gameObject as the chunk.

            }
            spawned = true;
            //Call the command that is to dictate the times when mobs move, we want to centralize it so mobs on different clients, on the same chunk, will be in sync time-wise, as they already are position wise (see: worldGenerator TargetSpawnItems).
        }
    }

    public void Update()
    {


    }



    [Command(requiresAuthority = false)]
    public void CmdspawnEnv(int type, GameObject chunk, NetworkConnectionToClient sender = null) //ISSUE IS BECAUSE THE OBJECT DOES NOT HAVE AUTHORITY THEY CANNOT PASS SENDER
    {
        float distance = Vector3.Distance(sender.identity.gameObject.transform.position, chunk.transform.position);
        if (distance < 200)
        {
            GameObject worldGenObject = GameObject.Find("worldGenerator(Clone)");
            //Find the worldGenerator object...so we can get the worldGenerator script as it contains the necessary function to spawn the enviornment.
            worldGenerator worldGenScript = worldGenObject.GetComponent<worldGenerator>();
            //Get the worldGenerator script from our object we have already located (worldGenObject).

            worldGenScript.SpawnEnviornment(chunk, type, sender);
            /*With the script we have gotten we will then call the SpawnEnviornment function within worldGeneration, we do this on the server as only the server has the necessary info (mainly seed) to spawn things such as enviornment.
              This function will kick of the other functions that actually spawn the enviornment, all we are doing here is getting the necessary info and having it run on the server, as chunkScript is ran locally unless a command is used.*/
        }
    }

    public void destroyEnviornment() //This function will merely go through this chunks *local* enviornmentObjects list and destroy the objects in said list.
    {
        for (int i = 0; i < enviornmentObjects.Count; i++) // This loop goes through the enviornmentObjects list, obviously.
        {
            Destroy(enviornmentObjects[i]); 
            //Destroy the enviornmentObject our loop is on.
        }
        CmddestroyChunk(this.gameObject);
        //After we have locally destroyed the local enviornment we will call a command to destroy the chunk. We call a command as only the server can do NetworkServer.Destroy.
    }

    [Command(requiresAuthority = false)]
    public void CmddestroyChunk(GameObject chunk) //This function will merely destroy the chunk that we pass to it, I explain why we use a command at the end of the destroyEnviornment function.
    {
        NetworkServer.Destroy(chunk);
        //Destroy the chunk over the network.
    }







}
