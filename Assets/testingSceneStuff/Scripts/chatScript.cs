using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class chatScript : NetworkBehaviour
{

    public GameObject chatPanel, textObject;

    public GameObject inputFieldObject;

    public int maxMessages = 25;

    public List<Message> messages = new List<Message>();

    public InputField chatInput;

    public GameObject mainChatObject;

    public mainChatScript serverChatScript;

    Image inputImage;

    public bool typing = false;

    // Start is called before the first frame update
    void Start()
    {
        mainChatObject = GameObject.Find("mainChat(Clone)");
        serverChatScript = mainChatObject.GetComponent<mainChatScript>();
        inputImage = inputFieldObject.GetComponent<Image>();
        inputImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            typing = true;
            inputImage.enabled = true;
            chatInput.enabled = true;
            chatInput.ActivateInputField();
        } 

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (chatInput.text != null)
            {
                serverChatScript.CmdsendMessage(chatInput.text);

                chatInput.DeactivateInputField();
                chatInput.enabled = false;
                inputImage.enabled = false;
                typing = false;
            }
        }
    }

}



[System.Serializable]
public class Message
{
    public string message;
    public Text messageInstance;
}
