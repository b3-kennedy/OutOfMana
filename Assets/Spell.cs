using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public float cooldown;
    public float manaCost;
    float cdTimer;
    public bool onCd;

    private void Update()
    {
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


}
