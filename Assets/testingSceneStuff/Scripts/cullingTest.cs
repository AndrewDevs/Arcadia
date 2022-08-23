using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cullingTest : MonoBehaviour
{
    public GameObject playerObject;

    private Quaternion rotation;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       /* rotation = playerObject.transform.rotation;

        if(rotation.y >= 0 && rotation.y < 0.70)
        {
            UnityEngine.Debug.Log("0");
        }
        if (rotation.y >= 0.70 && rotation.y < 1)
        {
            UnityEngine.Debug.Log("1");
        }
        if (rotation.y > 1)
        {
            UnityEngine.Debug.Log("2");
        }
        if (rotation.y < 0)
        {
            UnityEngine.Debug.Log("3");
        } */
    }
}
