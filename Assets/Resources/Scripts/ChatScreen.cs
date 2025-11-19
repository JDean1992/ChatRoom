using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class ChatScreen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public TMP_InputField ChatInput;
    public TextMeshProUGUI ChatText;
    public TextMeshProUGUI ChatText1;
    public static ChatScreen Instance;
    public AudioSource AudioSource;
    public AudioClip MessageReceivedSound;
    

    private int MaxMessages = 10;
    private List<string> MessageList = new List<string>();

    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SendButtonClicked()
    {

        // if you're the server  it will send messages to all clients then show the message locally on your screen without playing
        //the messasge sound and then clear the input field otherwise it does the same thing for the client
        if (MainMenuScreen.IsServer)

        {
            ChatServer.Instance.Send(ChatInput.text);
            ShowMessage("Server : " + ChatInput.text, false);
            ChatInput.text = "";
        }
        else
        {
            ChatClient.Instance.Send(ChatInput.text);
            ShowMessage("Client : " + ChatInput.text, false);
            ChatInput.text = "";
            ChatInput.text = "";
        }

    }

    // shows a message in the chat window
    public void ShowMessage(string text, bool playSound = true)
    {
        // Add new message to list
        MessageList.Add(text);

        // Limit number of messages and deletes the oldest one
        while (MessageList.Count > MaxMessages)
        {
            MessageList.RemoveAt(0);
        }
        // Rebuild the chat text
        ChatText.text = string.Join("\n", MessageList);

        // Play sound for received message if the sound exists
        if (playSound && AudioSource != null && MessageReceivedSound != null)
            AudioSource.PlayOneShot(MessageReceivedSound);

        
    }

    
    


}