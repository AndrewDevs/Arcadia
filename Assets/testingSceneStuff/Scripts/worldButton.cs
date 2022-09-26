using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class worldButton : MonoBehaviour
{
    GameObject worldsScreen;

    worldScreen worldScreenScript;

    public InputField thisInputField;

    public GameObject box;


    // Start is called before the first frame update
    void Start()
    {
        worldsScreen = GameObject.Find("worlds");
        worldScreenScript = worldsScreen.GetComponent<worldScreen>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectedWorld()
    {
        UnityEngine.Debug.Log("ran");
        worldScreenScript.currentWorldField = thisInputField;
        worldScreenScript.currentWorld = this.gameObject;
        worldScreenScript.changeSelectedUI(box);



    }
}
