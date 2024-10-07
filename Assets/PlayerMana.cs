using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMana : NetworkBehaviour
{
    public float maxMana;
    public NetworkVariable<float> currentMana;
    public NetworkVariable<float> manaPercentage;
    public GameObject manaBar;
    RectTransform manaBarRect;

    public override void OnNetworkSpawn()
    {
        manaPercentage.Value = 1;
        currentMana.Value = maxMana;
        manaBarRect = manaBar.GetComponent<RectTransform>();
    }

    public void UseMana(float dmg)
    {
        if (IsServer)
        {
            currentMana.Value -= dmg;
            if(currentMana.Value < 0)
            {
                currentMana.Value = 0;
            }
        }
       UseManaServerRpc(dmg);
    }

    public void GainMana(float mana)
    {
        if (IsServer)
        {
            currentMana.Value += mana;
            if(currentMana.Value > maxMana)
            {
                currentMana.Value = maxMana;
            }
        }
        GainManaServerRpc(mana);
    }

    private void Update()
    {
        if (manaBarRect.localScale.x != manaPercentage.Value)
        {
            UpdateBar();
        }


    }

    //fix client error
    void UpdateBar()
    {
        float value = Mathf.Lerp(manaBarRect.localScale.x, manaPercentage.Value, 10 * Time.deltaTime);
        manaBarRect.localScale = new Vector3(value, manaBarRect.localScale.y, manaBarRect.localScale.z);
    }


    [ServerRpc(RequireOwnership = false)]
    public void UseManaServerRpc(float dmg)
    {
        UpdateManaBarClientRpc();
    }

    [ClientRpc]
    public void UpdateManaBarClientRpc()
    {
        
        manaPercentage.Value = currentMana.Value / maxMana;
    }


    [ServerRpc(RequireOwnership = false)]
    public void GainManaServerRpc(float mana)
    {
        UpdateManaBarClientRpc();
    }
}
