using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class Slot : MonoBehaviour
{
    public GameObject player;
    public Item item;
    public Image slotIcon;
    public int slotIndex;
    public int playerId;

    void Start()
    {
        slotIcon = GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(ActivateButton);
        OnEquip();
    }

    public void OnEquip()
    {
        if(item != null)
        {
            item.player = player;
            slotIcon.sprite = item.GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void ActivateButton()
    {
        if(item != null)
        {
            item.Use();
            player.GetComponent<ItemManager>().UseItemServerRpc(slotIndex, playerId);
        }
        
    }



}
