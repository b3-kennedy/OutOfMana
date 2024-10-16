using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultWWE : Spell
{
    public float chargeTime;
    float timer;
    [HideInInspector] public DefaultSpellBook book;
    bool charge = false;

    public void BeginCharge()
    {
        charge = true;
    }

    public override void SpellUpdate()
    {
        if (charge)
        {
            timer += Time.deltaTime;
            if (timer > chargeTime)
            {
                if (book.player.GetComponent<SpriteRenderer>().color == Color.blue)
                {
                    book.SpawnProjectileServerRpc(3, -0.5f, 0);
                }
                else
                {
                    book.SpawnProjectileServerRpc(3, 0.5f, 0);
                }
                
                timer = 0;
                charge = false;
            }
        }
    }
}
