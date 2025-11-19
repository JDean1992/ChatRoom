using TMPro;
using UnityEngine;

public class MainMenuScreen : MonoBehaviour
{

    public TMP_InputField IPAddressInput;
    public GameObject chatScreen;
    public GameObject JoinScreen;

    public static bool IsServer = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //hides the chat screen until the user has has chosen host or join
        chatScreen.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {

    }

    //this function is called when the user clicks the host button
    public void HostButtonClicked()
    {
        IsServer = true;
        //starts the server using the ip address from the input field
        ChatServer.Instance.Host(IPAddressInput.text);
        //shows chat screen and hides this menu
        chatScreen.SetActive(true);
        JoinScreen.SetActive(false);

    }

    //this function is called when the user clicks the join button
    public void JoinButtonClicked()
    {
        IsServer = false;
        //starts the client and connects to the server at the given IP address
        ChatClient.Instance.Join(IPAddressInput.text);
        chatScreen.SetActive(true);
        JoinScreen.SetActive(false);
    }
}