using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Spell : NetworkBehaviour
{
    public float cooldown;
    public float manaCost;
    float cdTimer;
    public bool onCd;

    private void Update()
    {
        SpellUpdate();
        if (onCd)
        {
            cdTimer += Time.deltaTime;
            if (cdTimer >= cooldown)
            {
                cdTimer = 0;
                onCd = false;
            }
        }
    }

    public virtual void SpellUpdate() { }


}
