using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;


public class inventoryScript : MonoBehaviour
{

    public List<GameObject> inventoryObjects = new List<GameObject>();
    public List<GameObject> images = new List<GameObject>();

    public inventoryInfo[] inventory;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


[System.Serializable]
public class inventoryInfo
{

    public  GameObject itemImage;
    public GameObject itemObject;
    public int itemInSlot;

}
