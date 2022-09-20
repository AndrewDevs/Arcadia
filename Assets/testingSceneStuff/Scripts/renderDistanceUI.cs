using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class renderDistanceUI : NetworkBehaviour
{
    public Slider renderSlider;
    public Slider FOVSlider;

    public Text RDNumber;
    public Text FOVNumber;

    public playerLocator playerAccess;

    public GameObject options;




    // Start is called before the first frame update
    void Start()
    {

        renderSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        FOVSlider.onValueChanged.AddListener(delegate { fovValueChangeCheck(); });

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ValueChangeCheck()
    {
        RDNumber.text = renderSlider.value.ToString();
        //Depict slider value on our UI text.

        float value = renderSlider.value;

        int newDistance = (int) value;


        playerAccess.playerScript.CmdUpdateRenderDistance(newDistance);
        //Call command to change the service side render distance for the player.

    }

    public void fovValueChangeCheck()
    {
        Camera camera = playerAccess.playerScript.cameraObject.GetComponent<Camera>();

        FOVNumber.text = FOVSlider.value.ToString();

        camera.fieldOfView = FOVSlider.value;
    }

    public void Back()
    {
        options.SetActive(false);
    }



}
