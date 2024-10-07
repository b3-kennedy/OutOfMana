using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.Netcode;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{


    public NetworkVariable<Color> playerColour = new NetworkVariable<Color>();
    [HideInInspector] public Vector3 dir;
    public Vector3 otherPos;

    public override void OnNetworkSpawn()
    {

        if(OwnerClientId == 0)
        {
            playerColour.Value = Color.red;
            GetComponent<SpriteRenderer>().color = playerColour.Value;
            transform.position = new Vector3(-7.19f, -2.6f, 0f);
            otherPos = new Vector3(7.19f, -2.6f, 0f);
            dir = Vector3.right;
        }
        else
        {
            playerColour.Value = Color.blue;
            GetComponent<SpriteRenderer>().color = playerColour.Value;
            transform.position = new Vector3(7.19f, -2.6f, 0f);
            otherPos = new Vector3(-7.19f, -2.6f, 0f);
            transform.rotation = Quaternion.Euler(0, 180f, 0);
            Destroy(GetComponent<PingHolder>());
            dir = Vector3.left;
            

        }

        if (IsOwner)
        {
            SpawnClientBookServerRpc();
            if (OwnerClientId == 0)
            {
                GameManager.Instance.normalCam.SetActive(true);
                GameManager.Instance.flipCam.SetActive(false);
                GameManager.Instance.normalCam.GetComponent<AudioListener>().enabled = true;
                
            }
            else
            {
                GameManager.Instance.normalCam.SetActive(false);
                GameManager.Instance.flipCam.SetActive(true);
                GameManager.Instance.flipCam.GetComponent<AudioListener>().enabled = true;
                

            }
        }

        playerColour.OnValueChanged += UpdatePlayerColour;

    }

    [ClientRpc]
    void SetServerBookClientRpc()
    {
        var book = Instantiate(GetComponent<SpellManager>().book);
        GetComponent<SpellManager>().book = book;
        book.GetComponent<NetworkObject>().SpawnWithOwnership(0);
        book.player = transform;
    }

    [ServerRpc(RequireOwnership = false)]
    void SpawnClientBookServerRpc()
    {
        var book = Instantiate(GetComponent<SpellManager>().book);
        GetComponent<SpellManager>().book = book;
        book.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
        book.playerId.Value = GetComponent<NetworkObject>().NetworkObjectId;

        Debug.Log($"Spawning book for OwnerClientId: {OwnerClientId}");
    }

    void UpdatePlayerColour(Color oldColor, Color newColor)
    {
        GetComponent<SpriteRenderer>().color = newColor;
    }
}
