using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : Debuff
{


    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<SpellManager>())
        {
            GetComponent<SpellManager>().canCast.Value = false;
            GetComponent<SpellManager>().ClearLocalOrbs();
            GetComponent<SpellManager>().ClearServerOrbsServerRpc();
            Destroy(this, duration);
        }
        
    }

    private void OnDestroy()
    {
        if (GetComponent<SpellManager>())
        {
            GetComponent<SpellManager>().canCast.Value = true;
        }
        
    }

}
