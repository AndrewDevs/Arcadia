using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using ProtoBuf;

public class SaveAndLoad : MonoBehaviour
{
    public string File1Path;
    public persons MyGroup;

    void OnGUI()
    {
        if (GUI.Button(new Rect (10,10,200,100), "Load File 1"))
        {
            if (string.IsNullOrEmpty(File1Path))
            {
                UnityEngine.Debug.Log("empty");
                return;
            }

            if (!File.Exists(File1Path))
            {
                UnityEngine.Debug.Log("doesn't exist");
                return;
            }
            MyGroup = Serializer.Deserialize<persons>(new FileStream(File1Path, FileMode.Open, FileAccess.Read));
            //load a file with the path of file1Path, and set it equal to "MyGroup".
            UnityEngine.Debug.Log("let's go");



        }

        if (GUI.Button(new Rect(10, 120, 200, 100), "Save File 1"))
        {
            if (string.IsNullOrEmpty(File1Path))
            {
                UnityEngine.Debug.Log("empty");
                return;
            }

            if(MyGroup == null)
            {
                UnityEngine.Debug.Log("null");
                return;
            }

            using (FileStream Stream = new FileStream(File1Path, FileMode.Create, FileAccess.Write)) //By using "using" the filestream closes itself automatically, instead of us having to manually do it with later code.
            {

                Serializer.Serialize<persons>(Stream, MyGroup);

                Stream.Flush();

                

            }
        }
    }


}
