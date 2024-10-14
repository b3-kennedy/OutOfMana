using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.LowLevel;

public class DefaultEEQ : Spell
{
    [HideInInspector] public GameObject player;
    public GameObject minion;

    public void SpawnMinion()
    {
        SpawnMinionServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    void SpawnMinionServerRpc()
    {
        SpawnMinionClientRpc();
    }

    [ClientRpc]
    void SpawnMinionClientRpc()
    {
        if(player.GetComponent<SpriteRenderer>().color == Color.red)
        {
            var m = Instantiate(minion, player.GetComponent<SpellManager>().minionSpawn.position, Quaternion.Euler(0,0,0));
            m.GetComponent<MinionProjectile>().player = player;
        }
        else if(player.GetComponent<SpriteRenderer>().color == Color.blue)
        {
            var m = Instantiate(minion, player.GetComponent<SpellManager>().minionSpawn.position, Quaternion.Euler(0, 180, 0));
            m.GetComponent<MinionProjectile>().player = player;
        }

    }
}
