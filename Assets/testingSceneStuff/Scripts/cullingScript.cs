using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cullingScript : MonoBehaviour
{
    public float playerDistance;
    float distance;

    public GameObject playerObject;

    public List<GameObject> objectsToCull = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        GameObject objectList = GameObject.Find("objectList(Clone)");
        cullScriptHolder objectListScript = objectList.GetComponent<cullScriptHolder>();
        objectsToCull = objectListScript.cullObjects;
    }

    // Update is called once per frame
    void Update()
    {
        checkDistance();
    }

    public void checkDistance()
    {
        //Use a command inorder to access/use NetworkServer.
        foreach (GameObject cullObject in objectsToCull)
        {
            distance = Vector3.Distance(cullObject.transform.position, playerObject.transform.position);
            Renderer m_Renderer = cullObject.GetComponent<Renderer>();
            //Calculate the distance from the player and the cullObject in the list.
            if (distance > playerDistance)
            {
                cullObject.SetActive(false);
                
            } 
            else
            {
                cullObject.SetActive(true);
            }

        }
    }
}
