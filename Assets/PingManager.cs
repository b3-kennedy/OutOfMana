using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PingManager : NetworkBehaviour
{

    public static PingManager Instance;

    public ulong clientPing;

    private void Start()
    {
        Instance = this;
    }


    private void Update()
    {
        if (!IsServer)
        {

            GetClientPingServerRpc(NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetCurrentRtt(NetworkManager.Singleton.NetworkConfig.NetworkTransport.ServerClientId));
        }
    }


    [ServerRpc(RequireOwnership = false)]
    void GetClientPingServerRpc(ulong ping)
    {
        clientPing = ping;
        PingHolder.Instance.ping = ping;
    }
}
