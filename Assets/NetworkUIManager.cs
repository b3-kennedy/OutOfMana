using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUIManager : NetworkBehaviour
{
    public GameObject leavePanel;
    public Button leaveButton;
    public GameObject connectButtons;


    private void Start()
    {
        NetworkManager.Singleton.OnServerStopped += DisconnectUI;
        NetworkManager.Singleton.OnClientStopped += DisconnectUI;
    }


    public void Host()
    {
        NetworkManager.Singleton.StartHost();
        connectButtons.SetActive(false);
        leaveButton.onClick.AddListener(Leave);
    }

    public void Join()
    {
        NetworkManager.Singleton.StartClient();
        connectButtons.SetActive(false);
        leaveButton.onClick.AddListener(Leave);
    }

    void Leave()
    {
        NetworkManager.Singleton.Shutdown();
        connectButtons.SetActive(true);
        leavePanel.SetActive(false);
    }

    void DisconnectUI(bool stopped)
    {
        connectButtons.SetActive(true);
        leavePanel.SetActive(false);
    }



    private void Update()
    {
        if (!NetworkManager.Singleton.IsConnectedClient) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            leavePanel.SetActive(!leavePanel.activeSelf);
        }
    }
}
