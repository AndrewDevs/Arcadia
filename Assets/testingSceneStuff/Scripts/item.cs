using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Diagnostics;
using Mirror;

public class item : NetworkBehaviour
{

    public string itemName;
    public int itemNumber;
    public bool world;


    //Database uses ints, not strings so this is changed to an int.
    bool hitByRaycast;
}

