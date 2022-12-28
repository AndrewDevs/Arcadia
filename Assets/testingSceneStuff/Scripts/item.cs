using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Diagnostics;

public class item : MonoBehaviour
{

    public string itemName;
    public int itemNumber;
    public bool world;

    public bool food;
    public int foodValue;

    public bool water;
    public int waterValue;

    public int hardness;


    //Database uses ints, not strings so this is changed to an int.
    bool hitByRaycast;



}

