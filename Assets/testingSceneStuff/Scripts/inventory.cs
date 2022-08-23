using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class inventory : NetworkBehaviour
{
    public List<GameObject> inventoryObjects = new List<GameObject>();
    public List<GameObject> images = new List<GameObject>();

    public inventoryInformation[] Inventory;
    
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
public class inventoryInformation
{
    public GameObject itemImage;
    public GameObject itemObject;
    public int id;
}
