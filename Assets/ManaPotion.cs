using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotion : Item
{
    public float manaGain;

    public override void Use()
    {
        player.GetComponent<PlayerMana>().GainManaFromPotionServerRpc(manaGain);
    }
}
