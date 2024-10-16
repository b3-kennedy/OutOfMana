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
    public GameObject icon;
    public TextMeshProUGUI cdText;

    public NetworkVariable<bool> onCd = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    // Local cooldown timer for prediction purposes (only for client)
    private float predictedCdTimer = 0f;

    // Server cooldown timer (server authoritative)
    [HideInInspector] public float serverCdTimer = 0f;

    private void Update()
    {
        SpellUpdate();
        // Client-side cooldown prediction and visual updates
        if (onCd.Value)
        {
            UpdateCooldownPrediction();
        }
        else
        {
            ResetCooldownUI();
        }
    }

    private void UpdateCooldownPrediction()
    {
        // Update client-side cooldown visuals
        if (icon != null)
        {
            icon.GetComponent<Image>().color = Color.red;
        }

        // Show cooldown timer clamped to prevent negatives
        //cdText.gameObject.SetActive(true);
        //float timeRemaining = Mathf.Max(0, cooldown - predictedCdTimer);
        //cdText.text = string.Format("{0:0.0}", timeRemaining);

        // Increment the local predicted cooldown timer
        predictedCdTimer += Time.deltaTime;

        // Debugging

        // Request server confirmation when the client thinks the cooldown is over
        if (predictedCdTimer >= cooldown && IsClient)
        {
            RequestCooldownCompleteServerRpc();
        }
    }

    public void ResetCooldownUI()
    {
        // Reset the cooldown UI when the spell is ready again
        if (icon != null)
        {
            icon.GetComponent<Image>().color = Color.green;
        }
        cdText.gameObject.SetActive(false);
    }

    [ServerRpc(RequireOwnership = false)]
    public void TriggerCooldownServerRpc()
    {
        if (!onCd.Value)
        {
            // Start the cooldown on the server
            onCd.Value = true;
            serverCdTimer = 0f;  // Reset the server cooldown timer

            // Sync the client-side timer for prediction
            predictedCdTimer = 0f;  // Reset client timer for prediction

        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestCooldownCompleteServerRpc()
    {
        // The client is requesting cooldown completion; server must verify
        if (onCd.Value && serverCdTimer >= cooldown)
        {
            ResetCooldown();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestResetServerRpc()
    {
        ResetCooldown();
    }
    

    private void FixedUpdate()
    {
        // Server-only cooldown timing logic
        if (IsServer && onCd.Value)
        {
            serverCdTimer += Time.fixedDeltaTime;

            // Debugging

            // When the server cooldown is complete, reset it
            if (serverCdTimer >= cooldown)
            {
                ResetCooldown();
            }
        }
    }

    public void ResetCooldown()
    {
        // Reset cooldown on the server and all clients
        serverCdTimer = 0f;         // Reset server-side timer
        onCd.Value = false;         // Notify all clients that cooldown is over

        // Client's predicted timer will only reset on the next trigger
        // So it can be reused for another cooldown
    }


    public virtual void SpellUpdate() { }


}
