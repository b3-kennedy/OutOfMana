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
        }
       UseManaServerRpc(dmg);
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
        manaBarRect.localScale = new Vector3(manaPercentage.Value, manaBarRect.localScale.y, manaBarRect.localScale.z);
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
        manaBarRect.localScale = new Vector3(manaPercentage.Value, manaBarRect.localScale.y, manaBarRect.localScale.z);
    }


    [ServerRpc(RequireOwnership = false)]
    public void GainGainServerRpc(float heal)
    {
        currentMana.Value += heal;
    }
}
