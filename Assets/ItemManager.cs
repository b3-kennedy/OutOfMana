using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ItemManager : NetworkBehaviour
{

    public List<Slot> itemSlots;

    // Start is called before the first frame update
    void Start()
    {

    }

    public override void OnNetworkSpawn()
    {
        //foreach (var slot in itemSlots)
        //{
        //    slot.player = gameObject;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ServerRpc(RequireOwnership = false)]
    public void UseItemServerRpc(int slotIndex, int ownerIndex)
    {
        UseItemClientRpc(slotIndex, ownerIndex);
    }

    [ClientRpc]
    void UseItemClientRpc(int slotIndex, int ownerIndex)
    {
        var parent = GameManager.Instance.itemCanvas.transform.GetChild(0);
        Debug.Log(parent.GetChild(ownerIndex).GetChild(1).GetChild(slotIndex).name);
        parent.GetChild(ownerIndex).GetChild(1).GetChild(slotIndex).GetComponent<Slot>().item = null;
        parent.GetChild(ownerIndex).GetChild(1).GetChild(slotIndex).GetComponent<Slot>().slotIcon.sprite = null;
    }
}
