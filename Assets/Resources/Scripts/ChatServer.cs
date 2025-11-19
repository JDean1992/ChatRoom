using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class ChatServer : MonoBehaviour
{
    //the listener that waits for a client to connect and the coonected client
    private TcpListener server;
    private TcpClient client;

    //the server info
    private int port = 13000;
    private IPAddress serverIp = IPAddress.Parse("127.0.0.1");

    private bool isClientConnected = false;
    private bool isServerRunning = false;
    private Thread serverThread;

    private NetworkStream stream;
    private byte[] data;

    public static ChatServer Instance;
    
    void Awake()
    {
        Instance = this;
    }

    //called when the host presses the host button
    public void Host(string ip)
    {

        //this converts the ip string to a usable ip address
        serverIp = IPAddress.Parse(ip);
        // start  listening on the given ip address and port
        server = new TcpListener(serverIp, port);
        server.Start();
        isServerRunning = true;
      
        //starts background thread that waits for client and receives messages
        serverThread = new Thread(receiverThread);
        serverThread.Start();

    }

    // Update is called once per frame
    void Update()
    {

    }
    //Recieve messages
    void receiverThread()
    {
        while (isServerRunning)
        {
            //if no client has connected yet then it waits for a client
            if (isClientConnected == false)
            {
                client = server.AcceptTcpClient();
                
                isClientConnected = true;
                stream = client.GetStream();
            }
            else
            {
                //receive the messages
                data = new byte[256];
                string msg = string.Empty;
                //converts bytes into string
                int bytes = stream.Read(data, 0, data.Length);
                msg = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                //runs the code on the main thread
                UnityMainThreadDispatcher.Instance().Enqueue(() => ChatScreen.Instance.ShowMessage("Client: " + msg));
            }
            //a pause so the thread does not use all the cpu
            Thread.Sleep(100);
        }
    }

    //Send message logic

    public void Send(string msg)
    {

        byte[] data = System.Text.Encoding.ASCII.GetBytes(msg);
        stream.Write(data, 0, data.Length);

    }

    private void OnDisable()
    {
        isClientConnected = false;
        isServerRunning = false;
        stream.Close();
        server.Stop();
    }
}