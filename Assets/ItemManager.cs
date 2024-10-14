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
}
