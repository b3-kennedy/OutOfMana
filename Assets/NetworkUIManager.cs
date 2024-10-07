using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkUIManager : MonoBehaviour
{
    public void Host()
    {
        NetworkManager.Singleton.StartHost();
        gameObject.SetActive(false);
    }

    public void Join()
    {
        NetworkManager.Singleton.StartClient();
        gameObject.SetActive(false);
    }
}
