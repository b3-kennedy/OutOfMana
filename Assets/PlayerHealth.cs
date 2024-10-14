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

    public void GainHealth(float health)
    {
        if (IsServer)
        {
            currentHealth.Value += health;
            if(currentHealth.Value > maxHealth)
            {
                currentHealth.Value = maxHealth;
            }
        }
        GainHealthServerRpc(health);
    }

    [ServerRpc(RequireOwnership = false)]
    public void GainHealthFromPotionServerRpc(float health)
    {
        currentHealth.Value += health;
        if (currentHealth.Value > maxHealth)
        {
            currentHealth.Value = maxHealth;
        }
        GainHealthServerRpc(health);
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
        float value = Mathf.Lerp(healthBarRect.localScale.x, healthPercentage.Value, 10 * Time.deltaTime);
        healthBarRect.localScale = new Vector3(value, healthBarRect.localScale.y, healthBarRect.localScale.z);
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
    }


    [ServerRpc(RequireOwnership = false)]
    public void GainHealthServerRpc(float heal)
    {
        UpdateHealthBarClientRpc();
    }

}
