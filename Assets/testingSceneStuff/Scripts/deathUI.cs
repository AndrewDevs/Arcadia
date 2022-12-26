using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathUI : MonoBehaviour
{

    public GameObject UI;
    public GameObject player;
    private CharacterController characterController;
    private playerMovement playerScript;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player(Clone)");
        characterController = player.GetComponent<CharacterController>();
        playerScript = player.GetComponent<playerMovement>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void closeScreen()
    {
        playerScript.deathUI = false;
        //Set the player's deathUI bool to false, so the cursor becomes invisible and locked.
        characterController.enabled = true;
        //Re-enable the characterController so player can move.
        UI.SetActive(false);
        //DeActivate the death UI.
    }


}
