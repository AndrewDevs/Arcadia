using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class timeScript : NetworkBehaviour
{
    public int abstractTime;


    // Start is called before the first frame update
    void Start()
    {
        if(isServer == true)
        {
            StartCoroutine(objectiveTimeFunction());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator objectiveTimeFunction()
    {
        yield return new WaitForSeconds(1);
        abstractTime++;


        RpcChangeSeason(abstractTime);
        if(abstractTime > 240)
        {
            abstractTime = 0;
        }
        
        

        StartCoroutine(objectiveTimeFunction());


    }


    [ClientRpc]
    public void RpcChangeSeason(int objectiveTime)
    {
        GameObject playerSeasonObject = GameObject.Find("seasonObject(Clone)");
        playerSeasonInfo playerSeasonScript = playerSeasonObject.GetComponent<playerSeasonInfo>();
        playerSeasonScript.changeSeason(objectiveTime);



    }



}
