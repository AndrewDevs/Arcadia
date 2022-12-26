using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class dayNight : NetworkBehaviour
{
    [SyncVar]
    public int time;

    public GameObject light;

    public GameObject skyBoxDirLight;

    public Light lightComponent;


    // Start is called before the first frame update
    void Start()
    {
        skyBoxDirLight = GameObject.Find("sunDirLight(Clone)");

        StartCoroutine(rotateLight());

        lightComponent = light.GetComponent<Light>();

        RenderSettings.ambientIntensity = 1.5f;
        RenderSettings.reflectionIntensity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public IEnumerator rotateLight()
    {
        time++;
        if(time >= 95) //Night
        {
            RenderSettings.ambientIntensity = 0.1f;
            //lightComponent.intensity = 0.1f;
            lightComponent.shadowStrength = 0;
            lightComponent.color = Color.grey;
        }
        if (time >= 190) //Day
        {
            RenderSettings.ambientIntensity = 1.5f;
            //lightComponent.intensity = 1;
            lightComponent.color = Color.white;
            time = 0;
        }
        yield return new WaitForSeconds(1);
        skyBoxDirLight.transform.Rotate(2, 0, 0, Space.Self);
        StartCoroutine(rotateLight());
    }
}
