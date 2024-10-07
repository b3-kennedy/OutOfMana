using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DefaultQQE : Spell
{
    public float duration;
    public GameObject obscureObject;
    [HideInInspector] public SpellBook book;
    public void OnActivate()
    {
            if(book.player.GetComponent<SpriteRenderer>().color == Color.red)
            {
                SpawnObsureServerRpc(1, 1);
                Debug.Log("red");
            }
            else if(book.player.GetComponent<SpriteRenderer>().color == Color.blue)
            {
                Debug.Log("blue");
                SpawnObsureServerRpc(0,0);
            }
    }

    [ServerRpc(RequireOwnership = false)]
    void SpawnObsureServerRpc(ulong target, ulong id) 
    {
        ObscureClientVisionClientRpc(id,new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { target }
            }
        });
    }

    [ClientRpc]
    void ObscureClientVisionClientRpc(ulong id, ClientRpcParams clientRpcParams = default)
    {
        GameObject obj = Instantiate(obscureObject);
        Destroy(obj, duration);
        if(id == 1)
        {
            obj.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }

    }
}
