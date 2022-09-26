using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Security.AccessControl;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class worldName : MonoBehaviour
{
    public string oldName;
    public Text oldNameObj;
    public InputField currentName;


    // Start is called before the first frame update
    void Start()
    {
        oldName = oldNameObj.text;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void changeWorldName()
    {
        if (File.Exists(Application.persistentDataPath + "/seedFile.dat"))
        {
            worlds world = new worlds();


            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + "seedFile" + ".dat", FileMode.Open);
            seedInfo savedSeedInfo = (seedInfo)bf.Deserialize(file);
            file.Close();

            foreach (worlds worldInstance in savedSeedInfo.worldInfo) //Seach in said worlds list for one that equals our world's name, and is thus our world.
            {
                if (worldInstance.worldName == oldName)
                {
                    worldInstance.worldName = currentName.text;
                }

            }

            BinaryFormatter Bf = new BinaryFormatter();
            FileStream file0 = File.Open(Application.persistentDataPath + "/" + "seedFile" + ".dat", FileMode.Open);
            Bf.Serialize(file0, savedSeedInfo);
            file0.Close();
            //Here we are opening the worldData file that already exists and serializing new chunk data to it, then closing it. Now we merely add to and already existant file, instead of completly remaking it.


            System.IO.File.Move(Application.persistentDataPath + "/" + oldName, Application.persistentDataPath + "/" + currentName.text);

            File.Delete(Application.persistentDataPath + "/" + oldName);

            oldName = currentName.text;


        }
    }
}
