using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class gameInfo : MonoBehaviour
{

    public Text cordsText;
    public Text fpsText;
    public GameObject Player;
    public inventoryUI inventoryUi;

    public bool cords;
    public bool fps;


    // Start is called before the first frame update
    void Start()
    {
        Player = inventoryUi.Player;

    }

    // Update is called once per frame
    void Update()
    {
        if (cords == true)
        {
            cordsText.text = "X: " + Player.transform.position.x + " " + "Y: " + Player.transform.position.y + " " + "Z: " + Player.transform.position.z;
        }
        else if(fps == true)
        {
            float fps = 1 / Time.unscaledDeltaTime;
            fpsText.text = "FPS: " + Mathf.Round(fps);
        }
    }
}
