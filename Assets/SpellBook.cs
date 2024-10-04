using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpellBook : NetworkBehaviour
{
    public Transform player;

    public NetworkVariable<ulong> playerId = new NetworkVariable<ulong>();

    private void Update()
    {
        // Check if we are on the client and the player has not been assigned
        if (IsClient && player == null)
        {
            // Check if the player’s NetworkObject exists in the SpawnManager
            if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(playerId.Value, out var playerNetworkObject))
            {
                // Assign the player's Transform if the NetworkObject was found
                player = playerNetworkObject.transform;
            }
        }
    }


    public virtual void QQQ()
    {
        Debug.Log("QQQ");
    }

    public virtual void WWW()
    {
        Debug.Log("WWW");
    }

    public virtual void EEE()
    {
        Debug.Log("EEE");
    }

    public virtual void QQE()
    {
        Debug.Log("QQE");
    }

    public virtual void QQW()
    {
        Debug.Log("QQW");
    }

    public virtual void QEE()
    {
        Debug.Log("QEE");
    }

    public virtual void QWW()
    {
        Debug.Log("QWW");
    }

    public virtual void QWE()
    {
        Debug.Log("QWE");
    }

    public virtual void EWW()
    {
        Debug.Log("EWW");
    }

    public virtual void EEW()
    {
        Debug.Log("EEW");
    }

}
