using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : NetworkBehaviour
{


    public NetworkVariable<Color> playerColour = new NetworkVariable<Color>();
    [HideInInspector] public Vector3 dir;
    public Vector3 otherPos;
    public Canvas playerCanvas;
    public GameObject itemSlots;
    

    public override void OnNetworkSpawn()
    {

        if(OwnerClientId == 0)
        {
            playerColour.Value = Color.red;
            GetComponent<SpriteRenderer>().color = playerColour.Value;
            transform.position = new Vector3(-7.19f, -2.6f, 0f);
            otherPos = new Vector3(7.19f, -2.6f, 0f);
            dir = Vector3.right;
            playerCanvas.worldCamera = GameManager.Instance.normalCam.GetComponent<Camera>();
            var slots = Instantiate(itemSlots, GameManager.Instance.itemCanvas.transform.GetChild(0));
            slots.GetComponent<RectTransform>().localPosition = new Vector2(-486, -2);
            var slotParent = slots.transform.GetChild(1);
            for (int i = 0; i < slotParent.childCount; i++)
            {
                slotParent.GetChild(i).GetComponent<Slot>().player = gameObject;
                slotParent.GetChild(i).GetComponent<Slot>().slotIndex = i;
                slotParent.GetChild(i).GetComponent<Slot>().playerId = 0;
            }

        }
        else
        {
            playerColour.Value = Color.blue;
            GetComponent<SpriteRenderer>().color = playerColour.Value;
            transform.position = new Vector3(7.19f, -2.6f, 0f);
            otherPos = new Vector3(-7.19f, -2.6f, 0f);
            transform.rotation = Quaternion.Euler(0, 180f, 0);
            Destroy(GetComponent<PingHolder>());
            playerCanvas.worldCamera = GameManager.Instance.flipCam.GetComponent<Camera>();
            dir = Vector3.left;
            var slots = Instantiate(itemSlots, GameManager.Instance.itemCanvas.transform.GetChild(0));
            slots.GetComponent<RectTransform>().localPosition = new Vector2(481, -2);
            var slotParent = slots.transform.GetChild(1);
            for (int i = 0; i < slotParent.childCount; i++)
            {
                slotParent.GetChild(i).GetComponent<Slot>().player = gameObject;
                slotParent.GetChild(i).GetComponent<Slot>().slotIndex = i;
                slotParent.GetChild(i).GetComponent<Slot>().playerId = 1;
            }

            //slots.GetComponent<RectTransform>().localPosition = new Vector2(296, -350); right
            //slots.GetComponent<RectTransform>().localPosition = new Vector2(-670, -350); left
        }
        if(GameManager.Instance.itemCanvas.transform.GetChild(0).childCount > 1)
        {
            GameManager.Instance.itemCanvas.transform.GetChild(0).GetChild(1).rotation = Quaternion.Euler(0f, 180f, 0f);
        }


        if (IsOwner)
        {
            SpawnClientBookServerRpc();
            if (OwnerClientId == 0)
            {
                GameManager.Instance.normalCam.SetActive(true);
                GameManager.Instance.flipCam.SetActive(false);
                GameManager.Instance.normalCam.tag = "MainCamera";
                GameManager.Instance.flipCam.tag = "Untagged";
                GameManager.Instance.normalCam.GetComponent<AudioListener>().enabled = true;



                //var enemySlots = Instantiate(itemSlots, GameManager.Instance.itemCanvas.transform);
                //enemySlots.GetComponent<RectTransform>().localPosition = new Vector2(296, -350);
                //var enemySlotParent = enemySlots.transform.GetChild(1);
                //for (int i = 0; i < enemySlotParent.childCount; i++)
                //{
                //    enemySlotParent.GetChild(i).GetComponent<Button>().enabled = false;
                //}

            }
            else
            {
                GameManager.Instance.normalCam.SetActive(false);
                GameManager.Instance.flipCam.SetActive(true);
                GameManager.Instance.flipCam.tag = "MainCamera";
                GameManager.Instance.normalCam.tag = "Untagged";
                GameManager.Instance.flipCam.GetComponent<AudioListener>().enabled = true;
                GameManager.Instance.itemCanvas.transform.GetChild(0).rotation = Quaternion.Euler(0f, 180f, 0f);


                //var enemySlots = Instantiate(itemSlots, GameManager.Instance.itemCanvas.transform);
                //enemySlots.GetComponent<RectTransform>().localPosition = new Vector2(296, -350);
                //var enemySlotParent = enemySlots.transform.GetChild(1);
                //for (int i = 0; i < enemySlotParent.childCount; i++)
                //{
                //    enemySlotParent.GetChild(i).GetComponent<Button>().enabled = false;
                //}



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
