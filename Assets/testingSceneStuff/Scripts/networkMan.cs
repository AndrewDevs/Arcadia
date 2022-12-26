using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class networkMan : NetworkBehaviour
{

    public GameObject worldGenerationObject;

    public GameObject itemDatabase;

    public GameObject objectList;

    public GameObject serverChatObject;

    public GameObject textObject;

    public GameObject sunObject;

    public GameObject skyBoxDirLight;





    Vector3 position = new Vector3(0, 0, 0);

    // Use OnStartServer so this only runs when the server is created.
    public override void OnStartServer()
    {
        if (isServer == true)
        {
            GameObject objList = Instantiate(objectList, position, Quaternion.identity);
            NetworkServer.Spawn(objList);
            //Spawns object, and sets are most recently spawned object to last spawned. 
            GameObject skyboxDir = Instantiate(skyBoxDirLight, position, Quaternion.identity);
            NetworkServer.Spawn(skyboxDir);
            GameObject database = Instantiate(itemDatabase, position, Quaternion.identity);
            NetworkServer.Spawn(database);
            GameObject worldGenObj = Instantiate(worldGenerationObject, position, Quaternion.identity);
            NetworkServer.Spawn(worldGenObj);
            GameObject chatObj = Instantiate(serverChatObject, position, Quaternion.identity);
            NetworkServer.Spawn(chatObj);
            GameObject sunObj = Instantiate(sunObject, position, Quaternion.identity);
            NetworkServer.Spawn(sunObj);

        }



    }
}
