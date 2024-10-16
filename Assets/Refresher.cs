using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Refresher : Item
{
    public override void Use()
    {
        ResetCooldownServerRpc(player.GetComponent<NetworkObject>().NetworkObjectId);

    }

    [ServerRpc(RequireOwnership = false)]
    void ResetCooldownServerRpc(ulong id)
    {
        NetworkObject obj;
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(id, out obj))
        {
            obj.GetComponent<SpellManager>().book.lastUsedSpell.RequestResetServerRpc();
        }
    }
}
