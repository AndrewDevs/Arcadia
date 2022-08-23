using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class craftButtonUI : MonoBehaviour
{

    public GameObject craftingClass;
    int amountClicked;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void checkAmountClicked()
    {
        amountClicked++;
        if(amountClicked == 1)
        {
            setClassActive();
        }
        else
        {
            setClassUnActive();
            amountClicked = 0;
        }
    }
    public void setClassActive()
    {
        craftingClass.SetActive(true);
    }
    public void setClassUnActive()
    {
        craftingClass.SetActive(false);
    }
}
