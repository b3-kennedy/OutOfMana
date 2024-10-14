using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Spell : NetworkBehaviour
{
    public float cooldown;
    public float manaCost;
    float cdTimer;
    public NetworkVariable<bool> onCd;
    public GameObject icon;
    public TextMeshProUGUI cdText;

    private void Update()
    {


        SpellUpdate();


        if (onCd.Value)
        {
            if(icon != null)
            {
                icon.GetComponent<Image>().color = Color.red;
            }
            
            cdText.gameObject.SetActive(true);
            cdText.text = String.Format("{0:0.0}",cooldown - cdTimer);
            cdTimer += Time.deltaTime;
            if (cdTimer >= cooldown)
            {
                cdTimer = 0;
                UpdateCdBoolServerRpc(false);
            }
        }
        else
        {
            if(icon != null)
            {
                icon.GetComponent<Image>().color = Color.green;
            }
            
            cdText.gameObject.SetActive(false);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void UpdateCdBoolServerRpc(bool value)
    {
        onCd.Value = value;
    }

    public virtual void SpellUpdate() { }


}
