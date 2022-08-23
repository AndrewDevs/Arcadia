using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using static Message;
using static playerInfo;

public class mainChatScript : NetworkBehaviour
{

    worldGenerator worldGenScript;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    [Command(requiresAuthority = false)]
    public void CmdsendMessage(string Text, NetworkConnectionToClient client = null)
    {
        GameObject textObject = GameObject.Find("Text(Clone)");

        GameObject player = client.identity.gameObject;
        //Get the client gameObject.

        GameObject worldGenObject = GameObject.Find("worldGenerator(Clone)");
        worldGenScript = worldGenObject.GetComponent<worldGenerator>();

        string playerUsername = new string(new char[] { });
        //Create a new string to put our player username in.
        foreach (playerInfo playerInstance in worldGenScript.players) //Look in our player sync list in worldGenScript, which contains each player on the server, and try to find the player gameObject we got from the networkConnection (the player that's sending the message).
        {
            if(playerInstance.player == player)
            {
                playerUsername = playerInstance.username;
                //When we find them, set the playerUsername string to that player's username, this will be used for when we send the text.
                
            }
        }


        RpcsendMessage(Text, textObject, playerUsername);
        //We do this as an rpc as the chat UI is itself not networked.

    }

    [ClientRpc]
    public void RpcsendMessage(string text, GameObject textObject, string username)
    {

        Message newMessage = new Message();
        //Creat new message type.

        newMessage.message = "<" + username + "> " + text;
        //Format the message.
        GameObject content = GameObject.Find("Content");

        GameObject chatBox = GameObject.Find("chatBox");

        chatScript playerChatScript = chatBox.GetComponent<chatScript>();
        //This script will be used to get the textObject prebab.

        GameObject playerInputField = GameObject.Find("InputField");

        InputField playerInput = playerInputField.GetComponent<InputField>();
        //We grab the inputField so we can set it to null after the message has been set.


        GameObject newMessageInstance = Instantiate(playerChatScript.textObject, content.transform);
        //Create the actual message instance that will appear in player's chats, and set it's parent to "content.transform" - which is itself the actual content for out scrollrect.
        newMessage.messageInstance = newMessageInstance.GetComponent<Text>();
        //Set the newMessage messageInstance to our instantiated messageInstance, we will add this to the messagesList for regulation on the amount of messages a player sees.
        newMessage.messageInstance.text = newMessage.message;
        //Set the text of this newMessage, which will be added to the message list, to the actual text.
        if (username == "Mason")
        {
            newMessage.messageInstance.color = Color.red;
        }
        else
        {
            newMessage.messageInstance.color = Color.white;
        }

        playerChatScript.messages.Add(newMessage);
        //Add the message type to the list for regulation.

        playerInput.text = null;
        //Set the inputField to null, so the typed message dissapears and it put into the chat.



    }
}
