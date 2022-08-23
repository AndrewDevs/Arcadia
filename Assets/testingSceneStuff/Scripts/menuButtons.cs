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


public class menuButtons : MonoBehaviour
{

    NetworkManager manager;

    public bool exit = false;

    public bool hostingServer = false;

    public GameObject networkManagerObject;

    public GameObject canvasObject;

    public GameObject thisObject;

    public GameObject gameCanvasObject;

    public GameObject worldsScreen;

    public GameObject worldHolder;

    public GameObject worldHolderUI;

    public GameObject worldList;

    public bool findworlds = false;

    public GameObject serverUIObject;

    public GameObject mainMenu;

    public Canvas canvas;

    public InputField serverAddressText;

    public InputField username;

    public menuButtons parentScript;

    public playerMovement playerScript;

    public List<worlds> worldsList = new List<worlds>();

    public List<GameObject> worldInstances = new List<GameObject>();




    // Start is called before the first frame update
    void Start()
    {
        networkManagerObject = GameObject.Find("NetworkManager");
        manager = networkManagerObject.GetComponent<NetworkManager>();
        mainMenu = GameObject.Find("mainCanvas");
        if (hostingServer == false)
        {
            canvasObject = GameObject.Find("menuCanvas");
            canvas = canvasObject.GetComponent<Canvas>();
        }

        if(findworlds == true)
        {
            if (File.Exists(Application.persistentDataPath + "/seedFile.dat"))
            {
                float amount = Screen.height * 1f;
                worlds world = new worlds();

                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/" + "seedFile" + ".dat", FileMode.Open);
                seedInfo savedSeedInfo = (seedInfo)bf.Deserialize(file);
                file.Close();
                //Open the file that contains the worlds list.
                foreach (worlds worldInstance in savedSeedInfo.worldInfo) //Seach in said worlds list for one that equals our world's name, and is thus our world.
                {
                    amount = amount - 100;

                    GameObject instance = Instantiate(worldHolder, new Vector3(0, amount, 0), Quaternion.identity);
                    Transform textObj = instance.transform.Find("worldName");
                    Text text = textObj.GetComponent<Text>();
                    text.text = worldInstance.worldName;

                    worldInstances.Add(instance);

                    RectTransform rt = worldHolderUI.GetComponent<RectTransform>();
                    rt.offsetMin -= new Vector2(rt.offsetMin.x, 110);
                    rt.offsetMax -= new Vector2(rt.offsetMax.x, 0);




                }

                foreach (GameObject worldObject in worldInstances)
                {

                    GameObject holder = GameObject.Find("worldHolder");
                    worldObject.transform.SetParent(holder.transform);

                    worldObject.transform.position = new Vector3(Screen.width * 0.5f, worldObject.transform.position.y, 0);
                    //Setting our worldObject, which is the actual world button, to the center of our screen.




                }


            }


        }


    }
    // Update is called once per frame
    void Update()
    {
        
    }


    public void singlePlayer()
    {
        worldsScreen.SetActive(true);

        //manager.StartHost();
        //We are starting a client + host here, so the player is playing in singleplayer.
        //canvasObject.SetActive(false);
        //We then set the player's canvas for the menu to false, so they cannot see it in game.
    }

    public void JoinServer()
    {
        manager.networkAddress = serverAddressText.text;
        UnityEngine.Debug.Log(manager.networkAddress);
        manager.StartClient();
        playerScript.username = username.text;
        canvasObject.SetActive(false);

    }

    public void hostServer()
    {
        worldsScreen.SetActive(true);
        GameObject parent = GameObject.Find("worlds");
        menuButtons parentScript = parent.GetComponent<menuButtons>();
        parentScript.hostingServer = true;


    }

    public void Return()
    {
        worldsScreen.SetActive(false);

    }

    public void returnToMenu()
    {

        // stop host if host mode
        if (NetworkServer.active && NetworkClient.isConnected)
        {
                manager.StopHost();
                SceneManager.LoadScene("testScene0");
        }
        // stop client if client-only
        else if (NetworkClient.isConnected)
        {
                manager.StopClient();
                SceneManager.LoadScene("testScene0");
        }
        // stop server if server-only
        else if (NetworkServer.active)
        {
                manager.StopServer();
                if (hostingServer == false)
                {
                    canvas.enabled = true;
                }
                else
                {
                    canvasObject = GameObject.Find("server");
                canvasObject.SetActive(false);
                }
                SceneManager.LoadScene("testScene0");

        }

        if (hostingServer == false)
        {
            gameCanvasObject = GameObject.Find("Canvas(Clone)");
            gameCanvasObject.name = "null";
            gameCanvasObject.SetActive(false);
        }
    }

    public void multiplayer()
    {
        SceneManager.LoadScene("multiplayer");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void newWorld()
    {
        GameObject parent = GameObject.Find("worlds");

        parentScript = parent.GetComponent<menuButtons>();

        if (parentScript.hostingServer == false)
        {
            manager.StartHost();
        }
        else
        {
            manager.StartServer();
        }
        GameObject worldGen = GameObject.Find("worldGenerator(Clone)");


        worldGenerator worldGenScript = worldGen.GetComponent<worldGenerator>();

        worldGenScript.worldName = null;

        playerScript.username = username.text;

        worldGenScript.OnStart();

        if (parentScript.hostingServer == false)
        {
            canvasObject.SetActive(false);
            //We then set the player's canvas for the menu to false, so they cannot see it in game.
        }
        else
        {
            mainMenu.SetActive(false);
            parent.SetActive(false);
        }


        //We then set the player's canvas for the menu to false, so they cannot see it in game.
    }

    public void loadWorld()
    {

        worldList world = new worldList();

        GameObject parent = GameObject.Find("worlds");

        parentScript = parent.GetComponent<menuButtons>();

        GameObject usernameObject = GameObject.Find("usernameInput");
        username = usernameObject.GetComponent<InputField>();
        playerScript.username = username.text;

        if (parentScript.hostingServer == false)
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
            string name = "filler";

            Transform worldName = thisObject.transform.Find("worldName");

            Text nameText = worldName.GetComponent<Text>();

            name = nameText.text;

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + "seedFile" + ".dat", FileMode.Open);
            seedInfo savedSeedInfo = (seedInfo)bf.Deserialize(file);
            file.Close();
            //Open the file that contains the worlds list.
            worldGenerator worldGenScript = worldGen.GetComponent<worldGenerator>();

            foreach (worlds worldInstance in savedSeedInfo.worldInfo) //Seach in said worlds list for one that equals our world's name, and is thus our world.
            {
                if(worldInstance.worldName == name)
                {

                    worldGenScript.worldName = worldInstance.worldName;

                }
            }
            worldGenScript.OnStart();
            

        }

        if (parentScript.hostingServer == false)
        {
            canvasObject.SetActive(false);
            //We then set the player's canvas for the menu to false, so they cannot see it in game.
        }
        else
        {
            mainMenu.SetActive(false);
            parent.SetActive(false);
        }


    }

   /* public void CmdaddPlayer(worldGenerator script, playerInfo Player)
    {
        script.players.Add(Player);
    } */


}

[Serializable]
public class worldList
{
    public Sprite worldImage;
    public string worldName;
}

