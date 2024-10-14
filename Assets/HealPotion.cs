using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HealPotion : Item
{
    public float healAmount;
    public override void Use()
    {
        Debug.Log(player.GetComponent<SpriteRenderer>().color);
        player.GetComponent<PlayerHealth>().GainHealthFromPotionServerRpc(healAmount);
    }


}
