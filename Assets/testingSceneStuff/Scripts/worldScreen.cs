using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Mirror;
using System.Linq;
using System.Security.AccessControl;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using UnityEngine;
using static worldGenerator;
using static playerInfo;
using System;

public class worldScreen : NetworkBehaviour
{

    NetworkManager manager;

    public GameObject thisObject;

    public GameObject currentWorld;

    public InputField currentWorldField;

    public GameObject networkManagerObject;

    public GameObject canvasObject;

    public GameObject mainMenu;

    public GameObject selectedUI;

    public menuButtons menuScript;

    public playerMovement playerScript;

    public InputField username;





    // Start is called before the first frame update
    void Start()
    {
        networkManagerObject = GameObject.Find("NetworkManager");
        manager = networkManagerObject.GetComponent<NetworkManager>();
        mainMenu = GameObject.Find("mainCanvas");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeSelectedUI(GameObject box)
    {
        if (selectedUI != null)
        {
            selectedUI.SetActive(false);
        }
        selectedUI = box;
        selectedUI.SetActive(true);
    }

    public void loadWorld()
    {

        worldList world = new worldList();



        GameObject usernameObject = GameObject.Find("usernameInput");
        username = usernameObject.GetComponent<InputField>();
        playerScript.username = username.text;

        if (menuScript.hostingServer == false)
        {
            manager.StartHost();
        }
        else
        {
            manager.StartServer();
        }
        //We are starting a client + host here, so the player is playing in singleplayer.
        GameObject worldGen = GameObject.Find("worldGenerator(Clone)");

        if (File.Exists(Application.persistentDataPath + "/seedFile.dat"))
        {

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + "seedFile" + ".dat", FileMode.Open);
            seedInfo savedSeedInfo = (seedInfo)bf.Deserialize(file);
            file.Close();
            //Open the file that contains the worlds list.
            worldGenerator worldGenScript = worldGen.GetComponent<worldGenerator>();

            foreach (worlds worldInstance in savedSeedInfo.worldInfo) //Seach in said worlds list for one that equals our world's name, and is thus our world.
            {
                if (worldInstance.worldName == currentWorldField.text)
                {

                    worldGenScript.worldName = worldInstance.worldName;

                }
            }
            worldGenScript.OnStart();


        }

        if (menuScript.hostingServer == false)
        {
            canvasObject.SetActive(false);
            //We then set the player's canvas for the menu to false, so they cannot see it in game.
        }
        else
        {
            mainMenu.SetActive(false);
            //parent.SetActive(false);
        }


    }


    public void deleteWorld()
    {
        if (File.Exists(Application.persistentDataPath + "/seedFile.dat"))
        {

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + "seedFile" + ".dat", FileMode.Open);
            seedInfo savedSeedInfo = (seedInfo)bf.Deserialize(file);
            file.Close();
            //Open the file that contains the worlds list.
            foreach (worlds worldInstance in savedSeedInfo.worldInfo) //Seach in said worlds list for one that equals our world's name, and is thus our world.
            {
                if (worldInstance.worldName == currentWorldField.text)
                {

                    savedSeedInfo.worldInfo.Remove(worldInstance);
                    break;

                }
            }

            BinaryFormatter Bf = new BinaryFormatter();
            FileStream file0 = File.Open(Application.persistentDataPath + "/" + "seedFile" + ".dat", FileMode.Open);
            Bf.Serialize(file0, savedSeedInfo);
            file0.Close();
            //Here we are opening the worldData file that already exists and serializing new chunk data to it, then closing it. Now we merely add to and already existant file, instead of completly remaking it.


            int index = new int();

            foreach (GameObject world in menuScript.worldInstances)
            {
                if(world == currentWorld)
                {
                    Destroy(currentWorld);
                    index = menuScript.worldInstances.IndexOf(world);
                    menuScript.worldInstances.Remove(world);
                    File.Delete(Application.dataPath + "/" + currentWorldField.text);
                    break;
                }

            }

            for (int i = index; i < menuScript.worldInstances.Count; i++)
            {
                menuScript.worldInstances[i].transform.position = new Vector3(menuScript.worldInstances[i].transform.position.x, menuScript.worldInstances[i].transform.position.y + 100, menuScript.worldInstances[i].transform.position.z);
            }

        }

    }




}


