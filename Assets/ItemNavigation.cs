using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemNavigation : NetworkBehaviour, IPointerDownHandler
{

    public int id;
    public override void OnNetworkSpawn()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(id == 0)
        {
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                var obj = eventData.pointerCurrentRaycast.gameObject;
                if (obj.GetComponent<Slot>() && obj.GetComponent<Slot>().item != null)
                {
                    obj.GetComponent<Slot>().ActivateButton();
                }
            }
        }
        else if(id == 1)
        {
            var posX = Screen.width - eventData.position.x;
            Debug.Log(posX);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
