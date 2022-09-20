using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class playerLocator : MonoBehaviour
{
    public GameObject player;
    public playerMovement playerScript;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = player.GetComponent<playerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void Return()
    {
        playerScript.pause = false;
        playerScript.pauseMenu.SetActive(false);
    }
    public void returnToMenu()
    {
        SceneManager.LoadScene("menu");
    }
    public void Options()
    {
        playerScript.options.SetActive(true);
        playerScript.option = true;
        //Set the option bool to true as our options screen is active.
    }

}
