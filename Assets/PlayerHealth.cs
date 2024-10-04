using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    public float maxHealth;
    public NetworkVariable<float> currentHealth;
    public NetworkVariable<float> healthPercentage;
    public GameObject healthBar;
    RectTransform healthBarRect;

    public override void OnNetworkSpawn()
    {
        healthPercentage.Value = 1;
        currentHealth.Value = maxHealth;
        healthBarRect = healthBar.GetComponent<RectTransform>();
    }

    public void TakeDamage(float dmg)
    {
        if (IsServer)
        {
            currentHealth.Value -= dmg;
        }
        TakeDamageServerRpc(dmg);
    }

    private void Update()
    {
        if (healthBarRect.localScale.x != healthPercentage.Value)
        {
            UpdateBar();
        }
    }

    //fix client error
    void UpdateBar()
    {
        healthBarRect.localScale = new Vector3(healthPercentage.Value, healthBarRect.localScale.y, healthBarRect.localScale.z);
    }


    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(float dmg)
    {
        UpdateHealthBarClientRpc();
    }

    [ClientRpc]
    public void UpdateHealthBarClientRpc()
    {
        healthPercentage.Value = currentHealth.Value / maxHealth;
        healthBarRect.localScale = new Vector3(healthPercentage.Value, healthBarRect.localScale.y, healthBarRect.localScale.z);
    }


    [ServerRpc(RequireOwnership = false)]
    public void GainHealthServerRpc(float heal)
    {
        currentHealth.Value += heal;
    }
}
